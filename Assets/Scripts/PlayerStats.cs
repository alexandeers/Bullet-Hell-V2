using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(0f, 1f)] public float health;
    [Range(30f, 1600f)] public int maxHealth;

    [Range(0f, 1f)] public float mana;
    [Range(30f, 1000f)] public int maxMana;

    [Range(0f, 98f)] public int level;

    [SerializeField] Controls controls;
    GUIHandler guiHandler;

    void Start() {
        GetReferences();
        guiHandler.RefreshUI();
    }

    void OnValidate() {
        GetReferences();
        health *= maxHealth;
        mana *= maxMana;
        
        health = Mathf.Clamp(health, 0, maxHealth);
        mana = Mathf.Clamp(mana, 0, maxMana);
        guiHandler.RefreshUI();
    }

    void Update() {
        int inverse = Input.GetKey(controls.debug_inverseEffect) ? -1 : 1;

        if(Input.GetKeyDown(controls.debug_increaseHealth)) {
            health += 50 * inverse;
        }
        if(Input.GetKeyDown(controls.debug_increaseStamina)) {
            mana += 50 * inverse;
        }
        if(Input.GetKeyDown(controls.debug_increaseHealthMax)) {
            maxHealth += 50 * inverse;
        }
        if(Input.GetKeyDown(controls.debug_increaseStaminaMax)) {
            maxMana += 50 * inverse;
        }
        guiHandler.RefreshUI();
    }

    private void GetReferences(){
        guiHandler = GetComponent<GUIHandler>();
    }
}
