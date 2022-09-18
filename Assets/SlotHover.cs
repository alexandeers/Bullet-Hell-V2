using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotHover : MonoBehaviour
{
    [SerializeField] Transform itemSlotsParent;
    ItemSlot[] itemSlots;
    SlotUI[] images;

    void OnValidate() {
        images = GetComponentsInChildren<SlotUI>();

        if(itemSlotsParent != null) 
            itemSlots = itemSlotsParent.GetComponentsInChildren<ItemSlot>();
        
        for (int i = 0; i < itemSlots.Length; i++)
        {
            images[i].itemSlot = itemSlots[i];
        }
    }
}
