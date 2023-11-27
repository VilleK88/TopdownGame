using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

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
            if (item.type == ItemType.HealthPotion || item.type == ItemType.StaminaPotion ||
                item.type == ItemType.XpPotion)
            {
                AudioManager.instance.PlaySound(InventoryManager.instance.potionPickUpSound);
            }
            if(item.type == ItemType.WoodenAxe)
            {
                AudioManager.instance.PlaySound(InventoryManager.instance.weaponPickUpSound);
            }

            Destroy(gameObject);
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
}
