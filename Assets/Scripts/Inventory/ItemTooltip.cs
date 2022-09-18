using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Transform itemModifiersParent;
    [SerializeField] TextMeshProUGUI[] itemModifiers;

    void OnValidate() {
        itemModifiers = itemModifiersParent.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void ShowTooltip(EquippableItem item) {
        itemName.text = item.name;
        // itemType.text = item.equipmentType.ToString();
    
        int i = 0;
        for (; i < item.modifiers.Length; i++)
        {
            string color = "";
            if(item.modifiers[i].statType == StatType.Health) { color = "ff7a7a"; } else
            if(item.modifiers[i].statType == StatType.Shield) { color = "7ad7ff"; } else
            if(item.modifiers[i].statType == StatType.Damage) { color = "ffb77a"; } else
            if(item.modifiers[i].statType == StatType.ChargeRate) { color = "fff97a"; }

            var symbol = item.modifiers[i].statModType == StatModType.Flat ? "" : "%";
            itemModifiers[i].text = $"<sprite={(int)item.modifiers[i].statType}><color=#{color}> +{item.modifiers[i].value}{symbol}";
        }

        for (; i < itemModifiers.Length; i++) {
            itemModifiers[i].text = "";
        }

        itemDescription.text = item.description;
    }

}
