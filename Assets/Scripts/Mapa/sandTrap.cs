using UnityEngine;

public class sandTrap : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float velocidade = 4f;
    [SerializeField] private float limiteY = -6f;

    [Header("Captura")]
    [SerializeField] private Transform pontoDeCaptura;

    [Header("Escape")]
    [SerializeField] private int alternanciasParaEscapar = 4;
    [SerializeField] private float distanciaDoDebate = 0.15f;

    private PlayerMove jogadorPreso;

    private int quantidadeDeAlternancias;
    private KeyCode ultimaTecla = KeyCode.None;
    private float deslocamentoDoDebate;

    private void Awake()
    {
        // Se nenhum ponto for colocado no Inspector,
        // utiliza o centro da própria armadilha.
        if (pontoDeCaptura == null)
        {
            pontoDeCaptura = transform;
        }
    }

    private void Update()
    {
        // Faz a armadilha descer.
        transform.Translate(
            Vector2.down * velocidade * Time.deltaTime
        );

        // Verifica a sequência A-D-A-D.
        DetectarDebate();

        // Apaga a armadilha ao sair da tela.
        if (transform.position.y <= limiteY)
        {
            SoltarJogador();
            Destroy(gameObject);
            return;
        }
    }

    private void DetectarDebate()
    {
        if (jogadorPreso == null) return;

        KeyCode teclaAtual;

        if (Input.GetKeyDown(KeyCode.A))
        {
            teclaAtual = KeyCode.A;
            deslocamentoDoDebate = -distanciaDoDebate;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            teclaAtual = KeyCode.D;
            deslocamentoDoDebate = distanciaDoDebate;
        }
        else
        {
            return;
        }

        if (ultimaTecla == KeyCode.None)
        {
            quantidadeDeAlternancias = 1;
        }
        else if (teclaAtual != ultimaTecla)
        {
            quantidadeDeAlternancias++;
        }
        else
        {
            // Repetiu o mesmo lado: reinicia a sequência.
            quantidadeDeAlternancias = 1;
        }

        ultimaTecla = teclaAtual;

        if (quantidadeDeAlternancias >= alternanciasParaEscapar)
        {
            float direcaoDeSaida =
                teclaAtual == KeyCode.D ? 1f : -1f;

            SoltarJogador(true, direcaoDeSaida);
        }
    }

    private void LateUpdate()
    {
        if (jogadorPreso == null) return;

        // Para de segurar caso o jogador seja liberado externamente.
        if (!jogadorPreso.IsFrozen)
        {
            jogadorPreso = null;
            quantidadeDeAlternancias = 0;
            ultimaTecla = KeyCode.None;
            deslocamentoDoDebate = 0f;
            return;
        }

        // Mantém o jogador no centro da armadilha,
        // deslocando-o levemente ao pressionar A ou D.
        Vector3 centro = pontoDeCaptura.position;

        centro.x += deslocamentoDoDebate;
        centro.z = jogadorPreso.transform.position.z;

        jogadorPreso.transform.position = centro;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove jogador =
            collision.GetComponentInParent<PlayerMove>();

        if (jogador == null ||
            jogadorPreso != null ||
            jogador.IsFrozen)
        {
            return;
        }

        jogadorPreso = jogador;

        quantidadeDeAlternancias = 0;
        ultimaTecla = KeyCode.None;
        deslocamentoDoDebate = 0f;

        jogadorPreso.Freeze();
    }

    private void SoltarJogador(
        bool sairAndando = false,
        float direcaoDeSaida = 1f)
    {
        quantidadeDeAlternancias = 0;
        ultimaTecla = KeyCode.None;
        deslocamentoDoDebate = 0f;

        if (jogadorPreso == null) return;

        PlayerMove jogador = jogadorPreso;
        jogadorPreso = null;

        // Retira o jogador de dentro da hierarquia da armadilha.
        jogador.transform.SetParent(null, true);

        // Remove qualquer velocidade de queda.
        if (jogador.playerPhysics != null)
        {
            jogador.playerPhysics.linearVelocity = Vector2.zero;
        }

        if (sairAndando)
        {
            jogador.EscaparDaArmadilha(direcaoDeSaida);
        }
        else
        {
            jogador.Unfreeze();
        }
    }

    private void OnDestroy()
    {
        // Garante que o jogador não permaneça congelado.
        SoltarJogador();
    }
}