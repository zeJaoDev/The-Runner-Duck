using UnityEngine;

public class sandTrap : MonoBehaviour
{
    [Header("Movimentação vertical")]
    [SerializeField] private float velocidadeInicial = 4f;
    [SerializeField] private float aumentoPorSegundo = 0.05f;
    [SerializeField] private float velocidadeMaxima = 9f;
    [SerializeField] private float limiteY = -6f;

    [Header("Lentidão durante a captura")]
    [Range(0.1f, 1f)]
    [SerializeField] private float multiplicadorQuandoPreso = 0.35f;
    [SerializeField] private float tempoLentoAposEscapar = 0.75f;
    [SerializeField] private float duracaoDoRetorno = 1.5f;

    [Header("Captura")]
    [SerializeField] private Transform pontoDeCaptura;

    [Header("Escape")]
    [SerializeField] private int alternanciasParaEscapar = 4;
    [SerializeField] private float distanciaDoDebate = 0.15f;

    // Jogador capturado por esta armadilha.
    private PlayerMove jogadorPreso;

    // Controle do movimento de escape.
    private int quantidadeDeAlternancias;
    private KeyCode ultimaTecla = KeyCode.None;
    private float deslocamentoDoDebate;

    // Estado compartilhado por todas as armadilhas.
    private static bool jogadorEstaPreso;
    private static bool lentidaoFoiAtivada;
    private static float momentoEmQueEscapou;
    private static int cenaAtual = -1;

    private void Awake()
    {
        ReiniciarEstadoAoMudarDeCena();

        if (pontoDeCaptura == null)
        {
            pontoDeCaptura = transform;
        }
    }

    private void Update()
    {
        MovimentarArmadilha();
        DetectarDebate();

        if (transform.position.y <= limiteY)
        {
            SoltarJogador();
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (jogadorPreso == null)
        {
            return;
        }

        if (!jogadorPreso.IsFrozen)
        {
            jogadorPreso = null;

            ResetarEscape();
            RegistrarEscapeGlobal();

            return;
        }

        ManterJogadorNoPontoDeCaptura();
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

        ResetarEscape();
        jogadorPreso.Freeze();
        RegistrarCapturaGlobal();
    }

    private void OnDestroy()
    {
        SoltarJogador();
    }

    private void ReiniciarEstadoAoMudarDeCena()
    {
        int identificadorDaCena = gameObject.scene.handle;

        if (cenaAtual == identificadorDaCena)
        {
            return;
        }

        cenaAtual = identificadorDaCena;
        jogadorEstaPreso = false;
        lentidaoFoiAtivada = false;
        momentoEmQueEscapou = 0f;
    }

    private void MovimentarArmadilha()
    {
        float velocidadeBase =
            velocidadeInicial +
            Time.timeSinceLevelLoad * aumentoPorSegundo;

        velocidadeBase = Mathf.Min(
            velocidadeBase,
            velocidadeMaxima
        );

        float multiplicadorAtual =
            CalcularMultiplicadorDeVelocidade();

        float velocidadeAtual =
            velocidadeBase * multiplicadorAtual;

        transform.Translate(
            Vector2.down * velocidadeAtual * Time.deltaTime,
            Space.World
        );
    }

    private float CalcularMultiplicadorDeVelocidade()
    {
        // Antes da primeira captura, a velocidade permanece normal.
        if (!lentidaoFoiAtivada)
        {
            return 1f;
        }

        // Enquanto o jogador estiver preso, mantém a velocidade reduzida.
        if (jogadorEstaPreso)
        {
            return multiplicadorQuandoPreso;
        }

        float tempoDesdeOEscape =
            Time.time - momentoEmQueEscapou;

        // Mantém a lentidão por um curto período após o escape.
        if (tempoDesdeOEscape <= tempoLentoAposEscapar)
        {
            return multiplicadorQuandoPreso;
        }

        if (duracaoDoRetorno <= 0f)
        {
            lentidaoFoiAtivada = false;
            return 1f;
        }

        float progressoDoRetorno =
            (tempoDesdeOEscape - tempoLentoAposEscapar) /
            duracaoDoRetorno;

        if (progressoDoRetorno >= 1f)
        {
            lentidaoFoiAtivada = false;
            return 1f;
        }

        return Mathf.Lerp(
            multiplicadorQuandoPreso,
            1f,
            Mathf.Clamp01(progressoDoRetorno)
        );
    }

    private void DetectarDebate()
    {
        if (jogadorPreso == null)
        {
            return;
        }

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

        ContabilizarAlternancia(teclaAtual);

        if (quantidadeDeAlternancias >= alternanciasParaEscapar)
        {
            float direcaoDeSaida =
                teclaAtual == KeyCode.D ? 1f : -1f;

            SoltarJogador(
                true,
                direcaoDeSaida
            );
        }
    }

    private void ContabilizarAlternancia(KeyCode teclaAtual)
    {
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
            quantidadeDeAlternancias = 1;
        }

        ultimaTecla = teclaAtual;
    }

    private void ManterJogadorNoPontoDeCaptura()
    {
        Vector3 centro = pontoDeCaptura.position;

        centro.x += deslocamentoDoDebate;
        centro.z = jogadorPreso.transform.position.z;

        jogadorPreso.transform.position = centro;
    }

    private void RegistrarCapturaGlobal()
    {
        jogadorEstaPreso = true;
        lentidaoFoiAtivada = true;
    }

    private void RegistrarEscapeGlobal()
    {
        jogadorEstaPreso = false;
        momentoEmQueEscapou = Time.time;
    }

    private void ResetarEscape()
    {
        quantidadeDeAlternancias = 0;
        ultimaTecla = KeyCode.None;
        deslocamentoDoDebate = 0f;
    }

    private void SoltarJogador(
        bool sairAndando = false,
        float direcaoDeSaida = 1f)
    {
        ResetarEscape();

        if (jogadorPreso == null)
        {
            return;
        }

        PlayerMove jogador = jogadorPreso;
        jogadorPreso = null;

        RegistrarEscapeGlobal();

        jogador.transform.SetParent(null, true);

        if (jogador.playerPhysics != null)
        {
            jogador.playerPhysics.linearVelocity =
                Vector2.zero;
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
}