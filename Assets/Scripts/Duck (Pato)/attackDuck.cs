using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        enemyGeral enemy = other.GetComponent<enemyGeral>();

        if (enemy != null)
        {
            enemy.ReceberDano();
            Destroy(gameObject);
        }
    }

}
