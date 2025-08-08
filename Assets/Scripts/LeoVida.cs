using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeoVida : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private Transform puntoReaparicion;
    [SerializeField] private float fuerzaRetroceso = 5f;
    [SerializeField] private float tiempoInvulnerable = 0.5f;

    private LeoController controller;
    private int vidaActual;
    private Animator anim;
    private Rigidbody2D rb;
    private bool puedeRecibirDaño = true;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<LeoController>();
        if (controller == null)
        {
            Debug.LogError("LeoController no encontrado en el objeto. Asegurate de asignarlo.");
        }
    }

    public void RecibirDaño(int cantidad, Vector2 origenDaño)
    {
        if (!puedeRecibirDaño) return;

        puedeRecibirDaño = false;

        vidaActual -= cantidad;
        anim.SetTrigger("Daño");

        // Empuje hacia atrás
        Vector2 direccion = (transform.position - (Vector3)origenDaño).normalized;
        rb.velocity = new Vector2(direccion.x * fuerzaRetroceso, controller.FuerzaSalto * 0.5f);

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(ReactivarDaño());
        }
    }

    private IEnumerator ReactivarDaño()
    {
        yield return new WaitForSeconds(tiempoInvulnerable);
        puedeRecibirDaño = true;
        Debug.Log("Leo ahora puede volver a recibir daño");
    }

    public void Morir()
    {
        Debug.Log("Leo murió. Reiniciando...");

        if (puntoReaparicion != null)
        {
            transform.position = puntoReaparicion.position;
            vidaActual = vidaMaxima;
            puedeRecibirDaño = true; //REACTIVAR DAÑO

        }
        else
        {
            Debug.LogWarning("No hay punto de reaparición asignado.");
        }
    }

}
