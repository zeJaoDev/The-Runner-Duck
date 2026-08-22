using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class attackCharacters : MonoBehaviour
{

    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        
     rb = GetComponent<Rigidbody2D>();
     Destroy(gameObject, lifetime);

   
    }

    private void FixedUpdate()
    {
        
     rb.linearVelocity = transform.up * speed;

    }

}
