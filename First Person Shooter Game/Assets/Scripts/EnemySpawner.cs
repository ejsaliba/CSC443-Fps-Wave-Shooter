using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private EnemyHealth[] enemyPrefabs; // MULTIPLE TYPES
    [SerializeField] private int prewarmCount = 5;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxActiveEnemies = 10;

    private ObjectPool<EnemyHealth>[] pools;

    private void Start()
    {
        pools = new ObjectPool<EnemyHealth>[enemyPrefabs.Length];

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            pools[i] = new ObjectPool<EnemyHealth>(enemyPrefabs[i], transform, prewarmCount);
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int totalActive = 0;
            foreach (var p in pools)
                totalActive += p.CountActive;

            if (totalActive < maxActiveEnemies && spawnPoints.Length > 0)
                SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, pools.Length);
        var pool = pools[index];

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        EnemyHealth enemy = pool.Get(point.position, point.rotation);

        enemy.OnDied += (e) => HandleEnemyDied(e, pool);
    }

    private void HandleEnemyDied(EnemyHealth enemy, ObjectPool<EnemyHealth> pool)
    {
        enemy.OnDied -= (e) => HandleEnemyDied(e, pool);
        pool.Return(enemy);
    }
}