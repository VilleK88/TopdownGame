using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IDropHandler
{
    public Image image;
    //public bool weaponOnHand = false;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
        if (transform.childCount == 0 && inventoryItem.name == "Axe")
        {
            inventoryItem.parentAfterDrag = transform;
            //weaponOnHand = true;
        }
    }
}
