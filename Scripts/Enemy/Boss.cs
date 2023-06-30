using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
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
    [SerializeField] private float stateSpeed = 2;
    [SerializeField] private float rayDistance;

    [Header("Attack parametres")]
    [SerializeField] private float attackRange;
    [SerializeField] private float weakHpDamage;
    [SerializeField] private float weakBalanceDamage;
    [SerializeField] private float aoeHpDamage;
    [SerializeField] private float aoeBalanceDamage;
    [SerializeField] private float strongHpDamage;
    [SerializeField] private float strongBalanceDamage;
    [SerializeField] private float aoeCoeff = 4;
    [SerializeField] private GameObject bullet;

    [Header("Cooldowns parametres")]
    [SerializeField] private float cooldown = 0;
    [SerializeField] private float callDelay = 3;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float aoeAttackDelay = 2;
    [SerializeField] private float hardAttackDelay = 3;
    [SerializeField] private float stateDelay = 4;

    [Header("Cooldowns parametres")]
    [SerializeField] private int attackType = 1;
    [SerializeField] private bool isDoing = false;
    [SerializeField] private bool isWaiting = false;


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

    public void LifeCycle()
    {
        if (PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            if(!isDoing)
            {
                isDoing = true;
                switch (attackType)
                {
                    case 1:
                        cooldown = callDelay;
                        break;
                    case 2:
                        cooldown = attackDelay;
                        break;
                    case 3:
                        cooldown = aoeAttackDelay;
                        break;
                    case 4:
                        cooldown = hardAttackDelay;
                        break;
                }
            }
            else
            {
                DoAState();
            }
        }
    }

    private void DoAState()
    {
        switch (attackType)
        {
            case 1:
                if(!isWaiting)
                {
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Shot(bullet, attackPoint);
                        isWaiting = true;
                        cooldown = stateDelay;
                    }
                }
                else
                {
                    Pursuit(groundDetection, player, rayDistance, rb, stateSpeed, scaleX);
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 2;
                    }
                }
                break;

            case 2:
                if (!isWaiting)
                {
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Punch(attackPoint, playerLayer, attackRange, weakHpDamage, weakBalanceDamage);
                        isWaiting = true;
                        cooldown = stateDelay;
                    }
                }
                else
                {
                    Pursuit(groundDetection, player, rayDistance, rb, stateSpeed, scaleX);
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 3;
                    }
                }
                break;

            case 3:
                if (!isWaiting)
                {
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Punch(attackPoint, playerLayer, attackRange * aoeCoeff, aoeHpDamage, aoeBalanceDamage);
                        isWaiting = true;
                        cooldown = stateDelay;
                    }
                }
                else
                {
                    Pursuit(groundDetection, player, rayDistance, rb, stateSpeed, scaleX);
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 4;
                    }
                }
                break;

            case 4:
                if (!isWaiting)
                {
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Punch(attackPoint, playerLayer, attackRange, strongHpDamage, strongBalanceDamage);
                        isWaiting = true;
                        cooldown = stateDelay;
                    }
                }
                else
                {
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 1;
                    }
                }
                break;
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
