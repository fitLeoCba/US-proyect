using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LeoVida leo = collision.GetComponent<LeoVida>();
            if (leo != null)
            {
                leo.Morir(); // Muerte instantánea
            }
        }
    }
}
