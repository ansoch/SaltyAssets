using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
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
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        if (rb.velocity.x < 0)
            transform.rotation = Quaternion.Euler(_leftFlip);
        else if (rb.velocity.x > 0)
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            anim.Play("Running");
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            anim.Play("Running");
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
            anim.Play("Jump3");
        }
        Flip();
    }
}

