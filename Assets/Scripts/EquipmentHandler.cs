using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ItemSlot weaponSlot;
    [SerializeField] Transform equipPivot;
    [SerializeField] InventoryManager inventoryManager;
    Camera cam;
    GameObject weaponPrefab;
    [Header("Variables")]
    [SerializeField] float rotationSpeed;


    void Start() {
        cam = Camera.main;

        inventoryManager.onWeaponEquip += OnWeaponEquip;
        inventoryManager.onWeaponUnequip += OnWeaponUnequip;
    }

    void OnWeaponEquip(WeaponItem weapon)
    {
        if(weaponPrefab) return;
        weaponPrefab = Instantiate(weapon.weaponPrefab);
        weaponPrefab.transform.SetParent(equipPivot.GetChild(0), false);
        weaponPrefab.transform.localScale = Vector3.one * 0.75f;
    }

    void OnWeaponUnequip() {
        Destroy(weaponPrefab);
        weaponPrefab = null;
    }

    void Update() {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        equipPivot.rotation = Quaternion.Lerp(equipPivot.rotation, rotation, Time.deltaTime * rotationSpeed);
        
        if(weaponPrefab)
            weaponPrefab.GetComponent<IUseable>().Use();
    }
}
