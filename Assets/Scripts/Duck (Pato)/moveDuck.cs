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
  // [Range(0.1f, 1f)]
  // [SerializeField] private float fireRate = 0.5f;

  [SerializeField] private GameObject gameOverPanel;
  [SerializeField] private GameObject BarraVida;

private bool dead = false;
private bool stuck = false;
private int sequenceDash;

public void Morrer()
{
  if (dead) return;

  dead = true;

  // Mostra tela de Game Over
  gameOverPanel.SetActive(true);

  // Para movimento
  PlayerMove movement = GetComponent<PlayerMove>();

  if (movement != null)
{
  movement.enabled = false;
}
  // Esconde o sprite do jogador
  SpriteRenderer sprite = GetComponent<SpriteRenderer>();

  if (sprite != null)
{
            sprite.enabled = false;
}
  BarraVida[] barras = FindObjectsByType<BarraVida>(FindObjectsSortMode.None);

  foreach (BarraVida barra in barras)
{
  barra.gameObject.SetActive(false);
}
  Time.timeScale = 0f;
}

public void Stuck()
{
  stuck = true;
  sequenceDash = 0;
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
void Start()
    {

    }
    void Update()
    {
        playerDirection = playerControls.ReadValue<Vector2>();
        playerPhysics.linearVelocity = new Vector2(playerDirection.x * playerSpeed, playerPhysics.linearVelocity.y);

        if (Input.GetMouseButtonDown(0))
        {

            Shoot();

        }

        {
            if (stuck)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    sequenceDash++;

                    if (sequenceDash >= 5)
                    {
                        stuck = false;
                        sequenceDash = 0;
                    }
                }
                return;
            }

        }
    }

public void DoJump(InputAction.CallbackContext context)
    {
        playerPhysics.linearVelocity = new Vector2(playerDirection.x * playerSpeed, playerJumpHeight);
    }

private void FixedUpdate() 
    
    { 
    
    }
    
private void Shoot() 
    
{

        Instantiate(shotWaterPrefab, firingPoint.position, firingPoint.rotation);

}
    
}