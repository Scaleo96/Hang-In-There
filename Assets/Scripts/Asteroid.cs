using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject resourcePrefab;

    public void SpawnResource()
    {
        GameObject go = Instantiate(resourcePrefab, transform.position, Random.rotation, transform);
        go.transform.localScale = go.transform.localScale * 0.4f;
        go.transform.position += go.transform.forward * transform.localScale.x * 0.5f;
    }
}
