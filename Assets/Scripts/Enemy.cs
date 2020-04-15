using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileShooter), typeof(Collider), typeof(Rigidbody))]
public class Enemy : Entity
{
    [Header("Enemy stats")]
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float maxSpeed;
    [SerializeField]
    protected float rotationSpeed;
    [SerializeField]
    protected float minShootDistance;

    [Header("Behaviour values")]
    [SerializeField]
    private float chaseWeight;
    [SerializeField]
    private float obstacleWeight;
    [SerializeField]
    private float obstacleRadius;
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private float avoidanceWeight;
    [SerializeField]
    private float avoidanceRadius;
    [SerializeField]
    private LayerMask avoidanceLayer;
    [SerializeField]
    private float targetRadius;
    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private float targetWeight;


    protected Transform target;
    protected Vector3 forceTotal;

    protected ProjectileShooter ps;
    protected Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ProjectileShooter>();
    }

    public void ChoseTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        BehaviourCombinear();
        ShootTarget();
    }

    private void ShootTarget()
    {
        if (ps != null)
        {
            Vector3 dir = target.position - transform.position;
            if (dir.magnitude <= minShootDistance)
            {
                ps.ShootProjectiles(dir.normalized, transform.position);
            }
        }
    }

    public override void EntityDeath()
    {
        Destroy(gameObject);
    }

    private void BehaviourCombinear()
    {
        forceTotal = Vector3.zero;
        forceTotal += ChaseBehaviour();
        forceTotal += AvoidanceBehaviour(avoidanceRadius, avoidanceLayer, avoidanceWeight);
        forceTotal += AvoidanceBehaviour(targetRadius, targetLayer, targetWeight);
        forceTotal += ObstacleBehaviour();
        forceTotal = forceTotal.normalized * speed;
        rb.AddForce(forceTotal);
        CapSpeed();
        RotateTowardsDirection();
    }

    private void CapSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void RotateTowardsDirection()
    {
        Quaternion lookRotation = Quaternion.LookRotation(rb.velocity.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private Vector3 ChaseBehaviour()
    {
        return (target.position - transform.position).normalized * chaseWeight;
    }

    private Vector3 ObstacleBehaviour()
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, obstacleRadius, obstacleLayer);

        Vector3 localForceTotal = Vector3.zero;
        
        foreach(Collider c in obstacles)
        {
            Vector3 dir = transform.position - c.transform.position;
            localForceTotal += dir.normalized;
        }

        if (localForceTotal != Vector3.zero)
        {
            localForceTotal = Vector3.Cross(localForceTotal, transform.forward).normalized;
            localForceTotal = localForceTotal * obstacleWeight;
        }

        return localForceTotal;
    }

    private Vector3 AvoidanceBehaviour(float radius, LayerMask layerMask, float weight)
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, radius, layerMask);

        Vector3 localForceTotal = Vector3.zero;

        foreach (Collider c in obstacles)
        {
            localForceTotal += transform.position - c.transform.position;
        }

        if (localForceTotal != Vector3.zero)
        {
            localForceTotal = localForceTotal.normalized * weight;
        }

        return localForceTotal;
    }
}
