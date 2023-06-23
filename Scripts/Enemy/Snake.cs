using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
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

    [SerializeField] private float huntSpeed = 4;

    [SerializeField] private float rayDistance;

    [SerializeField] private float attackRange;

    [SerializeField] private float poisonChance;
    [SerializeField] private float poisonTime;

    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float attackCooldown = 0;

    [SerializeField] private float hpDamage;
    [SerializeField] private float balanceDamage;

    [SerializeField] private bool isDash = true;
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
        if (PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            if (Mathf.Abs(player.position.x - transform.position.x) >= attackRange)
            {
                Pursuit(groundDetection, player, rayDistance, rb, huntSpeed);
            }
            else
            {
                attackCooldown = AttackPoison(attackPoint, playerLayer, attackCooldown, attackDelay, attackRange, hpDamage, balanceDamage, poisonChance, poisonTime);
            }
        }
        else
        {
            Sleep();
        }
    }

    public void Sleep()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y + high, boxCollider.bounds.size.z));
    }
}