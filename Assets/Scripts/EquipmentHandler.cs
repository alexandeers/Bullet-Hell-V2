using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    [Header("References")]
    public List<Equipment> equippedItems = new List<Equipment>();
    public Transform useable;
    public Transform equipPivot;
    private Camera cam;
    [SerializeField] float rotationSpeed;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        equipPivot.rotation = Quaternion.Lerp(equipPivot.rotation, rotation, Time.deltaTime * rotationSpeed);

        
        useable.GetComponent<IUseable>().Use();

    }
}
