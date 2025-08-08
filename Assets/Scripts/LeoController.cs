using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LeoController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float fuerzaSalto = 7f;
    [SerializeField] private Transform verificadorSuelo;
    [SerializeField] private float radioSuelo = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Wall Slide & Jump")]
    [SerializeField] private Transform verificadorPared;
    [SerializeField] private float distanciaPared = 0.4f;
    [SerializeField] private float velocidadDeslizamiento = 0.5f;

    [Header("Dash")]
    [SerializeField] private float fuerzaDash = 12f;
    [SerializeField] private float duracionDash = 0.2f;
    [SerializeField] private float tiempoEsperaDash = 1f;

    [Header("Agacharse con restricción de techo")]
    [SerializeField] private Transform verificadorTecho;
    [SerializeField] private float radioTecho = 0.2f;

    public float FuerzaSalto => fuerzaSalto;

    private Rigidbody2D rb;
    private Animator anim;

    private float inputHorizontal;
    private bool estaEnSuelo;
    private bool estaEnPared;
    private bool deslizandoseEnPared;
    private bool estaAgachado;
    private bool puedeMoverse = true;
    private bool estaDasheando = false;
    private float tiempoProximoDash;
    private int ultimaDireccionSaltoPared = 0; // -1 = izquierda, 1 = derecha, 0 = ninguno
    private float cooldownSaltoPared = 0.2f;
    private float tiempoUltimoSaltoPared = -1f;
    private bool tocandoParedDerecha;
    private bool tocandoParedIzquierda;
    private bool hayTechoEncima;

    private bool MirandoDerecha => transform.localScale.x > 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        LeerInput();
        ManejarAnimaciones();
        VerificarPared();

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= tiempoProximoDash && !estaDasheando)
        {
            StartCoroutine(EjecutarDash());
        }
    }

    private void FixedUpdate()
    {
        VerificarSuelo();

        if (estaEnSuelo)
        {
            ultimaDireccionSaltoPared = 0;
        }

        Mover();

        MecanicaWallSlide();

        if (!estaEnSuelo && estaEnPared && rb.velocity.y < 0)
        {
            deslizandoseEnPared = true;
            rb.velocity = new Vector2(rb.velocity.x, -velocidadDeslizamiento);
        }
        else
        {
            deslizandoseEnPared = false;
        }
        VerificarTecho();
    }

    private void LeerInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        bool quiereAgacharse = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        estaAgachado = quiereAgacharse || hayTechoEncima;

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (estaEnSuelo && !estaAgachado)
            {
                rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            }
            else if (deslizandoseEnPared)
            {
                int direccionActual = tocandoParedDerecha ? 1 : tocandoParedIzquierda ? -1 : 0;

                if (direccionActual != 0 &&
                    (Time.time - tiempoUltimoSaltoPared > cooldownSaltoPared || direccionActual != ultimaDireccionSaltoPared))
                {
                    float direccionSalto = direccionActual == 1 ? -1 : 1;
                    rb.velocity = new Vector2(direccionSalto * fuerzaSalto, fuerzaSalto);
                    VoltearPersonaje(direccionSalto);

                    ultimaDireccionSaltoPared = direccionActual;
                    tiempoUltimoSaltoPared = Time.time;
                }
            }
        }
    }

    private void VerificarTecho()
    {
        hayTechoEncima = Physics2D.OverlapCircle(verificadorTecho.position, radioTecho, capaSuelo);
    }


    private void Mover()
    {
        if (!puedeMoverse) return;

        float velocidadFinal = estaAgachado ? velocidadMovimiento * 0.5f : velocidadMovimiento;
        rb.velocity = new Vector2(inputHorizontal * velocidadFinal, rb.velocity.y);

        if (inputHorizontal != 0)
        {
            VoltearPersonaje(inputHorizontal);
        }
    }

    private void VoltearPersonaje(float direccion)
    {
        transform.localScale = new Vector3(Mathf.Sign(direccion), 1f, 1f);
    }

    private void VerificarSuelo()
    {
        estaEnSuelo = Physics2D.OverlapCircle(verificadorSuelo.position, radioSuelo, capaSuelo);
    }

    private void VerificarPared()
    {
        tocandoParedDerecha = Physics2D.Raycast(verificadorPared.position, Vector2.right, distanciaPared, capaSuelo);
        tocandoParedIzquierda = Physics2D.Raycast(verificadorPared.position, Vector2.left, distanciaPared, capaSuelo);
        estaEnPared = tocandoParedDerecha || tocandoParedIzquierda;
    }

    private void MecanicaWallSlide()
    {
        deslizandoseEnPared = false;

        // Solo activa el deslizamiento si toca la pared y no está en suelo
        if (estaEnPared && !estaEnSuelo)
        {
            if (rb.velocity.y <= 0)
            {
                deslizandoseEnPared = true;
                rb.velocity = new Vector2(rb.velocity.x, -velocidadDeslizamiento);
            }
        }
        if (!estaEnSuelo && estaEnPared && rb.velocity.y < 0)
        {
            
            deslizandoseEnPared = true;
            rb.velocity = new Vector2(rb.velocity.x, -velocidadDeslizamiento);
            Debug.Log("Wall Slide activado");
        }
    }

    private IEnumerator EjecutarDash()
    {
        estaDasheando = true;
        puedeMoverse = false;
        tiempoProximoDash = Time.time + tiempoEsperaDash;

        float direccion = transform.localScale.x;
        rb.velocity = new Vector2(direccion * fuerzaDash, 0f);

        yield return new WaitForSeconds(duracionDash);

        estaDasheando = false;
        puedeMoverse = true;
    }

    private void ManejarAnimaciones()
    {
        anim.SetFloat("Velocidad", Mathf.Abs(inputHorizontal));
        anim.SetBool("EnSuelo", estaEnSuelo);
        anim.SetBool("Agachado", estaAgachado);
        anim.SetBool("DeslizandoEnPared", deslizandoseEnPared);
    }

    private void OnDrawGizmosSelected()
    {
        if (verificadorSuelo != null)
            Gizmos.DrawWireSphere(verificadorSuelo.position, radioSuelo);

        if (verificadorPared != null)
        {
            Vector2 direccion = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(verificadorPared.position, direccion * distanciaPared);
        }
        if (verificadorTecho != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(verificadorTecho.position, radioTecho);
        }
    }
}

