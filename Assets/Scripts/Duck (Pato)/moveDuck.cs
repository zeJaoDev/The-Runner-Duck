using System.Runtime.CompilerServices;

using UnityEngine;

using UnityEngine.InputSystem;

using System.Collections;

using System.Collections.Generic;

using Unity.VisualScripting;

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

    // Freeze state
    public bool IsFrozen { get; private set; }

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

    public void Freeze()
    {
        if (IsFrozen) return;
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
        if (!IsFrozen) return;
        IsFrozen = false;

        if (playerPhysics != null)
        {
            playerPhysics.constraints = RigidbodyConstraints2D.None;
        }

        enabled = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerJump.Enable();
        playerJump.performed += DoJump;
    }
    private void OnDisable()
    {
        playerControls.Disable();
        playerJump.Disable();
    }

    void Start() { }

    void Update()
    {
        if (IsFrozen) return;

        playerDirection = playerControls.ReadValue<Vector2>();
       
        playerPhysics.linearVelocity = new Vector2(playerDirection.x * playerSpeed, playerPhysics.linearVelocity.y);

        if (Input.GetMouseButtonDown(0)) Shoot();
    }

    public void DoJump(InputAction.CallbackContext context)
    {
        if (IsFrozen) return;
        playerPhysics.linearVelocity = new Vector2(playerDirection.x * playerSpeed, playerJumpHeight);
    }

    private void FixedUpdate() { }

    private void Shoot()
    {
        Instantiate(shotWaterPrefab, firingPoint.position, firingPoint.rotation);
    }

}