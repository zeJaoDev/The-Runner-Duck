using UnityEngine;

public class runCrocodile : MonoBehaviour
{
    [Header("Jogador")]
    public Transform player;

    [Header("Movimentação")]
    public float speed = 3f;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    private void Start()
    {
        ProcurarJogador();
    }

    private void Update()
    {
        if (player == null)
        {
            enabled = false;
            return;
        }

        MoverEmDirecaoAoJogador();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Duck"))
        {
            return;
        }

        PlayerMove jogador =
            collision.gameObject.GetComponentInParent<PlayerMove>();

        if (jogador != null)
        {
            jogador.Morrer();
        }
        else if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // O crocodilo para de procurar o jogador.
        enabled = false;
    }

    private void ProcurarJogador()
    {
        if (player != null)
        {
            return;
        }

        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag("Duck");

        if (objetoJogador != null)
        {
            player = objetoJogador.transform;
        }
        else
        {
            enabled = false;
        }
    }

    private void MoverEmDirecaoAoJogador()
    {
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
}