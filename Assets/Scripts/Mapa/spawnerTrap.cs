using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject sandTrapPrefab;

    [SerializeField]
    private float minX = -4f;

    [SerializeField]
    private float maxX = 4f;

    [SerializeField]
    private float spawnY = 6f;

    [SerializeField]
    private float intervaloSpawn = 2f;

    [SerializeField]
    private float distanciaMinima = 2.5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnTrap), 1f, intervaloSpawn);
    }

    private void SpawnTrap()
    {
        float posX = GerarPosicaoSegura();

        Instantiate(
            sandTrapPrefab,
            new Vector2(posX, spawnY),
            Quaternion.identity
        );
    }

    private float GerarPosicaoSegura()
    {
        float novaPosicaoX;
        int tentativas = 0;

        do
        {
            novaPosicaoX = Random.Range(minX, maxX);
            tentativas++;
        }
        while (!PosicaoValida(novaPosicaoX) && tentativas < 20);

        return novaPosicaoX;
    }

    private bool PosicaoValida(float posX)
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");

        foreach (GameObject trap in traps)
        {
            float distancia =
                Mathf.Abs(trap.transform.position.x - posX);

            if (distancia < distanciaMinima)
                return false;
        }

        return true;
    }
}