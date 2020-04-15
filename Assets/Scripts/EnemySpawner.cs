using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int enemyAmount;
    [SerializeField]
    private int enemyAmountBundle;
    [SerializeField]
    private int enemySpawnDelay;
    [SerializeField]
    private GameObject enemyPrefab;

    public SpawnManager sm;

    private int enemiesSpawned;

    public void SpawnerSettings(int enemyAmount, int enemyAmountBundle, SpawnManager spawnManager)
    {
        sm = spawnManager;
        this.enemyAmount = enemyAmount;
        this.enemyAmountBundle = enemyAmountBundle;
        enemiesSpawned = 0;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemyAmount)
        {
            for (int i = 0; i < enemyAmountBundle; i++)
            {
                if (enemiesSpawned < enemyAmount)
                {

                    GameObject go = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                    if (Random.Range(0, 2) == 0)
                    {
                        go.GetComponent<Enemy>().ChoseTarget(sm.player.transform);
                    }
                    else
                    {
                        go.GetComponent<Enemy>().ChoseTarget(sm.heart.transform);
                    }

                    enemiesSpawned++;

                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(enemySpawnDelay);
        }

        DestorySpawner();
    }

    private void DestorySpawner()
    {
        sm.RemoveSpawner(gameObject);
        Destroy(gameObject);
    }
}
