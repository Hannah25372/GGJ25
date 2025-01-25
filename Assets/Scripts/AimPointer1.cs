using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{

    Rigidbody2D body;

    public float horizontal;
    public float vertical;
    float moveLimiter = 0.7f;
    public float speed = 2f;
    public float aimHorizontal;
    public float aimVertical;

    public GameObject aimCircle;
    public float aimRange = 3.0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
    }

    void FixedUpdate()
    {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }
            body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private void OnLook(InputValue value)
    {
        aimHorizontal = value.Get<Vector2>().x;
        aimVertical = value.Get<Vector2>().y;
    }


    private void OnMove(InputValue value)
    {
        horizontal = value.Get<Vector2>().x;
        vertical = value.Get<Vector2>().y;
    }
}