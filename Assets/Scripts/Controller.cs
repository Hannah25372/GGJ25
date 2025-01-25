using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    Rigidbody2D body;

    public float horizontal;
    public float vertical;
    float moveLimiter = 0.7f;
    public float speed = 2f;
    public bool dead = false;
    public bool freeze = false;

    public int health = 5;
    public int attackDamage = 1;

    public GameObject bullet;
    private Rigidbody2D bulletBody;
    public float aimHorizontal;
    public float aimVertical;
    public float bulletSpeed = 4f;
    public GameObject aimPointer;

    public Controller opponent;

    public GameObject bubbleMachine;
    public GameObject BubbleWall;
    public bool carrying = false;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI phaseTwoText;

    public GameObject bubbleMixes;
    public GameObject wall;

    enum Phase { One = 1, Two = 2}
    Phase phase;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bulletBody = bullet.GetComponent<Rigidbody2D>();
        bullet.SetActive(false);
        phase = Phase.One;
        healthText.text = "Health: " + health.ToString();
        attackText.text = "Attack: " + attackDamage.ToString();
        phaseTwoText.enabled = false;

        Invoke(nameof(StartPhaseTwo), 60f);
        Invoke(nameof(Hide), 65f);
    }

    private void StartPhaseTwo()
    {
        phase = Phase.Two;
        phaseTwoText.enabled = true;
        freeze = true;
        bubbleMixes.SetActive(false);
        wall.SetActive(false);
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
        else if (collision.gameObject.CompareTag("BubbleMachine") && phase==Phase.One && !carrying)
        {
            //pick up bubble machine
            bubbleMachine = collision.gameObject;
            carrying = true;
            collision.gameObject.SetActive(false);
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
        }
        //update UI
        healthText.text = "Health: " + health.ToString();
    }

    void FixedUpdate()
    {
        //movement
        if (!freeze)
        {
             if (horizontal != 0 && vertical != 0) // Check for diagonal movement
             {
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
             }
             body.velocity = new Vector2(horizontal * speed, vertical * speed);
        }

        //aim
        aimPointer.transform.localPosition = new Vector2(aimHorizontal, aimVertical);

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

        if (phase == Phase.One && carrying)
        {
            //place bubble machine
            GameObject newObject = Instantiate(BubbleWall, new Vector2(transform.position.x + aimHorizontal, transform.position.y + aimVertical), Quaternion.identity);
            carrying = false;

        }
        if (phase == Phase.Two)
        {
            //shoot in direction of aim.
            bullet.transform.position = new Vector2(transform.position.x + aimHorizontal, transform.position.y + aimVertical);
            bullet.SetActive(true);
            bulletBody.velocity = (new Vector2(aimHorizontal * bulletSpeed, aimVertical * bulletSpeed));
        }     
    }


}
