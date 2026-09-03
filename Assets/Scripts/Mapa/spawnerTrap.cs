using System.Collections;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    [Header("Armadilha")]
    [SerializeField] private GameObject sandTrapPrefab;

    [Header("Área de criação")]
    [SerializeField] private float minX = -4f;
    [SerializeField] private float maxX = 4f;
    [SerializeField] private float spawnY = 6f;

    [Header("Intervalo de criação")]
    [SerializeField] private float atrasoInicial = 1f;
    [SerializeField] private float intervaloMinimo = 1.5f;
    [SerializeField] private float intervaloMaximo = 3f;

    [Header("Mira no jogador")]
    [Range(0f, 1f)]
    [SerializeField] private float chanceDeSpawnarNoJogador = 0.8f;

    [Header("Distância entre armadilhas")]
    [SerializeField] private float distanciaMinimaHorizontal = 1.5f;
    [SerializeField] private float distanciaMinimaVertical = 2.5f;
    [SerializeField] private int maximoDeTentativas = 50;

    private Transform jogador;

    private void Start()
    {
        ProcurarJogador();
        StartCoroutine(RotinaDeCriacaoDeArmadilhas());
    }

    private IEnumerator RotinaDeCriacaoDeArmadilhas()
    {
        yield return new WaitForSeconds(atrasoInicial);

        while (true)
        {
            CriarArmadilha();

            float menorIntervalo = Mathf.Min(
                intervaloMinimo,
                intervaloMaximo
            );

            float maiorIntervalo = Mathf.Max(
                intervaloMinimo,
                intervaloMaximo
            );

            float proximoIntervalo = Random.Range(
                menorIntervalo,
                maiorIntervalo
            );

            yield return new WaitForSeconds(proximoIntervalo);
        }
    }

    private void CriarArmadilha()
    {
        if (sandTrapPrefab == null)
        {
            return;
        }

        if (jogador == null)
        {
            ProcurarJogador();
        }

        if (!TentarGerarPosicao(out Vector2 posicao))
        {
            // Nenhuma posição segura foi encontrada.
            // Ignora somente esta tentativa de criação.
            return;
        }

        Instantiate(
            sandTrapPrefab,
            posicao,
            Quaternion.identity
        );
    }

    private void ProcurarJogador()
    {
        GameObject objetoJogador =
            GameObject.FindGameObjectWithTag("Duck");

        if (objetoJogador != null)
        {
            jogador = objetoJogador.transform;
        }
    }

    private bool TentarGerarPosicao(out Vector2 posicaoEncontrada)
    {
        bool tentarNoEixoDoJogador =
            jogador != null &&
            Random.value <= chanceDeSpawnarNoJogador;

        if (tentarNoEixoDoJogador)
        {
            float posicaoXDoJogador = Mathf.Clamp(
                jogador.position.x,
                minX,
                maxX
            );

            Vector2 posicaoDoJogador = new Vector2(
                posicaoXDoJogador,
                spawnY
            );

            // O mesmo X pode ser usado novamente,
            // desde que a armadilha anterior já tenha
            // descido o suficiente.
            if (PosicaoValida(posicaoDoJogador))
            {
                posicaoEncontrada = posicaoDoJogador;
                return true;
            }

            // Se estiver muito próximo de outra armadilha,
            // procura uma posição aleatória.
        }

        for (
            int tentativa = 0;
            tentativa < maximoDeTentativas;
            tentativa++
        )
        {
            Vector2 posicaoAleatoria = new Vector2(
                Random.Range(minX, maxX),
                spawnY
            );

            if (PosicaoValida(posicaoAleatoria))
            {
                posicaoEncontrada = posicaoAleatoria;
                return true;
            }
        }

        posicaoEncontrada = Vector2.zero;
        return false;
    }

    private bool PosicaoValida(Vector2 posicaoTestada)
    {
        sandTrap[] armadilhas = FindObjectsByType<sandTrap>(
            FindObjectsSortMode.None
        );

        foreach (sandTrap armadilha in armadilhas)
        {
            if (armadilha == null)
            {
                continue;
            }

            Vector2 posicaoDaArmadilha =
                armadilha.transform.position;

            float distanciaHorizontal = Mathf.Abs(
                posicaoTestada.x - posicaoDaArmadilha.x
            );

            float distanciaVertical = Mathf.Abs(
                posicaoTestada.y - posicaoDaArmadilha.y
            );

            bool estaPertoHorizontalmente =
                distanciaHorizontal < distanciaMinimaHorizontal;

            bool estaPertoVerticalmente =
                distanciaVertical < distanciaMinimaVertical;

            // Bloqueia a posição somente quando outra
            // armadilha estiver próxima nos dois eixos.
            if (estaPertoHorizontalmente &&
                estaPertoVerticalmente)
            {
                return false;
            }
        }

        return true;
    }
}