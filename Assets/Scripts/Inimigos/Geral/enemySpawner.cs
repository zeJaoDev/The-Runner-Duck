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

    private Coroutine rotinaDeCriacao;

    private void OnEnable()
    {
        IniciarRotinaDeCriacao();
    }

    private void OnDisable()
    {
        PararRotinaDeCriacao();
    }

    private void IniciarRotinaDeCriacao()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning(
                "O prefab do inimigo não foi configurado!",
                gameObject
            );

            return;
        }

        if (rotinaDeCriacao == null)
        {
            rotinaDeCriacao = StartCoroutine(
                RotinaDeCriacaoDeInimigos()
            );
        }
    }

    private void PararRotinaDeCriacao()
    {
        if (rotinaDeCriacao == null)
        {
            return;
        }

        StopCoroutine(rotinaDeCriacao);
        rotinaDeCriacao = null;
    }

    private IEnumerator RotinaDeCriacaoDeInimigos()
    {
        while (true)
        {
            // Espera o jogador apertar Jogar.
            // No menu o Time.timeScale está em zero.
            yield return new WaitUntil(
                () => Time.timeScale > 0f
            );

            CriarInimigo();

            float intervaloAleatorio =
                Random.Range(
                    minSpawnTime,
                    maxSpawnTime
                );

            yield return new WaitForSeconds(
                intervaloAleatorio
            );
        }
    }

    private void CriarInimigo()
    {
        float posicaoXAleatoria =
            Random.Range(minX, maxX);

        Vector2 posicaoDeCriacao =
            new Vector2(
                posicaoXAleatoria,
                transform.position.y
            );

        // O inimigo se torna filho do EnemySpawner.
        // Como o spawner está no ConteudoDoJogo,
        // o inimigo e sua barra somem no menu.
        Instantiate(
            enemyPrefab,
            posicaoDeCriacao,
            Quaternion.identity,
            transform
        );
    }
}