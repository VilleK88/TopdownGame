using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public string itemName;

    private void Start()
    {
        /*if(GameManager.manager != null && GameManager.manager.isGameLoaded)
        {
            if (PlayerPrefs.GetInt("ItemCollected", 0) == 1)
            {
                GameObject healthPotion = GameObject.Find("HealthPotion");
                if (healthPotion != null)
                {
                    Destroy(healthPotion);
                }
            }
        }*/
    }

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking up item " + item.name);
        bool wasPickedUp = InventoryManager.instance.AddItem(item);

        if(wasPickedUp)
        {
            AddItemToArray(item);
            GameManager.manager.AddItemPickupToArray(this);
            //CollectItem();
            if (item.type == ItemType.HealthPotion || item.type == ItemType.StaminaPotion ||
                item.type == ItemType.XpPotion)
            {
                AudioManager.instance.PlaySound(InventoryManager.instance.potionPickUpSound);
            }
            if(item.type == ItemType.WoodenAxe)
            {
                AudioManager.instance.PlaySound(InventoryManager.instance.weaponPickUpSound);
            }

            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    void AddItemToArray(Item item)
    {
        Item[] newItemArray = new Item[GameManager.manager.items.Length + 1];
        for (int i = 0; i < GameManager.manager.items.Length; i++)
        {
            newItemArray[i] = GameManager.manager.items[i];
        }
        newItemArray[GameManager.manager.items.Length] = item;
        GameManager.manager.items = newItemArray;
    }

    /*void AddItemPickupToArray(ItemPickup itemPickup)
    {
        ItemPickup[] newItemPickupArray = new ItemPickup[GameManager.manager.collectedItems.Length + 1];
        for(int i = 0; i < GameManager.manager.collectedItems.Length; i++)
        {
            newItemPickupArray[i] = GameManager.manager.collectedItems[i];
        }
        newItemPickupArray[GameManager.manager.collectedItems.Length] = itemPickup;
        GameManager.manager.collectedItems = newItemPickupArray;
    }*/

    void CollectItem()
    {
        //PlayerPrefs.SetInt("ItemCollected", 1);
    }
}
