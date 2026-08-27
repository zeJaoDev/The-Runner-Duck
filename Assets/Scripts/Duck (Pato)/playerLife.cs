using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject BarraVida;

    private bool dead = false;

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

        Time.timeScale = 0f;
    }

        private void OnCollisionEnter2D(Collision2D collision)
{

        if (collision.gameObject.CompareTag("Enemy"))
{
        Destroy(gameObject);
}
        
}
     
}
