using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public GameObject aimCircle;
    public float aimRange = 3.0f;

    void Update()
    {
        // input from joystick
       // float horizontal = Input.GetAxis("RightStickHorizontal");
       // float vertical = Input.GetAxis("RightStickVertical");

       // // show the aim circle
       // aimCircle.SetActive(true);

       // // get the direction of the aim
       // Vector2 aimDirection = new Vector2(horizontal, vertical).normalized;
       // Debug(aimDirection);


       // // put the aim circle on the scene
       //// Vector2 aimIndicatorPosition = transform.position + aimDirection * aimRange;
       // aimCircle.transform.position = aimIndicatorPosition;
    }
}