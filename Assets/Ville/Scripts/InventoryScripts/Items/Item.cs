using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public bool isDefaultItem = false;

    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite icon = null;

    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);
        InventoryManager.instance.GetSelectedItem(true);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
        //InventoryManager.instance.Remove(this);
    }
}

public enum ItemType
{
    BuildingBlock, Tool
}

public enum ActionType
{
    Dig, Mine
}
