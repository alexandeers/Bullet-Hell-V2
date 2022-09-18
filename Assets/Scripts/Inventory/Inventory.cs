using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] List<Item> startingItems;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;

    public event System.Action<ItemSlot> OnPointerEnterEvent;
    public event System.Action<ItemSlot> OnPointerExitEvent;
    public event System.Action<ItemSlot> OnRightClickEvent;
    public event System.Action<ItemSlot> OnBeginDragEvent;
    public event System.Action<ItemSlot> OnEndDragEvent;
    public event System.Action<ItemSlot> OnDragEvent;
    public event System.Action<ItemSlot> OnDropEvent;

    void Start() {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            itemSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            itemSlots[i].OnRightClickEvent += OnRightClickEvent;
            itemSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            itemSlots[i].OnEndDragEvent += OnEndDragEvent;
            itemSlots[i].OnDragEvent += OnDragEvent;
            itemSlots[i].OnDropEvent += OnDropEvent;
        }

        SetStartingItems();
    }

    void OnValidate() 
    {
        if(itemsParent != null) {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }

        SetStartingItems();
    }

    void SetStartingItems() 
    {
        int i = 0;
        for(; i < startingItems.Count && i < itemSlots.Length; i++) {
            itemSlots[i].item = startingItems[i];
        }

        for(; i < itemSlots.Length; i++) {
            itemSlots[i].item = null;
        }
    }

    public bool AddItem(Item item) 
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].item == null) {
                itemSlots[i].item = item;
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item) 
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].item == item) {
                itemSlots[i].item = null;
                return true;
            }
        }
        return false;
    }

    public bool IsFull()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].item == null) {
                return false;
            }
        }
        return true;
    }
}
