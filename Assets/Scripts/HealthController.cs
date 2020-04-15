using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Entity))]
public class HealthController : MonoBehaviour
{
    [SerializeField]
    protected int health;

    private Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();
    }

    public virtual void DamageEntity(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            entity.EntityDeath();
        }
    }

    public virtual void HealEntity(int health)
    {
        this.health += health;
    }
}
