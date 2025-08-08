using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Text textoMonedas;
    [SerializeField] private int totalMonedas = 5; // Las que haya en el nivel
    private int monedasActuales = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SumarMoneda()
    {
        monedasActuales++;
        textoMonedas.text = monedasActuales.ToString();

        if (monedasActuales >= totalMonedas)
        {
            Debug.Log("¡Todas las monedas obtenidas!");
            ReiniciarPartida();
        }
    }

    private void ReiniciarPartida()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
