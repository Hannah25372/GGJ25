using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimPointer : MonoBehaviour
{
    public float aimHorizontal;
    public float aimVertical;

    private void OnLook(InputValue value)
    {
        aimHorizontal = value.Get<Vector2>().x;
        aimVertical = value.Get<Vector2>().y;
    }

    void FixedUpdate()
    {
        transform.localPosition = new Vector2(aimHorizontal, aimVertical);
    }
}