using UnityEngine;

public class PlayerLife : MonoBehaviour
{
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

void Update()
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


