using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Manager settings")]
    [SerializeField]
    private float spawnRadius;
    [SerializeField]
    private float spawnHeight;
    [SerializeField]
    private float waveDelay;
    [SerializeField]
    private int spawnAmount;
    [SerializeField]
    private int enemySpawnIncrease;
    [SerializeField]
    private float bundleIncrease;
    [SerializeField]
    private GameObject[] spawner;
    [SerializeField]
    private float[] spawnChance;
    [SerializeField]
    private float[] spawnChanceIncrease;

    public GameObject player;
    public GameObject heart;

    [Header("Spawner settings")]
    [SerializeField]
    private int enemyAmount;
    [SerializeField]
    private int enemyAmountBundle;

    private float bundleBuildup = 0;
    private bool waveOn = false;

    private List<GameObject> spawners = new List<GameObject>();
    private float chancePool = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < spawnChance.Length; y++)
        {
            chancePool += spawnChance[y];
        }
    }

    public void StartWave()
    {
        waveOn = true;

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject prefab = null;
            float random = Random.Range(0, chancePool);
            float prev = 0;
            for (int y = 0; y < spawnChance.Length; y++)
            {
                if (random >= prev && prev < spawnChance[y])
                {
                    prefab = spawner[y];
                    break;
                }
                else
                {
                    prev = spawnChance[y];
                }
            }

            if (prefab != null)
            {
                Vector3 spawnPoint = (Vector3)Random.insideUnitCircle.normalized * spawnRadius;
                spawnPoint = new Vector3(spawnPoint.x, 0, spawnPoint.y) + transform.position;
                spawnPoint += new Vector3(0, Random.Range(-spawnHeight, spawnHeight), 0);
                GameObject go = Instantiate(prefab, spawnPoint, Quaternion.identity, transform);
                spawners.Add(go);
                go.GetComponent<EnemySpawner>().SpawnerSettings(enemyAmount, enemyAmountBundle, this);
            }
            else
            {
                Debug.LogError("No prefab");
            }
        }

        enemyAmount += enemySpawnIncrease;
        bundleBuildup += bundleIncrease;
        if (bundleBuildup >= 1f)
        {
            enemyAmountBundle++;
            bundleBuildup = 0;
        }

        for (int i = 0; i < spawnChance.Length; i++)
        {
            spawnChance[i] += spawnChanceIncrease[i];
        }

        waveOn = false;
    }

    public void RemoveSpawner(GameObject spawner)
    {
        if (spawners.Contains(spawner))
        {
            spawners.Remove(spawner);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
