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
        EncontrarBarraDeVida();
        InicializarVida();

        if (Time.timeScale > 0f)
        {
            ProcurarJogador();
        }
    }

    private void Update()
    {
        if (morreu)
        {
            return;
        }

        // Esconde a barra no menu, créditos
        // e na tela de Game Over.
        if (Time.timeScale <= 0f)
        {
            DefinirVisibilidadeDaBarra(false);
            return;
        }

        DefinirVisibilidadeDaBarra(true);

        if (player == null)
        {
            bool encontrouJogador =
                ProcurarJogador();

            if (!encontrouJogador)
            {
                return;
            }
        }

        MoverEmDirecaoAoJogador();
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

    private void MorrerInimigo()
    {
        if (morreu)
        {
            return;
        }

        morreu = true;

        // Registra uma kill antes de destruir o inimigo.
        if (ContadorKills.Instancia != null)
        {
            ContadorKills.Instancia.RegistrarKill();
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Duck"))
        {
            return;
        }

        PlayerMove jogadorEncontrado =
            collision.gameObject
                .GetComponentInParent<PlayerMove>();

        if (jogadorEncontrado != null)
        {
            jogadorEncontrado.Morrer();
        }
        else if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        enabled = false;
    }

    private void EncontrarBarraDeVida()
    {
        if (barraVida == null)
        {
            barraVida =
                GetComponentInChildren<BarraVida>(true);
        }
    }

    private void InicializarVida()
    {
        vidaAtual = vidaMaxima;

        if (barraVida == null)
        {
            Debug.LogWarning(
                "A BarraVida não foi encontrada no inimigo!",
                gameObject
            );

            return;
        }

        barraVida.SetVidaMaxima(vidaMaxima);
        barraVida.AlterarVida(vidaAtual);

        DefinirVisibilidadeDaBarra(
            Time.timeScale > 0f
        );
    }

    private bool ProcurarJogador()
    {
        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag("Duck");

        if (objetoJogador == null)
        {
            player = null;
            return false;
        }

        player = objetoJogador.transform;
        return true;
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

    private void DefinirVisibilidadeDaBarra(
        bool mostrar)
    {
        if (barraVida != null &&
            barraVida.gameObject.activeSelf != mostrar)
        {
            barraVida.gameObject.SetActive(mostrar);
        }
    }
}