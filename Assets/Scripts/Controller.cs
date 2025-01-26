using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum Phase { One = 1, Two = 2 }

public class Controller : MonoBehaviour
{
    Rigidbody2D body;

    public float horizontal;
    public float vertical;
    public float speed = 2f;
    public bool dead = false;
    public bool freeze = false;

    public int health = 3;          //IVE SET HEALTH TO 3
    public int attackDamage = 1;

    public GameObject bullet;
    private Rigidbody2D bulletBody;
    public float aimHorizontal;
    public float aimVertical;
    public float bulletSpeed = 4f;
    public GameObject aimPointer;

    public Controller opponent;
    public string playerName;

    public GameObject bubbleMachine;
    public GameObject BubbleWall;
    public int carrying = 0;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI wallsText;
    public TextMeshProUGUI phaseTwoText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI grenadesText;

    public GameObject bubbleMixes;
    public GameObject wall;
    public GameObject shadowWall;
    public GameObject grenade;

    public GameObject grenades;
    public GameObject attackBubbles;

    public int grenadeCount = 0;
   
   
    public Phase phase;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bulletBody = bullet.GetComponent<Rigidbody2D>();
        bullet.SetActive(false);
        phase = Phase.One;
        healthText.text = "Health: " + health.ToString();
        attackText.text = "Attack: " + attackDamage.ToString();
        wallsText.text = "Walls: " + carrying.ToString();
        grenadesText.text = "Grenades: " + grenadeCount.ToString();
        phaseTwoText.enabled = false;
        gameOverText.enabled = false;
        winnerText.enabled = false;
        shadowWall.SetActive(false);

        Invoke(nameof(StartPhaseTwo), 10f); //FOR TESTING, CHANGE BACK TO 60!
        Invoke(nameof(Hide), 13f);
    }

    private void StartPhaseTwo()
    {
        phase = Phase.Two;
        phaseTwoText.enabled = true;
        freeze = true;
        bubbleMixes.SetActive(false);
        wall.SetActive(false);
        grenades.SetActive(false);
        attackBubbles.SetActive(false);
    }
    private void Hide()
    {
        phaseTwoText.enabled = false;
        freeze = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Current player collided with opponent bullet
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject!=bullet)
        {
            TakeDamage();
        } 
        else if (collision.gameObject.CompareTag("BubbleMachine") && phase==Phase.One && carrying < 5)
        {
            //pick up bubble machine
            bubbleMachine = collision.gameObject;
            carrying += 1;
            wallsText.text = "Walls: " + carrying.ToString();
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Rocket") && phase == Phase.One)
        {
            //pick up Rocket
            grenadeCount += 1;
            grenadesText.text = "Grenades: " + grenadeCount.ToString();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BubbleVial"))
        {
            health += 1;
            healthText.text = "Health: " + health.ToString();
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("AttackBubble"))
        {
            attackDamage += 1;
            attackText.text = "Attack: " + attackDamage.ToString();
            collision.gameObject.SetActive(false);
        }
    }


    private void TakeDamage()
    {
        health -= opponent.attackDamage;
        opponent.bullet.SetActive(false);
        if (health <= 0)
        {
            health = 0;
            dead = true;
            winnerText.text = playerName + " WINS!";
            gameOverText.enabled = true;
            winnerText.enabled = true;
            freeze = true;
            IEnumerator loadMenu(float time)
            {
                yield return new WaitForSeconds(time);
                SceneManager.LoadScene("Menu");
            }
            StartCoroutine(loadMenu(3));
        }
        //update UI
        healthText.text = "Health: " + health.ToString();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        //movement
        if (!freeze)
        {
             body.velocity = new Vector2(horizontal * speed, vertical * speed);
        }

        //aim
        aimPointer.transform.localPosition = new Vector2(aimHorizontal, aimVertical);

        //aimPointer angle
        if (aimHorizontal != 0 || aimVertical != 0)
        {
            float angle = Mathf.Atan2(aimVertical, aimHorizontal) * Mathf.Rad2Deg + 90;
            aimPointer.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        //shadow wall to see where you'll place a wall
        if (phase == Phase.One && carrying > 0 && Math.Sqrt(Math.Pow(aimHorizontal, 2) + Math.Pow(aimVertical, 2)) > 0.5)
        {
            shadowWall.SetActive(true);
            shadowWall.transform.localPosition = new Vector2(aimHorizontal, aimVertical);
            shadowWall.transform.rotation = aimPointer.transform.rotation;
        }
        else {
            shadowWall.SetActive(false);
        }
    }


    private void OnMove(InputValue value)
    {
        horizontal = value.Get<Vector2>().x;
        vertical = value.Get<Vector2>().y;
    }

    private void OnLook(InputValue value)
    {
        aimHorizontal = value.Get<Vector2>().x;
        aimVertical = value.Get<Vector2>().y;
    }

    private void OnFire(InputValue value)
    {
        if (freeze)
            return;

        if (phase == Phase.One && carrying > 0)
        {
            //place wall
            if (Math.Sqrt(Math.Pow(aimHorizontal,2)+Math.Pow(aimVertical,2))>0.5) //this makes them not place on top of themself
            {
                GameObject newObject = Instantiate(BubbleWall, new Vector2(transform.position.x + aimHorizontal, transform.position.y + aimVertical), aimPointer.transform.rotation);
                carrying -= 1;
                wallsText.text = "Walls: " + carrying.ToString();
            }
        }

        if (phase == Phase.Two)
        {
            //shoot in direction of aim.
            bullet.transform.position = new Vector2(transform.position.x + aimHorizontal, transform.position.y + aimVertical);
            bullet.SetActive(true);
            bulletBody.velocity = (new Vector2(aimHorizontal * bulletSpeed, aimVertical * bulletSpeed));
        }     
    }

    private void OnThrow(InputValue value)
    {
        if (phase == Phase.Two && grenadeCount > 0)
        {
            //shoot in direction of aim.
            grenadeCount -= 1;
            grenadesText.text = "Grenades: " + grenadeCount.ToString();
            GameObject newGrenade = Instantiate(grenade, new Vector2(transform.position.x + aimHorizontal, transform.position.y + aimVertical), aimPointer.transform.rotation);
            newGrenade.GetComponent<Rigidbody2D>().velocity = (new Vector2(aimHorizontal * bulletSpeed, aimVertical * bulletSpeed));
            Destroy(newGrenade, 2f);
        }
    }


}


//countdown timer for phase


//nicer background

//upload to game jam site