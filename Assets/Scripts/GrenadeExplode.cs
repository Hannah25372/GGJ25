using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplode : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BubbleWall"))
        {
            gameObject.GetComponent<Animator>().SetBool("pin", true);
            Destroy(gameObject, 0.2f);
        }

    }
}
