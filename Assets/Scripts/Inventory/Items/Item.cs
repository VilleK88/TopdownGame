using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public bool isDefaultItem = false;

    public ItemType type;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite icon = null;

    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);
        if(type == ItemType.Stamina)
        {
            ExperienceManager.instance.AddExperience(20); // test
        }
        RemoveFromInventory();
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}

public enum ItemType
{
    Health, Stamina
}
