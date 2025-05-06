
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform player;
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private float spawnRadius = 50f;
    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());

    }
    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            if (player == null)
            {

                yield break;
            }
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector2 spawnPosition = (Vector2)player.position + randomOffset;
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }

}
