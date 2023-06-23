using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Enemy
{
    [Header("Enemy parametres")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float scaleX;

    [Header("Enemy parametres")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform player;

    [Header("Vision parametres")]
    [SerializeField] private float range;
    [SerializeField] private float high;
    [SerializeField] private float colliderDistance;

    [Header("Moving parametres")]
    [SerializeField] private float runSpeed = 4;
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float rayDistance;

    [Header("Attack parametres")]
    [SerializeField] private float attackRange;
    [SerializeField] private float weakAttackDelay = 1;
    [SerializeField] private float strongAttackDelay = 5;
    [SerializeField] private Vector2 cooldowns = new Vector2(0, 0);
    [SerializeField] private float weakHpDamage;
    [SerializeField] private float weakBalanceDamage;
    [SerializeField] private float strongHpDamage;
    [SerializeField] private float strongBalanceDamage;

    public void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
    }

    void Update()
    {
        LifeCycle();
    }

    void LifeCycle()
    {
        if (!PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            Patrol(groundDetection, rayDistance, rb, patrolSpeed, scaleX);
        }
        else
        {
            if (Mathf.Abs(player.position.x - transform.position.x) >= attackRange)
            {
                cooldowns = new(0, 0);
                Pursuit(groundDetection, player, rayDistance, rb, runSpeed, scaleX);
            }
            else
            {
                cooldowns = AttackLightAndHeavy(attackPoint, playerLayer, player, cooldowns[0], cooldowns[1], weakAttackDelay, strongAttackDelay, attackRange, weakHpDamage, strongHpDamage, weakBalanceDamage, strongBalanceDamage, scaleX);
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
