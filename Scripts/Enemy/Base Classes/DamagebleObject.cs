using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagebleObject : MonoBehaviour
{
    [SerializeField] private float hp;
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
