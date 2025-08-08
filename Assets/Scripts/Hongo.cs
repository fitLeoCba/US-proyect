using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hongo : MonoBehaviour
{
    [SerializeField] private GameObject prefabEspora;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float distanciaActivacion = 6f;
    [SerializeField] private float tiempoEntreDisparos = 2f;

    private Transform jugador;
    private Animator anim;
    private float tiempoSiguienteDisparo;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        tiempoSiguienteDisparo = Time.time;

        if (puntoDisparo == null)
        {
            Debug.LogError("¡No se asignó el Punto Disparo al hongo!", this);
        }
    }

    private void Update()
    {
        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= distanciaActivacion && Time.time >= tiempoSiguienteDisparo)
        {
            anim.SetTrigger("Disparando");
            tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    // Animation Event
    public void LanzarEspora()
    {
        Instantiate(prefabEspora, puntoDisparo.position, Quaternion.identity);
    }


}
