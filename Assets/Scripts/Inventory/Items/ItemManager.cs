using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameManager manager;
    public ItemPickup[] itemPickups;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();

        if (manager != null && manager.collectedItems != null)
        {
            foreach(ItemPickup pickup in itemPickups)
            {
                foreach(ItemPickup collectedItem in manager.collectedItems)
                {
                    if(pickup.name == collectedItem.itemName)
                    {
                        pickup.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
