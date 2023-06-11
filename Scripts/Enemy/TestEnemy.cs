using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 HW;
    public void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Move(float speed)
    {
        HW = new Vector2(speed, rb.velocity.y);
        rb.velocity = HW;
    }
}
