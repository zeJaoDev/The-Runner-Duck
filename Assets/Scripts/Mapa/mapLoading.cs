using UnityEngine;

public class mapLoading : MonoBehaviour
{
    [Header("Componentes")]
    public BoxCollider2D colisor;
    public Rigidbody2D rb;

    [Header("Movimentação")]
    private float altura;
    private float velocidadeDeDescida = -3f;

    private void Start()
    {
        colisor = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        altura = colisor.size.y;
        colisor.enabled = false;

        rb.linearVelocity = new Vector2(
            0f,
            velocidadeDeDescida
        );
    }

    private void Update()
    {
        if (transform.position.y < -altura)
        {
            ReposicionarMapa();
        }
    }

    private void ReposicionarMapa()
    {
        Vector2 deslocamentoDeReinicio = new Vector2(
            0f,
            5f
        );

        transform.position =
            (Vector2)transform.position +
            deslocamentoDeReinicio;
    }
}