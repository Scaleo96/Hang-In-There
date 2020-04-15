using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private LayerMask layer;

    private Rigidbody rb;

    public void InitializedProjectile(int damage, float speed, LayerMask layer)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.layer = layer;

        rb.velocity = transform.forward * speed;
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HealthController hc = null;
        if (layer == (layer | (1 << collision.gameObject.layer)))
        {
             hc = collision.transform.GetComponent<HealthController>();
        }

        if (hc != null)
        {
            hc.DamageEntity(damage);
        }

        Destroy(gameObject);
    }
}
