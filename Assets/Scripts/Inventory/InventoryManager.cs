using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public WeaponSlot weaponSlot;
    public bool weaponOnHand = false;
    public GameObject inventoryUI;
    public GameObject equipmentUI;
    public GameObject inventoryItemPrefab;
    [HideInInspector] public InventorySlot slot;
    [HideInInspector] public InventoryItem itemInSlot;
    public bool isInventoryActive = false;

    [Header("Audio")]
    public AudioClip drinkingSound;
    public AudioClip potionPickUpSound;
    public AudioClip weaponPickUpSound;


    private void Start()
    {
        /*if (GameManager.manager != null && GameManager.manager.items != null)
        {
            foreach (Item item in GameManager.manager.items)
            {
                AddItem(item);
            }
        }*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            equipmentUI.SetActive(!equipmentUI.activeSelf);
            isInventoryActive = inventoryUI.activeSelf;
        }

        if(weaponSlot.GetComponentInChildren<InventoryItem>() != null)
        {
            weaponOnHand = true;
        }
        else
        {
            weaponOnHand = false;
        }
    }

    public bool AddItem(Item item)
    {
        // Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
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
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
