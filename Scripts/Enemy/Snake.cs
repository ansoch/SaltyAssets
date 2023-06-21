using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    private Vector2 HW;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float runSpeed = 4;
    [SerializeField] private float patrolSpeed = 2;

    [SerializeField] private float distance;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private Transform groundDetection;

    [SerializeField] private bool isChose = false;
    [SerializeField] private float patrolTime;
    [SerializeField] private float standingTime;
    [SerializeField] private bool isStanding;

    [SerializeField] private float stepDistance = 3;

    [SerializeField] private float range;
    [SerializeField] private float high;
    [SerializeField] private float colliderDistance;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float hpDamage;
    [SerializeField] private float balanceDamage;
    [SerializeField] private float poison;
    [SerializeField] private float poisonChanse;
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
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (PlayerInSight())
        {
            if (distToPlayer >= stepDistance)
            {
                Running();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            Patrol();
        }
    }

    public void Patrol()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider == false)
        {
            movingRight = !movingRight;
        }

        if (movingRight)
        {
            transform.localScale = new Vector2(-1, 1);
            HW = new Vector2(-patrolSpeed, rb.velocity.y);
            rb.velocity = HW;
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
            HW = new Vector2(patrolSpeed, rb.velocity.y);
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

    public void Attack()
    {
        if (attackCooldown <= 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer.Length != 0)
            {
                for (int i = 0; i < hitPlayer.Length; ++i)
                {
                    float chanceOfPoison = Random.Range(0, 100);
                    if(chanceOfPoison <= poisonChanse)
                    {
                        hitPlayer[i].GetComponent<Player>().GetPoisoned(10);
                    }
                    hitPlayer[i].GetComponent<Player>().TakeDamage(hpDamage, balanceDamage);
                }
            }
            attackCooldown = 1;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private bool PlayerInSight()
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
