using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public bool isDefaultItem = false;

    [Header("Only gameplay")]
    public ItemType type;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite icon = null;

    [TextArea]
    public string description;


    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);


        if(type == ItemType.XpPotion)
        {
            ExperienceManager.instance.AddExperience(20); // test
        }
        if(type == ItemType.HealthPotion)
        {
            GameManager.manager.currentHealth += 20;
            AudioManager.instance.PlaySound(InventoryManager.instance.drinkHealthPSound);
        }
        if(type == ItemType.StaminaPotion)
        {
            GameManager.manager.currentStamina += 20;
            AudioManager.instance.PlaySound(InventoryManager.instance.drinkStaminaPSound);
        }
    }

    public void RemoveFromInventory()
    {
        //Inventory.instance.Remove(this);
    }
}

public enum ItemType
{
    HealthPotion, StaminaPotion, Axe, XpPotion
}
