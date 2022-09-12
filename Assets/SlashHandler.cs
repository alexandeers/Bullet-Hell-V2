using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHandler : MonoBehaviour
{

    [SerializeField] Transform lookPivot;
    Transform pivot;
    float attackCycle = 1f;


    void Start() {
        pivot = lookPivot.GetChild(0);
    }

    void Update() {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        lookPivot.eulerAngles = new Vector3(lookPivot.eulerAngles.x, lookPivot.eulerAngles.y, Mathf.Lerp(lookPivot.eulerAngles.z, rotation.eulerAngles.z, Time.deltaTime * 0.5f)); 

        if(Input.GetKeyDown(KeyCode.Mouse0)) Attack();

        var currentRotation = pivot.localRotation.eulerAngles;
        var rotationCycle = attackCycle == 1f ? 1f : 0f;
        currentRotation.z = Mathf.Lerp(currentRotation.z, 90f - 90f * attackCycle, Time.deltaTime * 30f);
        pivot.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }

    private void Attack() => attackCycle *= -1;
}
