using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : DamagebleObject
{
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    private Vector2 HW;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float distance;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private Transform groundDetection;

    [SerializeField] private float stepDistance = 5;
    [SerializeField] private float stopDistance = 2;

    [SerializeField] private float runSpeed = 10;
    [SerializeField] private float stepSpeed = 5;

    [SerializeField] private float range;
    [SerializeField] private float high;
    [SerializeField] private float colliderDistance;

    [SerializeField] private Transform attackPoint;

    [SerializeField] private float weakHpDamage;
    [SerializeField] private float weakBalanceDamage;
    [SerializeField] private float strongHpDamage;
    [SerializeField] private float strongBalanceDamage;

    [SerializeField] private float strongAttackCooldown = 0;
    [SerializeField] private float attackCooldown = 0;

    [SerializeField] private float attackRange = 0.5f;

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
        LifeCicle();
    }

    public void LifeCicle()
    {
        if (PlayerInZone())
        {
            Hunting();
        }
        else
        {
            Sleep();
        }
    }

    public void Hunting()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer >= stepDistance)
        {
            Running();
        }
        else
        {
            if (distToPlayer >= stopDistance)
            {
                Steping();
            }
            else
            {
                Fighting();
            }
        }
    }

    public void Sleep()
    {

    }

    public void Steping()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
            HW = new Vector2(-stepSpeed, rb.velocity.y);
            rb.velocity = HW;
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
            HW = new Vector2(stepSpeed, rb.velocity.y);
            rb.velocity = HW;
        }
    }

    public void Running()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
            HW = new Vector2(-runSpeed, rb.velocity.y);
            rb.velocity = HW;
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
            HW = new Vector2(runSpeed, rb.velocity.y);
            rb.velocity = HW;
        }
    }

    public void Fighting()
    {
        if (attackCooldown <= 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer.Length != 0)
            {
                for (int i = 0; i < hitPlayer.Length; ++i)
                {
                    if(strongAttackCooldown <= 0)
                    {
                        hitPlayer[i].GetComponent<Player>().TakeDamage(strongHpDamage, strongBalanceDamage);
                        if (player.position.x < transform.position.x)
                        {
                            hitPlayer[i].GetComponent<Player>().KnockBack(true);
                        }
                        else
                        {
                            hitPlayer[i].GetComponent<Player>().KnockBack(false);
                        }
                        attackCooldown = 1;
                        strongAttackCooldown = 10;
                    }
                    else
                    {
                        hitPlayer[i].GetComponent<Player>().TakeDamage(weakHpDamage, weakBalanceDamage);
                        attackCooldown = 1;
                    }
                }
            }
        }
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (strongAttackCooldown > 0)
        {
            strongAttackCooldown -= Time.deltaTime;
        }
    }

    private bool PlayerInZone()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y + high, boxCollider.bounds.size.z),
            0,
            Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y + high, boxCollider.bounds.size.z));
    }
}
