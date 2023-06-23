using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool movingRight = true;
    public void Patrol(Transform groundDetection, float distance, Rigidbody2D rb, float speed, float scaleX)
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider == false)
        {
            movingRight = !movingRight;
        }

        if (movingRight)
        {
            transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
            Vector2 HW = new Vector2(-speed, rb.velocity.y);
            rb.velocity = HW;
        }
        else
        {
            transform.localScale = new Vector2(scaleX, transform.localScale.y);
            Vector2 HW = new Vector2(speed, rb.velocity.y);
            rb.velocity = HW;
        }
    }

    public float DashPatrol(Transform groundDetection, Transform secGroundDetection, float dashCooldown, float dashDelay, float distance, Rigidbody2D rb, float speed, float scaleX)
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        RaycastHit2D secGroundInfo = Physics2D.Raycast(secGroundDetection.position, Vector2.down, distance);

        if ((groundInfo.collider == false)|| (secGroundInfo.collider == false))
        {
            movingRight = !movingRight;
        }

        if(dashCooldown <= 0)
        {
            if (movingRight)
            {
                transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
                rb.AddForce(Vector2.left * speed);
            }
            else
            {
                transform.localScale = new Vector2(scaleX, transform.localScale.y);
                rb.AddForce(Vector2.right * speed);
            }
            dashCooldown = dashDelay;
        }
        else
        {
            dashCooldown -= Time.deltaTime;
        }
        return dashCooldown;
    }

    public void Pursuit(Transform groundDetection, Transform player, float distance, Rigidbody2D rb, float speed, float scaleX)
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        if (groundInfo.collider)
        {
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
                Vector2 HW = new Vector2(-speed, rb.velocity.y);
                rb.velocity = HW;
            }
            else
            {
                transform.localScale = new Vector2(scaleX, transform.localScale.y);
                Vector2 HW = new Vector2(speed, rb.velocity.y);
                rb.velocity = HW;
            }
        }
    }

    public float DashPursuit(Transform groundDetection, Transform secGroundDetection, Transform player, float dashCooldown, float dashDelay, float distance, Rigidbody2D rb, float speed, float scaleX)
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        RaycastHit2D secGroundInfo = Physics2D.Raycast(secGroundDetection.position, Vector2.down, distance);

        if ((groundInfo.collider) && (secGroundInfo.collider))
        {
            if(dashCooldown <= 0)
            {
                if (player.position.x < transform.position.x)
                {
                    transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
                    rb.AddForce(Vector2.left * speed);
                }
                else
                {
                    transform.localScale = new Vector2(scaleX, transform.localScale.y);
                    rb.AddForce(Vector2.right * speed);
                }
                dashCooldown = dashDelay;
            }
            else
            {
                dashCooldown -= Time.deltaTime;
            }
        }
        return dashCooldown;
    }

    public float Attack(Transform attackPoint, LayerMask playerLayer, Transform player, float attackCooldown, float attackDelay, float attackRange, float hpDamage, float balanceDamage, float scaleX)
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(scaleX, transform.localScale.y);
        }
        if (attackCooldown <= 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer.Length != 0)
            {
                for (int i = 0; i < hitPlayer.Length; ++i)
                {
                    hitPlayer[i].GetComponent<Player>().TakeDamage(hpDamage, balanceDamage);
                }
            }
            attackCooldown = attackDelay;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
        return attackCooldown;
    }

    public float AttackPoison(Transform attackPoint, LayerMask playerLayer, Transform player, float attackCooldown, float attackDelay, float attackRange, float hpDamage, float balanceDamage, float poisonChanse, float poisonTime, float scaleX)
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(scaleX, transform.localScale.y);
        }
        if (attackCooldown <= 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer.Length != 0)
            {
                for (int i = 0; i < hitPlayer.Length; ++i)
                {
                    float chanceOfPoison = Random.Range(0, 100);
                    if (chanceOfPoison <= poisonChanse)
                    {
                        hitPlayer[i].GetComponent<Player>().GetPoisoned(poisonTime);
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
        return attackCooldown;
    }

    public Vector2 AttackLightAndHeavy(Transform attackPoint, LayerMask playerLayer, Transform player, float attackCooldown, float strongAttackCooldown, float weakAttackDelay, float strongAttackDelay, float attackRange, float weakHpDamage, float strongHpDamage,  float weakBalanceDamage, float strongBalanceDamage, float scaleX)
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(scaleX, transform.localScale.y);
        }
        if (attackCooldown <= 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer.Length != 0)
            {
                for (int i = 0; i < hitPlayer.Length; ++i)
                {
                    if (strongAttackCooldown <= 0)
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
                        attackCooldown = weakAttackDelay;
                        strongAttackCooldown = strongAttackDelay;
                    }
                    else
                    {
                        hitPlayer[i].GetComponent<Player>().TakeDamage(weakHpDamage, weakBalanceDamage);
                        attackCooldown = weakAttackDelay;
                    }
                }
            }
        }
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (strongAttackCooldown > 0)
        {
            strongAttackCooldown -= Time.deltaTime;
        }
        Vector2 ret = new(attackCooldown, strongAttackCooldown);
        return ret;
    }

    public bool DashAttack(Transform attackPoint, Transform player, Rigidbody2D rb, LayerMask playerLayer, float speed, float attackRange, float hpDamage, float scaleX)
    {
        if(Mathf.Abs(player.position.x - transform.position.x) >= attackRange)
        {
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector2(scaleX * -1, transform.localScale.y);
                Vector2 HW = new Vector2(-speed, rb.velocity.y);
                rb.velocity = HW;
            }
            else
            {
                transform.localScale = new Vector2(scaleX, transform.localScale.y);
                Vector2 HW = new Vector2(speed, rb.velocity.y);
                rb.velocity = HW;
            }
            return true;
        }
        else
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer.Length != 0)
            {
                for (int i = 0; i < hitPlayer.Length; ++i)
                {
                    hitPlayer[i].GetComponent<Player>().TakeDamage(hpDamage, 100);
                }
            }
            return false;
        }
    }

    /*
    Transform firepoint;
    GameObject[] projectiles;

    public float RangedAtack(GameObject[] projectiles, float cooldown)
    {
        if(cooldown <= 0)
        {
            projectiles[FindRangedTile(projectiles)].transform.position = firepoint.position;
            projectiles[FindRangedTile(projectiles)].GetComponent<EnemyProjectile>().ActivateProjectile();
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
        return cooldown;
    }
    */

    private int FindRangedTile(GameObject[] projectiles)
    {
        for(int i = 0; i < projectiles.Length; ++i)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public bool PlayerInSight(LayerMask playerLayer, BoxCollider2D boxCollider, float range, float colliderDistance, float high)
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y + high, boxCollider.bounds.size.z),
            0,
            Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }
}
