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

    [SerializeField] private Animator playerAnimator;

    private AnimatorStateInfo stateInfo;
    private AnimatorStateInfo playerStateInfo;
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
        if(!stateInfo.IsName("weapon_sword_side") && !playerStateInfo.IsName("slide_side"))
        {
            if(Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("IsAttacking");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange,DamagableLayerMask);
                if (enemies.Length != 0)
                {
                    for (int i = 0; i < enemies.Length; ++i)
                    {
                        enemies[i].GetComponent<DamagebleObject>().TakeDamage(Damage);
                    }
                }
            }
        }
    }
    private void GetReferences()
    {

    }
    private void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        Attack();
    }
    private void FixedUpdate()
    {
        
    }
}
