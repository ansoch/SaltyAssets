using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator Anim { get; private set; }

    public float speed;
    public float jumpForse;
    public Rigidbody2D rb { get; private set; }
    public Transform groundCheck;
    public LayerMask WhatIsGround;
    public float radiusGroundCheck;
    public bool IsGrounded { get; private set; } = true;
    private float signPreviousFrame;
    private float signCurrentFrame;
    private Vector3 _leftFlip = new Vector3(0, 180, 0);
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    private bool _isJumping = false;
    private bool _isRunning = false;
    private bool _isAttacking = false;

    private float _facingRight = -1;

    //private IPlayerState _playerState = new IdlePlayerState();

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0f;
        rb.angularDrag = 0f;
        inventory = new Inventory();
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null && Input.GetKey(KeyCode.E))
        {
            if (inventory.GetItemList().Count < 8)
            {

                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        if (collision.CompareTag("Box"))
        {
            //heldObject = collision.gameObject;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    private void Flip()
    {
        if (Input.GetKey(KeyCode.A))
            transform.rotation = Quaternion.Euler(Vector3.zero);
        else if (Input.GetKey(KeyCode.D))
            transform.rotation = Quaternion.Euler(_leftFlip);
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("sword_side") && !stateInfo.IsName("slide_side"))
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);

            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                _facingRight = 1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                _facingRight = -1f;
            }
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            {
                rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
                //Anim.Play("jump_side");
                Anim.SetTrigger("IsJumping");
            }
            if (Input.GetButtonDown("Fire1"))
            {
                //Anim.Play("AttackingCatana");
                rb.velocity = new Vector2(speed*_facingRight*0.75f, rb.velocity.y);
                Anim.SetTrigger("IsAttacking");
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)){
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
                rb.velocity = new Vector2(speed * _facingRight * 2f, rb.velocity.y);
                Anim.SetTrigger("IsSliding");
                //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
            }
            Flip();
        }
        //_playerState = _playerState.UpdateState(this);
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        Anim.SetFloat("SpeedX", Math.Abs(rb.velocity.x));
        Anim.SetBool("IsRunning", Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A));
        Anim.SetBool("IsGrounded", IsGrounded);
    }
}