using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : Entity
{
    [SerializeField]
    private int minResourceAmount;
    [SerializeField]
    private int maxResourceAmount;

    public override void UseEntity(Game_Manager game_Manager = null)
    {
        game_Manager.AddResources(Random.Range(minResourceAmount, maxResourceAmount + 1));
        Destroy(gameObject);
    }
}
