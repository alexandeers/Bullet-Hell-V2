using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Handles leveling and player stats, scalable system.
public class PlayerStats : MonoBehaviour
{
    //Core stats
    [Range(0f, 1f)] public float health;
    public CharacterStat maxHealth;
    [Range(1f, 1f)] public float shield;
    [Range(1f, 1000f)] public int maxShield;

    //Experience
    public int level = 0;
    public int experience, experienceNeededToLevel;
    public int skillPoints;

    public float additionMultiplier, powerMultiplier, divisonMultiplier;

    public bool isShieldEnabled = true;

    [SerializeField] Controls controls;
    GUIHandler guiHandler;

    void Start() {
        GetReferences();
        guiHandler.RefreshUIComponents();
        LevelUp();
    }

    void OnValidate() {
        GetReferences();
        health *= maxHealth.Value;
        shield *= maxShield;
        
        health = Mathf.Clamp(health, 0, maxHealth.Value);
        shield = Mathf.Clamp(shield, 0, maxShield);
        guiHandler.RefreshUIComponents();
    }

    void Update()
    {
        DebugControls();
        if(Input.GetKeyDown(KeyCode.Space)) {
            experience = experienceNeededToLevel;
        }

        if(experience >= experienceNeededToLevel) LevelUp();

        guiHandler.RefreshUIComponents();
    }

    public void GainExperienceFlat(int xpGained) => experience += xpGained;

    void LevelUp() {
        level++;
        experience = experience -= experienceNeededToLevel;
        IncreaseHealth();
        IncreaseMana();
        experienceNeededToLevel = CalculateRequiredXP();

        guiHandler.RefreshUIComponents();
    }

    int CalculateRequiredXP() {
        int solveForRequiredXP = 0;
        for(int levelCycle = 1; levelCycle <= level; levelCycle++) {
            solveForRequiredXP += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisonMultiplier));
        }

        return solveForRequiredXP / 4;
    }

    void IncreaseHealth() {
        // maxHealth.AddModifier( new StatModifier((int)(((float)maxHealth.BaseValue * 0.01f) * ((200 - level) * 0.01f) + 5f), StatModType.Flat, this) );
        maxHealth.BaseValue += (int)(maxHealth.BaseValue * 0.01f) * ((200 - level) * 0.01f) + 5f;
        health = maxHealth.Value;
    }

    void IncreaseMana() {
        // maxShield += (int)(((float)maxShield * 0.01f) * ((100 - level) * 0.065f));
        shield = maxShield;
    }

    private void DebugControls()
    {
        int inverse = Input.GetKey(controls.debug_inverseEffect) ? -1 : 1;

        if (Input.GetKeyDown(controls.debug_increaseHealth))
        {
            health += 50 * inverse;
            guiHandler.OnDamaged(true);
        }
        if (Input.GetKeyDown(controls.debug_increaseStamina))
        {
            shield += 50 * inverse;
            guiHandler.OnDamaged(false);
        }
        if (Input.GetKeyDown(controls.debug_increaseHealthMax))
        {
            maxHealth.AddModifier(new StatModifier(50*inverse, StatModType.Flat, this));
            guiHandler.RefreshUIComponents();
        }
        if (Input.GetKeyDown(controls.debug_increaseStaminaMax))
        {
            maxShield += 50 * inverse;
            guiHandler.RefreshUIComponents();
        }

        if(Input.GetKeyDown(KeyCode.G)) {
            maxHealth.AddModifier(new StatModifier(0.5f, StatModType.PercentAdd, this));
            guiHandler.RefreshUIComponents();
        }
    }

    private void GetReferences(){
        guiHandler = GetComponent<GUIHandler>();
    }
}
