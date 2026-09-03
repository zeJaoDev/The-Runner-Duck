using System.Collections;
using UnityEngine;
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

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;

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
        if (playerPhysics == null)
        {
            playerPhysics = GetComponent<Rigidbody2D>();
        }

        if (playerPhysics != null)
        {
            constraintsOriginais =
                playerPhysics.constraints;
        }
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
        // O tiro é verificado antes do bloqueio de movimento.
        // Assim, o jogador consegue atirar mesmo preso.
        VerificarTiro();

        if (IsFrozen ||
            saindoDaArmadilha ||
            playerPhysics == null)
        {
            return;
        }

        AtualizarMovimentacao();
    }

    public void Morrer()
    {
        if (dead)
        {
            return;
        }

        dead = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Aqui o script pode ser desativado,
        // pois o jogador realmente morreu.
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
            playerPhysics.linearVelocity = Vector2.zero;
            playerPhysics.angularVelocity = 0f;
            playerPhysics.constraints =
                RigidbodyConstraints2D.FreezeAll;
        }

        // Não desativa o PlayerMove.
        // O Update precisa continuar funcionando para atirar.
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
            playerPhysics == null)
        {
            return;
        }

        playerPhysics.linearVelocity = new Vector2(
            playerDirection.x * playerSpeed,
            playerJumpHeight
        );
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
        if (dead || saindoDaArmadilha)
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