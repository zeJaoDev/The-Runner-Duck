using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    // Área horizontal onde os inimigos podem surgir
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;

    // Tempo aleatório entre um spawn e outro
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 4f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Escolhe posição aleatória
            float randomX = Random.Range(minX, maxX);

            Vector2 spawnPosition = new Vector2(
                randomX,
                transform.position.y
            );

            // Cria o inimigo
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Espera um tempo aleatório
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomTime);
        }
    }
}