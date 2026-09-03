using UnityEngine;

public class enemyGeral : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 30f;
    [SerializeField] private float danoDoTiro = 10f;
    [SerializeField] private BarraVida barraVida;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Movimentação")]
    [SerializeField] private Transform player;
    [SerializeField] private float horizontalSpeed = 3f;

    private float vidaAtual;
    private bool morreu;

    private void Start()
    {
        ProcurarBarraDeVida();
        InicializarVida();
        ProcurarJogador();
    }

    private void Update()
    {
        if (morreu)
        {
            return;
        }

        if (player == null)
        {
            enabled = false;
            return;
        }

        MoverEmDirecaoAoJogador();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Duck"))
        {
            return;
        }

        PlayerMove jogador =
            collision.gameObject.GetComponentInParent<PlayerMove>();

        if (jogador != null)
        {
            jogador.Morrer();
        }
        else if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        enabled = false;
    }

    public void ReceberDano()
    {
        ReceberDano(danoDoTiro);
    }

    public void ReceberDano(float quantidadeDeDano)
    {
        if (morreu || vidaAtual <= 0f)
        {
            return;
        }

        vidaAtual -= quantidadeDeDano;

        vidaAtual = Mathf.Clamp(
            vidaAtual,
            0f,
            vidaMaxima
        );

        if (barraVida != null)
        {
            barraVida.AlterarVida(vidaAtual);
        }

        if (vidaAtual <= 0f)
        {
            MorrerInimigo();
        }
    }

    private void ProcurarBarraDeVida()
    {
        if (barraVida != null)
        {
            return;
        }

        barraVida = GetComponentInChildren<BarraVida>(true);
    }

    private void InicializarVida()
    {
        vidaAtual = vidaMaxima;

        if (barraVida != null)
        {
            barraVida.gameObject.SetActive(true);
            barraVida.SetVidaMaxima(vidaMaxima);
            barraVida.AlterarVida(vidaAtual);
        }
        else
        {
            Debug.LogWarning(
                "A BarraVida não foi encontrada no inimigo!",
                gameObject
            );
        }
    }

    private void ProcurarJogador()
    {
        if (player != null)
        {
            return;
        }

        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag("Duck");

        if (objetoJogador != null)
        {
            player = objetoJogador.transform;
        }
        else
        {
            Debug.LogWarning(
                "Nenhum jogador com a tag Duck foi encontrado!",
                gameObject
            );
        }
    }

    private void MoverEmDirecaoAoJogador()
    {
        if (player == null)
        {
            return;
        }

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

    private void MorrerInimigo()
    {
        if (morreu)
        {
            return;
        }

        morreu = true;
        enabled = false;

        Destroy(gameObject);
    }
}