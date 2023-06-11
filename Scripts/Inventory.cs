using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<Item> itemList;
    public Inventory()
    { 
        itemList = new List<Item>();

        Debug.Log(itemList.Count);
    }
    public event EventHandler OnItemListChanged;
    public void AddItem(Item item)
    {
        bool IsInInventory = false;
        if (item.IsStackable())
        {
            foreach(Item it in itemList)
            {
                if (it.itemType == item.itemType)
                {
                    it.amount += item.amount;
                    IsInInventory = true;
                }
            }
            if(!IsInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInv = null;
            foreach (Item it in itemList)
            {
                if (it.itemType == item.itemType)
                {
                    it.amount -= item.amount;
                    itemInv = it;
                }
            }
            if (itemInv != null && itemInv.amount <= 0)
            {
                itemList.Remove(itemInv);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public List<Item> GetItemList() 
    {
        return itemList;
    }
}
