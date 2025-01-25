using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Rigidbody2D body;
    public Vector2 move;
    public float horizontal;
    public float vertical;

    float moveLimiter = 0.7f;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
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


}
