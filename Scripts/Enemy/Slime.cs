using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [Header("Enemy parametres")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform secGroundDetection;
    [SerializeField] private Transform wallDetection;
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
    [SerializeField] private float speed = 4;
    [SerializeField] private float rayDistance;
    [SerializeField] private float dashDelay = 1;
    [SerializeField] private float dashCooldown = 0;

    [Header("Attack parametres")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float attackCooldown = 0;
    [SerializeField] private float hpDamage;
    [SerializeField] private float balanceDamage;

    [Header("Animation")]
    [SerializeField] private Animator anim;

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
        if (PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            if (Mathf.Abs(player.position.x - transform.position.x) >= attackRange)
            {
                anim.SetBool("isAttack", false);
                dashCooldown = DashPursuit(groundDetection, secGroundDetection, player, dashCooldown, dashDelay, rayDistance, rb, speed, scaleX);
            }
            else
            {
                anim.SetBool("isAttack", true);
                attackCooldown = Attack(attackPoint, playerLayer, player, attackCooldown, attackDelay, attackRange, hpDamage, balanceDamage, scaleX);
            }
        }
        else
        {
            anim.SetBool("isAttack", false);
            dashCooldown = DashPatrol(groundDetection, secGroundDetection, wallDetection, dashCooldown, dashDelay, rayDistance, rb, speed, scaleX);
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
