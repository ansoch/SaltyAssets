using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private Transform player;

    [SerializeField] private float range;
    [SerializeField] private float high;
    [SerializeField] private float colliderDistance;

    [SerializeField] private float runSpeed = 4;
    [SerializeField] private float patrolSpeed = 2;

    [SerializeField] private float rayDistance;

    [SerializeField] private float attackRange;

    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float attackCooldown = 0;

    [SerializeField] private float hpDamage;
    [SerializeField] private float balanceDamage;

    public void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        LifeCycle();
    }

    void LifeCycle()
    {
        if (!PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            Patrol(groundDetection, rayDistance, rb, patrolSpeed);
        }
        else
        {
            if (Mathf.Abs(player.position.x - transform.position.x) >= attackRange)
            {
                attackCooldown = 0;
                Pursuit(groundDetection, player, rayDistance, rb, runSpeed);
            }
            else
            {
                attackCooldown = Attack(attackPoint, playerLayer, attackCooldown, attackDelay, attackRange, hpDamage, balanceDamage);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y + high, boxCollider.bounds.size.z));
    }
}
