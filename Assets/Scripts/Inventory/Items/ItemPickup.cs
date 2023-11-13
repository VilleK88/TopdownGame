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
        //bool wasPickedUp = Inventory.instance.Add(item);
        bool wasPickedUp = InventoryManager.instance.AddItem(item);
        //InventoryManager.instance.AddItem(item);

        if(wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
