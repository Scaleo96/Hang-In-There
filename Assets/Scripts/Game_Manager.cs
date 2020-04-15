using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private int resources = 0;

    [Header("Map Generation")]
    [SerializeField]
    private GameObject[] asteroidPrefab;
    [SerializeField]
    private int fieldRadius = 100;
    [SerializeField]
    private int asteroidCount = 500;
    [SerializeField]
    private float heightPotensiel = 50f;
    [SerializeField]
    private float minScale = 0.5f;
    [SerializeField]
    private float maxScale = 5f;
    [SerializeField]
    private float minimumDistanceFromCenter = 5;

    [Header("New Wave")]
    [SerializeField]
    private float resourceChance = 0.01f;

    private List<Asteroid> asteroids;

    [Header("Components")]
    [SerializeField]
    private UI_Manager ui;

    private void Awake()
    {
        asteroids = new List<Asteroid>();

        for (int i = 0; i < asteroidCount; i++)
        {
            int index = Random.Range(0, asteroidPrefab.Length);
            Transform t = Instantiate(asteroidPrefab[index], (Vector3)Random.insideUnitCircle * fieldRadius, Random.rotation).transform;
            t.position = new Vector3(t.position.x, transform.position.y + Random.Range(-heightPotensiel, heightPotensiel), t.position.y);
            if (Vector3.Distance(transform.position, t.position) <= minimumDistanceFromCenter)
            {
                t.position += (t.position - transform.position).normalized * minimumDistanceFromCenter;
            }
            t.localScale = t.localScale * Random.Range(minScale, maxScale);
            asteroids.Add(t.GetComponent<Asteroid>());
        }
    }

    public bool ConsumeResources(int amount)
    {
        if (resources >= amount)
        {
            resources -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddResources(int amount)
    {
        resources += amount;
        if (ui != null)
        {
            ui.UpdateResources(resources);
        }
    }

    public void NewWavePreparation()
    {
        foreach(Asteroid a in asteroids)
        {
            if (Random.value <= resourceChance)
            a.SpawnResource();
        }
    }
}
