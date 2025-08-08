using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esporas : MonoBehaviour
{
    [SerializeField] private int da�o = 1;
    [SerializeField] private float intervaloDa�o = 1f;

    private bool jugadorDentro = false;
    private LeoVida leo;
    private float tiempoProximoDa�o;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            leo = collision.GetComponent<LeoVida>();
            if (leo != null)
            {
                jugadorDentro = true;
                tiempoProximoDa�o = Time.time;
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
        if (jugadorDentro && leo != null && Time.time >= tiempoProximoDa�o)
        {
            leo.RecibirDa�o(da�o, transform.position);
            tiempoProximoDa�o = Time.time + intervaloDa�o;
        }
    }
}
