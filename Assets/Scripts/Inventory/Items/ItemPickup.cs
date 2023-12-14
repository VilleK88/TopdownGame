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
        //Debug.Log("Picking up item " + item.name);
        bool wasPickedUp = InventoryManager.instance.AddItem(item);

        if(wasPickedUp)
        {
            item.itemID = pickUpItemID;
            //AddItemIDToArray(this.item.itemID);
            AddPickUpItemIDToArray(this.pickUpItemID);
            if (item.type == ItemType.HealthPotion || item.type == ItemType.StaminaPotion ||
                item.type == ItemType.XpPotion)
            {
                AudioManager.instance.PlaySound(InventoryManager.instance.potionPickUpSound);
            }
            if(item.type == ItemType.WoodenAxe)
            {
                AudioManager.instance.PlaySound(InventoryManager.instance.weaponPickUpSound);
                GameManager.manager.woodenAxeCollected = true;
            }

            if(item.type == ItemType.HealthPotion)
            {
                GameManager.manager.healthPotions++;
            }
            if (item.type == ItemType.StaminaPotion)
            {
                GameManager.manager.staminaPotions++;
            }
            if (item.type == ItemType.XpPotion)
            {
                GameManager.manager.xpPotions++;
            }

            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    public void PickUpFromGameManager()
    {
        Debug.Log("Picking up item " + item.name);
        bool wasPickedUp = InventoryManager.instance.AddItem(item);

        if (wasPickedUp)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    void AddPickUpItemIDToArray(int newPickUpItemID)
    {
        int[] newPickUpItemIDs = new int[GameManager.manager.pickUpItemIDs.Length + 1];
        for(int i = 0; i < GameManager.manager.pickUpItemIDs.Length; i++)
        {
            newPickUpItemIDs[i] = GameManager.manager.pickUpItemIDs[i];
        }
        newPickUpItemIDs[GameManager.manager.pickUpItemIDs.Length] = newPickUpItemID;
        GameManager.manager.pickUpItemIDs = newPickUpItemIDs;
    }
}
