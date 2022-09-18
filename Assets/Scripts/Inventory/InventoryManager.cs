using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    ItemSlot draggedSlot;

    void Start() {
        statPanel.SetStats(PlayerHandler.i.playerStats.maxHealth, PlayerHandler.i.playerStats.maxShield, PlayerHandler.i.playerStats.damage, PlayerHandler.i.playerStats.chargeRate);
        statPanel.UpdateStatValues();
    }

    void OnValidate() {
        if(itemTooltip == null) {
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    void Awake() {
        //Right click
        inventory.OnRightClickEvent += Equip;
        equipmentPanel.OnRightClickEvent += Unequip;
        //Pointer enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //Pointer exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        //Begin drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        //End drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
    }

    private void Equip(ItemSlot itemSlot) {
        EquippableItem equippableItem = itemSlot.item as EquippableItem;
        if(equippableItem != null)
        {
            Equip(equippableItem);
        }
    }

    private void Unequip(ItemSlot itemSlot) {
        EquippableItem equippableItem = itemSlot.item as EquippableItem;
        if(equippableItem != null)
        {
            Unequip(equippableItem);
        }
    }

    void ShowTooltip(ItemSlot itemSlot) {
        EquippableItem equippableItem = itemSlot.item as EquippableItem;
        if(equippableItem != null)
        {
            itemTooltip .ShowTooltip(equippableItem);
        }
    }

    void HideTooltip(ItemSlot itemSlot) {
        //Hide tooltip
    }

    void BeginDrag(ItemSlot itemSlot) {
        if(itemSlot.item != null) {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.item.icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    void EndDrag(ItemSlot itemSlot) {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    void Drag(ItemSlot itemSlot) {
        if(draggableItem.enabled)
            draggableItem.transform.position = Input.mousePosition;
    }

    void Drop(ItemSlot dropItemSlot) {
        if(dropItemSlot.item == draggedSlot.item) return;
        if(dropItemSlot.CanReceiveItem(draggedSlot.item) && draggedSlot.CanReceiveItem(dropItemSlot.item)) {

            EquippableItem dragItem = draggedSlot.item as EquippableItem;
            EquippableItem dropItem = dropItemSlot.item as EquippableItem;

            if(draggedSlot is EquipmentSlot) {
                if(dragItem != null) dragItem.Unequip(PlayerHandler.i.playerStats);
                if(dropItem != null) dropItem.Equip(PlayerHandler.i.playerStats);
            }
            if(dropItemSlot is EquipmentSlot) {
                if(dragItem != null) dragItem.Equip(PlayerHandler.i.playerStats);
                if(dropItem != null) dropItem.Unequip(PlayerHandler.i.playerStats);
            }

            statPanel.UpdateStatValues();

            Item draggedItem = draggedSlot.item;
            draggedSlot.item = dropItemSlot.item;
            dropItemSlot.item = draggedItem;
        }
    }

    public void Equip(EquippableItem item) 
    {
        if(inventory.RemoveItem(item)) 
        {
            EquippableItem previousItem;
            if(equipmentPanel.AddItem(item, out previousItem)) 
            {
                if(previousItem != null) 
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(PlayerHandler.i.playerStats);
                }
                item.Equip(PlayerHandler.i.playerStats);
                statPanel.UpdateStatValues();
            } 
            else 
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item) {
        if(!inventory.IsFull() && equipmentPanel.RemoveItem(item)) {
            item.Unequip(PlayerHandler.i.playerStats);
            inventory.AddItem(item);
            statPanel.UpdateStatValues();
        }
    }
}
