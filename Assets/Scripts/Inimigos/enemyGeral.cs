using UnityEngine;

public class enemyGeral : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int vida;
    [SerializeField] private BarraVida barraVida;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Movimentação")]
    public Transform player;
    public float horizontalSpeed = 3f;

    private void Start()
    {
        if (player == null)
        {
            GameObject duck =
                GameObject.FindGameObjectWithTag("Duck");

            if (duck != null)
            {
                player = duck.transform;
            }
            else
            {
                enabled = false;
            }
        }

        if (barraVida != null)
        {
            barraVida.VidaMax = vida;
            barraVida.Vida = vida;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            enabled = false;
            return;
        }

        if (vida > 0)
        {
            Mover();
        }
    }

    public void ReceberDano()
    {
        if (vida <= 0) return;

        vida--;

        if (barraVida != null)
        {
            barraVida.Vida = vida;
        }

        if (vida <= 0)
        {
            Destroy(gameObject);

            // Quando adicionar animação:
            // animator.SetBool("eliminado", true);
        }
        else
        {
            // Quando adicionar animação:
            // animator.SetTrigger("recebendoDano");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Duck"))
        {
            PlayerMove duck =
                collision.gameObject.GetComponentInParent<PlayerMove>();

            if (duck != null)
            {
                // O próprio Morrer() mostra o painel e pausa o jogo.
                duck.Morrer();
            }
            else if (gameOverPanel != null)
            {
                // Segurança caso o PlayerMove não seja encontrado.
                gameOverPanel.SetActive(true);
            }

            // O inimigo para de perseguir o jogador.
            enabled = false;
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void Mover()
    {
        if (player == null) return;

        float novaPosicaoX = Mathf.MoveTowards(
            transform.position.x,
            player.position.x,
            horizontalSpeed * Time.deltaTime
        );

        transform.position = new Vector2(
            novaPosicaoX,
            transform.position.y
        );
    }
}