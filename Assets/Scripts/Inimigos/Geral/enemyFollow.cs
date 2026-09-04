using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float speed = 5f;

    private Transform jogador;

    private void Start()
    {
        ProcurarJogador();
    }

    private void Update()
    {
        // Não procura nem movimenta o inimigo
        // enquanto estiver no menu ou nos créditos.
        if (Time.timeScale <= 0f)
        {
            return;
        }

        // Tenta encontrar novamente quando o
        // jogador for ativado pelo botão Jogar.
        if (jogador == null)
        {
            bool encontrouJogador =
                ProcurarJogador();

            if (!encontrouJogador)
            {
                return;
            }
        }

        MoverEmDirecaoAoJogador();
    }

    private bool ProcurarJogador()
    {
        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag("Duck");

        if (objetoJogador == null)
        {
            jogador = null;
            return false;
        }

        jogador = objetoJogador.transform;
        return true;
    }

    private void MoverEmDirecaoAoJogador()
    {
        if (jogador == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            jogador.position,
            speed * Time.deltaTime
        );
    }
}