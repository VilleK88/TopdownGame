using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
}
