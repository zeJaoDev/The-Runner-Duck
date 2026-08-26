using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runCrocodile : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public GameObject gameOverPanel;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Duck").transform;
    }
    void Update()
    {
        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime), transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Duck"))
        {
            gameOverPanel.SetActive(true);

            PlayerMove movement = collision.gameObject.GetComponent<PlayerMove>();

            if (movement != null)
            {
                movement.enabled = false;
            }
        }
    }
}