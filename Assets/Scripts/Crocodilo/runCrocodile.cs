using UnityEngine;

public class runCrocodile : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public GameObject gameOverPanel;

    private void Start()
    {
        if (player == null)
        {
            GameObject duck = GameObject.FindGameObjectWithTag("Duck");

            if (duck != null)
            {
                player = duck.transform;
            }
            else
            {
                enabled = false;
            }
        }
    }

    private void Update()
    {
        if (player == null)
        {
            enabled = false;
            return;
        }

        float novaPosicaoX = Mathf.MoveTowards(
            transform.position.x,
            player.position.x,
            speed * Time.deltaTime
        );

        transform.position = new Vector2(
            novaPosicaoX,
            transform.position.y
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Duck")) return;

        PlayerMove movement =
            collision.gameObject.GetComponentInParent<PlayerMove>();

        if (movement != null)
        {
            movement.Morrer();
        }
        else if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // O crocodilo para de procurar o jogador.
        enabled = false;
    }
}