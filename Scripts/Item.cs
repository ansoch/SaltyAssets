using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public enum ItemType
    {
        Weapon,
        HealthPotion,
        ManaPotion,
        Coin,
    }
    public ItemType itemType;
    public int amount;
    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.Weapon: return ItemAssets.Instance.weapon;
            case ItemType.HealthPotion: return ItemAssets.Instance.healtPotionSprite;
            case ItemType.ManaPotion: return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Coin: return ItemAssets.Instance.coinSprite;
        }
    }
    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.Coin:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
                return true;
            case ItemType.Weapon:
                return false;
        }
    }
}
