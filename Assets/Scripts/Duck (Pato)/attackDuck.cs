using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class attackCharacters : MonoBehaviour
{
    [Header("Movimentação")]
    [Range(1f, 10f)]
    [SerializeField] private float speed = 10f;

    [Header("Dano")]
    [SerializeField] private float dano = 10f;

    [Header("Tempo de vida")]
    [Range(1f, 10f)]
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D corpoRigido;
    private bool acertouInimigo;

    private void Awake()
    {
        corpoRigido = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MovimentarTiro();

        Destroy(
            gameObject,
            lifetime
        );
    }

    private void OnTriggerEnter2D(Collider2D outroColisor)
    {
        if (acertouInimigo)
        {
            return;
        }

        enemyGeral inimigo =
            outroColisor.GetComponentInParent<enemyGeral>();

        if (inimigo == null)
        {
            return;
        }

        acertouInimigo = true;

        // Aplica ao inimigo o dano configurado no Inspector.
        inimigo.ReceberDano(dano);

        // Destrói somente o tiro após acertar o inimigo.
        Destroy(gameObject);
    }

    private void MovimentarTiro()
    {
        corpoRigido.linearVelocity =
            (Vector2)transform.up * speed;
    }
}