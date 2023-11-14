using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour //CharacterStats
{
    public PlayerHealth playerHealth;
    public PlayerHitbox playerHitbox;
    /*private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            playerHealth.armor.AddModifier(newItem.armorModifier);
            playerHealth.damage.AddModifier(newItem.damageModifier);

            playerHitbox.armor.AddModifier(newItem.armorModifier);
            playerHitbox.damage.AddModifier(newItem.damageModifier);
        }

        if(oldItem != null)
        {
            playerHealth.armor.RemoveModifier(oldItem.armorModifier);
            playerHealth.damage.RemoveModifier(oldItem.damageModifier);

            playerHitbox.armor.RemoveModifier(oldItem.armorModifier);
            playerHitbox.damage.RemoveModifier(oldItem.damageModifier);
        }
    }*/
}
