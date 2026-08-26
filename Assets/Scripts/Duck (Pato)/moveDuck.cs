using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

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