using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Rigidbody2D body;

    public float horizontal;
    public float vertical;
    public float aimHorizontal;
    public float aimVertical;
    public bool fired;

    float moveLimiter = 0.7f;
    public float speed = 2f;

    public GameObject bullet;
    private Rigidbody2D bulletBody;
    public float bulletSpeed = 4f;

    public GameObject enemyBullet;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bulletBody = bullet.GetComponent<Rigidbody2D>();
        bullet.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("HIT");
        }
    }

    void FixedUpdate()
    {
        //movement
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        body.velocity = new Vector2(horizontal * speed, vertical * speed);

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
        //shoot in direction of aim.
        bullet.transform.position = new Vector2(transform.position.x + aimHorizontal, transform.position.y + aimVertical);
        bullet.SetActive(true);
        bulletBody.velocity = (new Vector2(aimHorizontal * bulletSpeed, aimVertical * bulletSpeed));
    }


}
