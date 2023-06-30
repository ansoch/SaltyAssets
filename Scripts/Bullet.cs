using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float hpDamage;
    [SerializeField] private float balanceDamage;
    [SerializeField] private float speed;

    private void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(hpDamage, balanceDamage);
        }
        Destroy(gameObject);
    }
}
