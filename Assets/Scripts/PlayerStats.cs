using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Core stats
    [Range(0f, 1f)] public float health;
    [Range(30f, 1600f)] public int maxHealth;
    [Range(0f, 1f)] public float mana;
    [Range(30f, 1000f)] public int maxMana;

    //Experience
    [Range(1f, 99f)] public int level;
    public int experience, experienceNeededToLevel;
    public int skillPoints;
    [SerializeField] Controls controls;
    GUIHandler guiHandler;

    void Start() {
        GetReferences();
        guiHandler.RefreshUIComponents();
    }

    void OnValidate() {
        GetReferences();
        health *= maxHealth;
        mana *= maxMana;
        
        health = Mathf.Clamp(health, 0, maxHealth);
        mana = Mathf.Clamp(mana, 0, maxMana);
        guiHandler.RefreshUIComponents();
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
        guiHandler.RefreshUIComponents();
    }

    private void GetReferences(){
        guiHandler = GetComponent<GUIHandler>();
    }
}
