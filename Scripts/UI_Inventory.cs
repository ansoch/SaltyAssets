using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Player player;
    private Item selectedInventoryItem;
    private List<RectTransform> itemSlots = new List<RectTransform>();
    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }
    public void SetPlayer(Player pl)
    {
        this.player = pl;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }
    private void SelectInventoryItem(Item item)
    {
        selectedInventoryItem = item;
        Item CopyItem = new Item { amount = item.amount, itemType = item.itemType  };
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.GetPosition(), CopyItem);
    }
    private void RefreshInventoryItems()
    {
        foreach (var slot in itemSlots)
        {
            Destroy(slot.gameObject);
        }
        itemSlots.Clear();
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 100f;
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x*itemSlotCellSize, -1*y*itemSlotCellSize);
            itemSlots.Add(itemSlotRectTransform);
            Button button = itemSlotRectTransform.GetComponent<Button>();
            button.onClick.AddListener(() => SelectInventoryItem(item));
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI textUI = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                textUI.SetText(item.amount.ToString());
            }
            else
            {
                textUI.SetText("");
            }
            x++;
            if(x==4)
            {
                x = 0;
                y++;
            }
        }
    }
}
