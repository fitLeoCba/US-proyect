using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esporas : MonoBehaviour
{
    [SerializeField] private int daño = 1;
    [SerializeField] private float intervaloDaño = 1f;

    private bool jugadorDentro = false;
    private LeoVida leo;
    private float tiempoProximoDaño;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            leo = collision.GetComponent<LeoVida>();
            if (leo != null)
            {
                jugadorDentro = true;
                tiempoProximoDaño = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorDentro = false;
            leo = null;
        }
    }

    private void Update()
    {
        if (jugadorDentro && leo != null && Time.time >= tiempoProximoDaño)
        {
            leo.RecibirDaño(daño, transform.position);
            tiempoProximoDaño = Time.time + intervaloDaño;
        }
    }
}
