using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapLoading : MonoBehaviour
{

    public BoxCollider2D colisor;
    public Rigidbody2D rb;

   
    private float height;
    private float loadingSpeed = -3f;


    void Start()
    {
        
     colisor = GetComponent <BoxCollider2D>();
     rb = GetComponent <Rigidbody2D>();

     height = colisor.size.y;
     colisor.enabled = false;

     rb.linearVelocity = new Vector2(0, loadingSpeed);

    }

    void Update()
    {
       
     if (transform.position.y < -height)
        {

         Vector2 resetPosition = new Vector2(height * 0, 5f);
         transform.position = (Vector2)transform.position + resetPosition;

        }

    }
}
