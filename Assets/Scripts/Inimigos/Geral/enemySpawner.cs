using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Inimigo")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Área de criação")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;

    [Header("Intervalo de criação")]
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 4f;

    private void Start()
    {
        StartCoroutine(RotinaDeCriacaoDeInimigos());
    }

    private IEnumerator RotinaDeCriacaoDeInimigos()
    {
        while (true)
        {
            float posicaoXAleatoria = Random.Range(
                minX,
                maxX
            );

            Vector2 posicaoDeCriacao = new Vector2(
                posicaoXAleatoria,
                transform.position.y
            );

            Instantiate(
                enemyPrefab,
                posicaoDeCriacao,
                Quaternion.identity
            );

            float intervaloAleatorio = Random.Range(
                minSpawnTime,
                maxSpawnTime
            );

            yield return new WaitForSeconds(
                intervaloAleatorio
            );
        }
    }
}