using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMan : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D hit)
    {
        // kill player
        if (hit.gameObject.CompareTag("Player") && !hit.collider.gameObject.CompareTag("PlayerFeet"))
        {
            PlayerMovement.PM.Hurt();
        }
    }
}
