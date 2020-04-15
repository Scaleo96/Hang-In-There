using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartStructure : Entity
{
    [SerializeField]
    private Player_Manager player;
    [SerializeField]
    private UI_Manager ui_Manager;

    public override void EntityDeath()
    {
    }

    public override void UseEntity(Game_Manager game_Manager = null)
    {
        ui_Manager.OpenHeartMenu();
    }
}
