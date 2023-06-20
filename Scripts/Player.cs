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

    private IPlayerState _playerState = new IdlePlayerState();
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

        if (!stateInfo.IsName("sword_side"))
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
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
                rb.velocity = Vector2.zero;
                Anim.SetTrigger("IsAttacking");
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

interface IPlayerState
{
    IPlayerState UpdateState(Player player);
}

class IdlePlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        if (Input.GetKey(KeyCode.D)) return new RunningPlayerState();
        if (Input.GetKey(KeyCode.A)) return new RunningPlayerState();
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded) return new JumpingPlayerState();
        if (Input.GetButtonDown("Fire1")) return new AttackingPlayerState();
        return this;
    }
}
class RunningPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        player.Anim.Play("run_side");
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded) return new JumpingPlayerState();
        if (Input.GetKeyDown(KeyCode.A))
        {
            player.rb.velocity = new Vector2(-player.speed, player.rb.velocity.y);
            return new RunningPlayerState();
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.rb.velocity = new Vector2(player.speed, player.rb.velocity.y);
            return new RunningPlayerState();
        }
        
        return new IdlePlayerState();
    } 
}
class JumpingPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        player.Anim.Play("jump_side");
        player.rb.AddForce(player.transform.up * player.jumpForse, ForceMode2D.Impulse);
        return new AirbornePlayerState();
    }
}
class AirbornePlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        if(player.IsGrounded) return new IdlePlayerState();
        if((Input.GetKey(KeyCode.D))) player.rb.velocity = new Vector2(player.speed, player.rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.A)) player.rb.velocity = new Vector2(-player.speed, player.rb.velocity.y);
        return new AirbornePlayerState();
    }
}
class AttackingPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        player.Anim.Play("sword_side");
        return new IdlePlayerState();
    }
}