using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    [SerializeField] private float balance = 100f;

    [SerializeField] private float balanceHealTime = 6;
    [SerializeField] private bool isTimer = false;

    [SerializeField] private float stunTime = 4;
    [SerializeField] private bool isStuned = false;

    [SerializeField] private float poisonTime = 0;
    [SerializeField] private float poisonDamage = 0.01f;

    public float speed;
    public float jumpForse;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask WhatIsGround;
    public float radiusGroundCheck;
    private bool isGrounded = true;
    private float signPreviousFrame;
    private float signCurrentFrame;
    private Vector3 _leftFlip = new Vector3(0, 180, 0);
    private bool isJumping = false;
    // Start is called before the first frame update
    void Start()
    {
        rb.drag = 0f;
        rb.angularDrag = 0f;
    }
    private void Flip()
    {
        if (rb.velocity.x < 0)
            transform.rotation = Quaternion.Euler(_leftFlip);
        else if (rb.velocity.x > 0)
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        if(isStuned == true)
        {
            stunTime -= Time.deltaTime;
            balance = 100f;
            if (stunTime <= 0)
            {
                isStuned = false;
                stunTime = 4;
            }
        }
        else
        {
            if (isTimer == true)
            {
                balanceHealTime -= Time.deltaTime;
                if (balanceHealTime <= 0)
                {
                    balance = 100f;
                    isTimer = false;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
            }
        }
        GetDamageByPoison();
        Flip();
    }

    public void TakeDamage(float hpDamage, float balanceDamage)
    {
        hp -= hpDamage;
        balance -= balanceDamage;
        balanceHealTime = 6;
        isTimer = true;
        if (hp == 0)
        {
            Die();
        }
        if(balance == 0)
        {
            isTimer = false;
            isStuned = true;
        }
    }

    public void Die()
    {
        hp = 100f;
    }

    public void GetPoisoned(float poisonAmount)
    {
        poisonTime = poisonAmount;
    }

    public void GetDamageByPoison()
    {
        if(poisonTime > 0)
        {
            poisonTime -= Time.deltaTime;
            hp -= poisonDamage;
        }
    }

    public void KnockBack(bool direction)
    {
        if(direction)
        {
            rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
            rb.AddForce(transform.right * jumpForse, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
            rb.AddForce(transform.forward * jumpForse, ForceMode2D.Impulse);
        }
    }
}

