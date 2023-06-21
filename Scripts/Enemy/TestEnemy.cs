using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    private Vector2 HW;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float runSpeed = 4;
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float stepSpeed = 1;

    [SerializeField] private float distance;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private Transform groundDetection;

    [SerializeField] private float stepDistance = 3;
    [SerializeField] private float stopDistance = 2;

    [SerializeField] private float range;
    [SerializeField] private float high;
    [SerializeField] private float colliderDistance;

    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;

    private float damage;

    public void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void LifeCicle(float speed)
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if(PlayerInSight())
        {
            if (distToPlayer >= stepDistance)
            {
                Running();
            }
            else
            {
                if (distToPlayer - stopDistance > 0.1)
                {
                    Steping();
                }
                else
                {
                    Fighting();
                }
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

        if(movingRight)
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

    public void Fighting()
    {
        HW = new Vector2(runSpeed, rb.velocity.y);
        rb.velocity = HW;

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
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