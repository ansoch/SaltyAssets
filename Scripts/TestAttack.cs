using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    public Transform AttackPoint;
    public LayerMask DamagableLayerMask;
    public float Damage;
    public float AttackRange;
    public float TimeBtwAttack;
    private float timer;
    Animator anim;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Attack()
    {
        if(timer <= 0)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                anim.Play("AttackCatana");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange,DamagableLayerMask);
                if (enemies.Length != 0)
                {
                    for (int i = 0; i < enemies.Length; ++i)
                    {
                        enemies[i].GetComponent<DamagebleObject>().TakeDamage(Damage);
                    }
                }
            }
            
            timer = 0;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
    private void GetReferences()
    {

    }
    private void Update()
    {
        Attack();
    }
}