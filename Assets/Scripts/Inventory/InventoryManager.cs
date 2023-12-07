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

    [Header("Items")]
    [SerializeField] Item healthPotion;
    [SerializeField] Item staminaPotion;
    [SerializeField] Item xpPotion;


    private void Start()
    {
        if(GameManager.manager != null)
        {
            int healthPotionCount = GameManager.manager.healthPotions;
            int staminaPotionCount = GameManager.manager.staminaPotions;
            int xpPotionCount = GameManager.manager.xpPotions; ;

            if(healthPotionCount != 0)
            {
                for (int i = 0; i < healthPotionCount; i++)
                {
                    AddItem(healthPotion);
                }
            }

            if (staminaPotionCount != 0)
            {
                for (int i = 0; i < staminaPotionCount; i++)
                {
                    AddItem(staminaPotion);
                }
            }

            if (xpPotionCount != 0)
            {
                for (int i = 0; i < xpPotionCount; i++)
                {
                    AddItem(xpPotion);
                }
            }
        }
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

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseInventorySlot(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseInventorySlot(1);
        }
    }

    public void UseInventorySlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventorySlots.Length)
        {
            InventorySlot slot = inventorySlots[slotIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                itemInSlot.UseItem();
            }
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
