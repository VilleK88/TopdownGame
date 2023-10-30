using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour //CharacterStats
{
    public TestPlayerHealth testPlayerHealth;
    public TestPlayerHitbox playerHitbox;
    private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            testPlayerHealth.armor.AddModifier(newItem.armorModifier);
            testPlayerHealth.damage.AddModifier(newItem.damageModifier);

            playerHitbox.armor.AddModifier(newItem.armorModifier);
            playerHitbox.damage.AddModifier(newItem.damageModifier);
        }

        if(oldItem != null)
        {
            testPlayerHealth.armor.RemoveModifier(oldItem.armorModifier);
            testPlayerHealth.damage.RemoveModifier(oldItem.damageModifier);

            playerHitbox.armor.RemoveModifier(oldItem.armorModifier);
            playerHitbox.damage.RemoveModifier(oldItem.damageModifier);
        }
    }
}
