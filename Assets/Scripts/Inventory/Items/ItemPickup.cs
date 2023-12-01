using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public string itemName;
    public int pickUpItemID;


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
            //CollectItem();
            AddItemIDToArray(this.pickUpItemID);
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

    void AddItemIDToArray(int newItemID)
    {
        int[] newItemIDs = new int[GameManager.manager.pickUpItemIDs.Length + 1];
        for(int i = 0; i < GameManager.manager.pickUpItemIDs.Length; i++)
        {
            newItemIDs[i] = GameManager.manager.pickUpItemIDs[i];
        }
        newItemIDs[GameManager.manager.pickUpItemIDs.Length] = newItemID;
        GameManager.manager.pickUpItemIDs = newItemIDs;
    }

    void CollectItem()
    {
        //PlayerPrefs.SetInt("ItemCollected", 1);
    }
}
