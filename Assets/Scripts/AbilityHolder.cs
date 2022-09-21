using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    [SerializeField] AbilitySlot[] abilities;
    [SerializeField] GameObject abilitySlotPrefab;
    [SerializeField] Transform abilitySlotsParent;

    void Start() {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        foreach(AbilitySlot ability in abilities) {
            Transform instantiatedPrefab = Instantiate(abilitySlotPrefab).GetComponent<Transform>();
            instantiatedPrefab.SetParent(abilitySlotsParent);
            instantiatedPrefab.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            ability.SetAbilitySlotPrefab(instantiatedPrefab);
        }
    }
}
