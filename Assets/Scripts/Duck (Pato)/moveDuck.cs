using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Movimentação")]
    public InputAction playerControls;
    public float playerSpeed = 8f;
    public Rigidbody2D playerPhysics;

    [Header("Pulo")]
    public InputAction playerJump;
    public float playerJumpHeight = 10f;

    [Header("Tiro")]
    [SerializeField] private GameObject shotWaterPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Pontuação")]
    [SerializeField] private ContadorPontos contadorPontos;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private PontuacaoFinal pontuacaoFinal;

    [Header("Escape da armadilha")]
    [SerializeField] private float velocidadeDeEscape = 8f;
    [SerializeField] private float duracaoDoEscape = 0.3f;

    private Vector2 playerDirection;
    private RigidbodyConstraints2D constraintsOriginais;

    private bool dead;
    private bool saindoDaArmadilha;

    private float ultimaDirecaoHorizontal = 1f;

    public bool IsFrozen { get; private set; }

    private void Awake()
    {
        EncontrarComponentes();
        SalvarRestricoesOriginais();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerJump.Enable();

        playerJump.performed += DoJump;
    }

    private void OnDisable()
    {
        playerJump.performed -= DoJump;

        playerControls.Disable();
        playerJump.Disable();
    }

    private void Update()
    {
        // Permite atirar mesmo preso na armadilha.
        VerificarTiro();

        if (IsFrozen ||
            saindoDaArmadilha ||
            playerPhysics == null)
        {
            return;
        }

        AtualizarMovimentacao();
    }

    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        VerificarContatoComInimigo(
            collision.collider
        );
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        VerificarContatoComInimigo(other);
    }

    public void Morrer()
    {
        if (dead)
        {
            return;
        }

        dead = true;

        int pontosFinais =
            FinalizarContadorDePontos();

        MostrarGameOver(pontosFinais);

        // O jogador morreu, então o script
        // pode ser desativado completamente.
        enabled = false;

        EsconderJogador();
        EsconderBarrasDeVida();

        Time.timeScale = 0f;
    }

    public void Freeze()
    {
        if (dead || IsFrozen || saindoDaArmadilha)
        {
            return;
        }

        IsFrozen = true;

        if (playerPhysics != null)
        {
            playerPhysics.linearVelocity =
                Vector2.zero;

            playerPhysics.angularVelocity = 0f;

            playerPhysics.constraints =
                RigidbodyConstraints2D.FreezeAll;
        }

        // O PlayerMove permanece ativo para
        // permitir o tiro enquanto estiver preso.
    }

    public void Unfreeze()
    {
        if (dead || !IsFrozen)
        {
            return;
        }

        IsFrozen = false;

        if (playerPhysics != null)
        {
            playerPhysics.constraints =
                constraintsOriginais;

            playerPhysics.WakeUp();
        }
    }

    public void EscaparDaArmadilha(
        float direcaoDeSaida)
    {
        if (dead || !IsFrozen)
        {
            return;
        }

        ultimaDirecaoHorizontal =
            direcaoDeSaida < 0f ? -1f : 1f;

        IsFrozen = false;
        saindoDaArmadilha = true;

        if (playerPhysics != null)
        {
            playerPhysics.constraints =
                constraintsOriginais;

            playerPhysics.WakeUp();
        }

        StartCoroutine(SaidaAutomatica());
    }

    public void DoJump(
        InputAction.CallbackContext context)
    {
        if (dead ||
            IsFrozen ||
            saindoDaArmadilha ||
            playerPhysics == null ||
            Time.timeScale <= 0f)
        {
            return;
        }

        playerPhysics.linearVelocity = new Vector2(
            playerDirection.x * playerSpeed,
            playerJumpHeight
        );
    }

    private void EncontrarComponentes()
    {
        if (playerPhysics == null)
        {
            playerPhysics =
                GetComponent<Rigidbody2D>();
        }

        if (contadorPontos == null)
        {
            contadorPontos =
                FindFirstObjectByType<ContadorPontos>();
        }

        if (pontuacaoFinal == null &&
            gameOverPanel != null)
        {
            pontuacaoFinal =
                gameOverPanel
                    .GetComponentInChildren<PontuacaoFinal>(
                        true
                    );
        }
    }

    private void SalvarRestricoesOriginais()
    {
        if (playerPhysics != null)
        {
            constraintsOriginais =
                playerPhysics.constraints;
        }
    }

    private void AtualizarMovimentacao()
    {
        playerDirection =
            playerControls.ReadValue<Vector2>();

        if (Mathf.Abs(playerDirection.x) > 0.01f)
        {
            ultimaDirecaoHorizontal =
                Mathf.Sign(playerDirection.x);
        }

        playerPhysics.linearVelocity = new Vector2(
            playerDirection.x * playerSpeed,
            playerPhysics.linearVelocity.y
        );
    }

    private void VerificarTiro()
    {
        if (dead ||
            saindoDaArmadilha ||
            Time.timeScale <= 0f)
        {
            return;
        }

        // Impede o tiro quando o mouse estiver
        // sobre um botão ou elemento da interface.
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Atirar();
        }
    }

    private void Atirar()
    {
        if (shotWaterPrefab == null ||
            firingPoint == null)
        {
            return;
        }

        Instantiate(
            shotWaterPrefab,
            firingPoint.position,
            firingPoint.rotation
        );
    }

    private void VerificarContatoComInimigo(
        Collider2D objetoTocado)
    {
        if (dead || objetoTocado == null)
        {
            return;
        }

        GameObject objeto =
            objetoTocado.gameObject;

        GameObject objetoPrincipal =
            objetoTocado.transform.root.gameObject;

        bool tocouInimigo =
            objeto.CompareTag("Enemy") ||
            objeto.CompareTag("Croc") ||
            objetoPrincipal.CompareTag("Enemy") ||
            objetoPrincipal.CompareTag("Croc");

        if (tocouInimigo)
        {
            Morrer();
        }
    }

    private IEnumerator SaidaAutomatica()
    {
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoDoEscape &&
               !dead)
        {
            if (playerPhysics != null)
            {
                playerPhysics.linearVelocity =
                    new Vector2(
                        ultimaDirecaoHorizontal *
                        velocidadeDeEscape,
                        playerPhysics.linearVelocity.y
                    );
            }

            tempoDecorrido += Time.deltaTime;

            yield return null;
        }

        if (playerPhysics != null && !dead)
        {
            playerPhysics.linearVelocity =
                new Vector2(
                    ultimaDirecaoHorizontal *
                    velocidadeDeEscape,
                    0f
                );
        }

        saindoDaArmadilha = false;
    }

    private int FinalizarContadorDePontos()
    {
        if (contadorPontos == null)
        {
            contadorPontos =
                FindFirstObjectByType<ContadorPontos>();
        }

        if (contadorPontos != null)
        {
            return contadorPontos
                .FinalizarPontuacao();
        }

        Debug.LogWarning(
            "O ContadorPontos não foi encontrado!"
        );

        return 0;
    }

    private void MostrarGameOver(int pontosFinais)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "O GameOverPanel não foi configurado!"
            );
        }

        if (pontuacaoFinal != null)
        {
            pontuacaoFinal.MostrarPontuacao(
                pontosFinais
            );
        }
        else
        {
            Debug.LogWarning(
                "O componente PontuacaoFinal não foi encontrado!"
            );
        }
    }

    private void EsconderJogador()
    {
        SpriteRenderer renderizador =
            GetComponent<SpriteRenderer>();

        if (renderizador != null)
        {
            renderizador.enabled = false;
        }
    }

    private void EsconderBarrasDeVida()
    {
        BarraVida[] barras =
            FindObjectsByType<BarraVida>(
                FindObjectsSortMode.None
            );

        foreach (BarraVida barra in barras)
        {
            barra.gameObject.SetActive(false);
        }
    }
}