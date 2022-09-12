using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AbilityState {
    Ready,
    Active,
    Cooldown
}

public class AbilitySlot : MonoBehaviour
{

    [SerializeField] Ability ability;
    float cooldownTime, activeTime;
    AbilityState state;

    [SerializeField] bool isAcquired;
    [HideInInspector] Transform abilitySlotPrefab;
    RectTransform cooldownBar;

    public void SetAbilitySlotPrefab(Transform prefab) {
        abilitySlotPrefab = prefab;
        cooldownBar = abilitySlotPrefab.GetChild(0).GetComponent<RectTransform>();
    }

    void Update() {
        if(!ability) return;

        switch (state) {
            case AbilityState.Ready:
                if(Input.GetKeyDown(ability.key)) {
                    ability.Activate(transform.parent.parent.gameObject);
                    state = AbilityState.Active;
                    activeTime = ability.activeTime;
                }
                break;

            case AbilityState.Active:
                if(activeTime > 0) {
                    activeTime -= Time.deltaTime;
                } else {
                    cooldownTime = ability.cooldownTime;
                    state = AbilityState.Cooldown;
                }
                break;

            case AbilityState.Cooldown:
                if(cooldownTime > 0) {
                    cooldownTime -= Time.deltaTime;
                } else {
                    state = AbilityState.Ready;
                }
                break;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        if(state == AbilityState.Cooldown) {
            cooldownBar.sizeDelta = new Vector2(( (ability.cooldownTime - cooldownTime) / ability.cooldownTime) * 35f, cooldownBar.sizeDelta.y);   
        } else if (state == AbilityState.Active) {
            if(activeTime > 0.5f)
                cooldownBar.sizeDelta = new Vector2((activeTime / ability.activeTime) * 35f, cooldownBar.sizeDelta.y);  
            else
                cooldownBar.sizeDelta = new Vector2(0f, cooldownBar.sizeDelta.y);   
        } else {
            cooldownBar.sizeDelta = new Vector2(35f, cooldownBar.sizeDelta.y); 
        }
    }
}
