using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;



public class PlayerMove : MonoBehaviour

{

    public InputAction playerControls;

    public float playerSpeed = 8f;

    Vector2 playerDirection;

    public Rigidbody2D playerPhysics;
    public InputAction playerJump;
    public float playerJumpHeight = 10f;

    [SerializeField] private GameObject shotWaterPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject BarraVida;


    private bool dead = false;

    public bool IsFrozen { get; private set; }

    [SerializeField] private float velocidadeDeEscape = 8f;
    [SerializeField] private float duracaoDoEscape = 0.3f;

    private bool saindoDaArmadilha = false;
    private float ultimaDirecaoHorizontal = 1f;

    private RigidbodyConstraints2D constraintsOriginais;

    public void Morrer()
    {
        if (dead) return;

        dead = true;
        gameOverPanel.SetActive(true);

        // Para movimento
        enabled = false;

        // Esconde o sprite do jogador
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        BarraVida[] barras = FindObjectsByType<BarraVida>(FindObjectsSortMode.None);
        foreach (BarraVida barra in barras) barra.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    private void Awake()
    {
        if (playerPhysics == null)
        {
            playerPhysics = GetComponent<Rigidbody2D>();
        }

        if (playerPhysics != null)
        {
            constraintsOriginais = playerPhysics.constraints;
        }
    }

    public void Freeze()
    {
        if (dead || IsFrozen) return;

        IsFrozen = true;

        if (playerPhysics != null)
        {
            playerPhysics.linearVelocity = Vector2.zero;
            playerPhysics.angularVelocity = 0f;
            playerPhysics.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        enabled = false;
    }

    public void Unfreeze()
    {
        if (dead || !IsFrozen) return;

        IsFrozen = false;

        if (playerPhysics != null)
        {
            playerPhysics.constraints = constraintsOriginais;
            playerPhysics.WakeUp();
        }

        enabled = true;
    }

    public void EscaparDaArmadilha(float direcaoDeSaida)
    {
        if (dead || !IsFrozen) return;

        ultimaDirecaoHorizontal =
            direcaoDeSaida < 0f ? -1f : 1f;

        IsFrozen = false;
        saindoDaArmadilha = true;

        if (playerPhysics != null)
        {
            playerPhysics.constraints = constraintsOriginais;
            playerPhysics.WakeUp();
        }

        enabled = true;
        StartCoroutine(SaidaAutomatica());
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

    void Start() { }

    private void Update()
    {
        if (IsFrozen ||
            saindoDaArmadilha ||
            playerPhysics == null)
        {
            return;
        }

        playerDirection = playerControls.ReadValue<Vector2>();

        if (Mathf.Abs(playerDirection.x) > 0.01f)
        {
            ultimaDirecaoHorizontal =
                Mathf.Sign(playerDirection.x);
        }

        playerPhysics.linearVelocity = new Vector2(
            playerDirection.x * playerSpeed,
            playerPhysics.linearVelocity.y
        );

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private IEnumerator SaidaAutomatica()
    {
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoDoEscape && !dead)
        {
            if (playerPhysics != null)
            {
                playerPhysics.linearVelocity = new Vector2(
                    ultimaDirecaoHorizontal * velocidadeDeEscape,
                    playerPhysics.linearVelocity.y
                );
            }

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        if (playerPhysics != null)
        {
            playerPhysics.linearVelocity = new Vector2(ultimaDirecaoHorizontal * velocidadeDeEscape, 0f);
        }

        saindoDaArmadilha = false;
    }

    public void DoJump(InputAction.CallbackContext context)
    {
        if (IsFrozen || playerPhysics == null) return;

        playerPhysics.linearVelocity = new Vector2(
            playerDirection.x * playerSpeed,
            playerJumpHeight
        );
    }

    private void FixedUpdate() { }

    private void Shoot()
    {
        if (shotWaterPrefab == null || firingPoint == null)
        {
            return;
        }

        Instantiate(shotWaterPrefab, firingPoint.position, firingPoint.rotation);
    }

}