using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    [SerializeField] private Image barraVida;

    public void ActualizarBarra(int vidaActual, int vidaMaxima)
    {
        barraVida.fillAmount = (float)vidaActual / vidaMaxima;
    }
}
