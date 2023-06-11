using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;
    private void Awake()
    {
        Debug.Log("SpawnerStart");
        Debug.Log(transform.position);
        Debug.Log(item);
        ItemWorld.SpawnItemWorld(transform.position, item);
        Destroy(gameObject);
    }
}
