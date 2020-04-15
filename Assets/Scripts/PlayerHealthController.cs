using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : HealthController
{
    [SerializeField]
    private UI_Manager ui;
    [SerializeField]
    private int index;

    public override void DamageEntity(int damage)
    {
        base.DamageEntity(damage);
        uiHealth();
    }

    public override void HealEntity(int health)
    {
        base.HealEntity(health);
        uiHealth();
    }

    private void uiHealth()
    {
        if (ui != null)
        {
            ui.UpdateHealth(health, index);
        }
    }
}
