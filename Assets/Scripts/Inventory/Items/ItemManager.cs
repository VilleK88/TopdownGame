using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameManager manager;
    public ItemPickup[] itemPickups;
    public int[] itemIDs;
    public int index;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        ItemPickup[] pickupsInScene = FindObjectsOfType<ItemPickup>();

        foreach(ItemPickup pickup in pickupsInScene)
        {
            if(GameManager.manager.pickUpItemIDs.Contains(pickup.pickUpItemID))
            {
                Destroy(pickup.gameObject);
            }
        }
    }
}
