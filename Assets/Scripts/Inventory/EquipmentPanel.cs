using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{

    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event System.Action<ItemSlot> OnPointerEnterEvent;
    public event System.Action<ItemSlot> OnPointerExitEvent;
    public event System.Action<ItemSlot> OnRightClickEvent;
    public event System.Action<ItemSlot> OnBeginDragEvent;
    public event System.Action<ItemSlot> OnEndDragEvent;
    public event System.Action<ItemSlot> OnDragEvent;
    public event System.Action<ItemSlot> OnDropEvent;

    void Start() {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            equipmentSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            equipmentSlots[i].OnRightClickEvent += OnRightClickEvent;
            equipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            equipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
            equipmentSlots[i].OnDragEvent += OnDragEvent;
            equipmentSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    void Update() {
        
        if(Input.GetKeyDown(KeyCode.F)) {
            print("SUBSCRIBED");
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                // equipmentSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
            }
        }
    }

    void OnValidate() {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }


    public bool AddItem(EquippableItem item, out EquippableItem previousItem) 
    {
        for (int i = 0; i < equipmentSlots.Length; i++) 
        {
            if (equipmentSlots[i].equipmentType == item.equipmentType && equipmentSlots[i].item == null) 
            {
                previousItem = (EquippableItem)equipmentSlots[i].item;
                equipmentSlots[i].item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item) {
        for (int i = 0; i < equipmentSlots.Length; i++) 
        {
            if (equipmentSlots[i].item == item) 
            {
                equipmentSlots[i].item = null;
                return true;
            }
        }
        return false;
    }
}