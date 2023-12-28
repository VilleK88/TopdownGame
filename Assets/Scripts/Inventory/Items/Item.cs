using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public int itemID;
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

        //Debug.Log("Using " + name);


        if(type == ItemType.XpPotion)
        {
            ExperienceManager.instance.AddExperience(20); // test
            AudioManager.instance.PlaySound(InventoryManager.instance.drinkingSound);
            GameManager.manager.xpPotions--;
        }
        if(type == ItemType.HealthPotion)
        {
            if(GameManager.manager.healthPotionPlus)
            {
                GameManager.manager.currentHealth += 30;
            }
            else
            {
                GameManager.manager.currentHealth += 20;
            }
            AudioManager.instance.PlaySound(InventoryManager.instance.drinkingSound);
            GameManager.manager.healthPotions--;
        }
        if(type == ItemType.StaminaPotion)
        {
            if(GameManager.manager.staminaPotionPlus)
            {
                GameManager.manager.currentStamina += 30;
            }
            else
            {
                GameManager.manager.currentStamina += 20;
            }
            AudioManager.instance.PlaySound(InventoryManager.instance.drinkingSound);
            GameManager.manager.staminaPotions--;
        }
    }

    public void RemoveFromInventory()
    {
        //Inventory.instance.Remove(this);
    }
}

public enum ItemType
{
    HealthPotion, StaminaPotion, WoodenAxe, XpPotion
}
