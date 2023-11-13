using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public Item itemType;

    public ItemInstance(Item itemData)
    {
        itemType = itemData;
    }
}
