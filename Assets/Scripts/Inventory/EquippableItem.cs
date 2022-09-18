using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType {
    Shard,
    Unavailable
}

public enum StatType {
    Health,
    Shield,
    Damage,
    ChargeRate
}

[System.Serializable]
public class EquipmentModifier {
    [SerializeField] public StatType statType;
    [SerializeField] public StatModType statModType;
    [SerializeField] public float value;
}

[System.Serializable]
[CreateAssetMenu]
public class EquippableItem : Item
{
    public EquipmentModifier[] modifiers; 
    public EquipmentType equipmentType;

    public void Equip(PlayerStats c) {
        foreach(EquipmentModifier modifier in modifiers) {
            if(modifier.statType == StatType.Health) c.maxHealth.AddModifier(new StatModifier(modifier.value, modifier.statModType, this));
            if(modifier.statType == StatType.Shield) c.maxShield.AddModifier(new StatModifier(modifier.value, modifier.statModType, this));
            if(modifier.statType == StatType.Damage) c.damage.AddModifier(new StatModifier(modifier.value, modifier.statModType, this));
            if(modifier.statType == StatType.ChargeRate) c.chargeRate.AddModifier(new StatModifier(modifier.value, modifier.statModType, this));
        }
    }

    public void Unequip(PlayerStats c) {
        foreach(EquipmentModifier modifier in modifiers) {
            if(modifier.statType == StatType.Health) c.maxHealth.RemoveAllModifiersFromSource(this);
            if(modifier.statType == StatType.Shield) c.maxShield.RemoveAllModifiersFromSource(this);
            if(modifier.statType == StatType.Damage) c.damage.RemoveAllModifiersFromSource(this);
            if(modifier.statType == StatType.ChargeRate) c.chargeRate.RemoveAllModifiersFromSource(this);
        }
    }
}
