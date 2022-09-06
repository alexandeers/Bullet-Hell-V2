using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    [Header("References")]
    public List<Equipment> equippedItems = new List<Equipment>();
    public Transform equipPivot;
    private Camera cam;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        
    }
}
