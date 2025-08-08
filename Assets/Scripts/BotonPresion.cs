using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonPresion : MonoBehaviour
{
    [SerializeField] private PlataformaMovil plataforma;

    private int objetosSobreBoton = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Caja"))
        {
            objetosSobreBoton++;
            plataforma.Activar(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Caja"))
        {
            objetosSobreBoton--;
            if (objetosSobreBoton <= 0)
            {
                plataforma.Activar(false);
            }
        }
    }
}
