using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int maxStackedItems = 4;
    public InventorySlot2[] inventorySlots;
    public GameObject inventoryItemPrefab;

    InventorySlot2 slot;
    InventoryItem itemInSlot;

    int selectedSlot = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                if(hit.collider.gameObject == inventorySlots[i])
                {
                    selectedSlot = i;
                    break;
                }
            }
        }

        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if ((isNumber && number > 0 && number < 8))
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        // Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //InventorySlot2 slot = inventorySlots[i];
            //InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            slot = inventorySlots[i];
            itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot2 slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot2 slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot2 slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if(use == true)
            {
                itemInSlot.count--;
                if(itemInSlot.count >= 0)
                {
                    Debug.Log("Used item: " + itemInSlot);
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    Debug.Log("Used item: " + itemInSlot);
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }

        return null;
    }
}