using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform destino;
    [SerializeField] private float velocidad = 2f;
    private Vector3 origen;
    private bool activada = false;

    private void Start()
    {
        origen = transform.position;
    }

    public void Activar(bool estado)
    {
        activada = estado;
    }

    private void Update()
    {
        Vector3 objetivo = activada ? destino.position : origen;
        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
    }
}
