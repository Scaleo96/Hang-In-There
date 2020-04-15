using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int projectileAmount;
    [SerializeField]
    protected float fireSpeed;
    [SerializeField]
    protected float projectileSpeed;
    [SerializeField]
    protected float spreadAmount;

    [Header("Misc")]
    [SerializeField]
    protected LayerMask enemyLayers;
    [SerializeField]
    private int[] correctionLayers;
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected Vector3 offset;

    private int[] layersmasks;
    private int layers;
    protected float timer = 0;

    private void Start()
    {
        layersmasks = new int[correctionLayers.Length];
        layers = 0;

        for (int i = 0; i < correctionLayers.Length; i++)
        {
            layersmasks[i] = 0;
            layersmasks[i] = 1 << correctionLayers[i];
            layers = layers | layersmasks[i];
        }
    }

    public void ShootProjectiles(Vector3 dir, Vector3 origin)
    {
        if (timer <= 0)
        {
            Vector3 projectileOrigin = transform.position + new Vector3(transform.forward.x * offset.x, transform.forward.y * offset.y, transform.forward.z * offset.x);

            RaycastHit hit;
            Physics.Raycast(origin, dir, out hit, 100f, layers);
            Vector3 newDir;
            if (hit.collider == null)
            {
                newDir = (origin + dir * 100f) - projectileOrigin;
            }
            else
            {
                newDir = hit.point - projectileOrigin;
            }
            newDir.Normalize();
            Quaternion shootDir = Quaternion.LookRotation(newDir);

            for (int i = 0; i < projectileAmount; i++)
            {
                Quaternion shootDirOffset = shootDir * Quaternion.Euler(Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount), Random.Range(-spreadAmount, spreadAmount));
                GameObject go = Instantiate(projectile, projectileOrigin, shootDirOffset);
                go.GetComponent<Projectile>().InitializedProjectile(damage, projectileSpeed, enemyLayers);
            }

            timer = fireSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
}
