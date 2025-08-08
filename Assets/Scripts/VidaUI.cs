using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaUI : MonoBehaviour
{
    [SerializeField] private Slider barra;

    public void ActualizarBarra(int vidaActual, int vidaMaxima)
    {
        barra.value = (float)vidaActual / vidaMaxima;
    }
}
