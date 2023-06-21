using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : DamagebleObject
{
    public float speed;
    private TestEnemy TE;

    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    private void move()
    {
        TE.LifeCicle(speed);
    }
    private void GetReferences()
    {
        TE = GetComponent<TestEnemy>();
    }
}