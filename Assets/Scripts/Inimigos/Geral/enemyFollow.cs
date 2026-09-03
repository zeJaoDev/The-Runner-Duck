using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Movimentação")]
    public float speed = 5f;

    private Transform jogador;

    private void Start()
    {
        ProcurarJogador();
    }

    private void Update()
    {
        if (jogador != null)
        {
            MoverEmDirecaoAoJogador();
        }
    }

    private void ProcurarJogador()
    {
        jogador = GameObject
            .FindGameObjectWithTag("Duck")
            .transform;
    }

    private void MoverEmDirecaoAoJogador()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            jogador.position,
            speed * Time.deltaTime
        );
    }
}