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
    [Range(0f, 1f)] public float mana;
    [Range(30f, 1000f)] public int maxMana;

    //Experience
    public int level = 0;
    public int experience, experienceNeededToLevel;
    public int skillPoints;

    public float additionMultiplier, powerMultiplier, divisonMultiplier;


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
        mana *= maxMana;
        
        health = Mathf.Clamp(health, 0, maxHealth.Value);
        mana = Mathf.Clamp(mana, 0, maxMana);
        guiHandler.RefreshUIComponents();
    }

    void Update()
    {
        DebugControls();
        if(Input.GetKeyDown(KeyCode.Space)) {
            experience = experienceNeededToLevel;
        }

        if(experience >= experienceNeededToLevel) LevelUp();
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
        maxHealth.AddModifier( new StatModifier((int)(((float)maxHealth.Value * 0.01f) * ((200 - level) * 0.03f)), StatModType.Flat, this) );
        health = maxHealth.Value;
    }

    void IncreaseMana() {
        maxMana += (int)(((float)maxMana * 0.01f) * ((100 - level) * 0.065f));
        mana = maxMana;
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
            mana += 50 * inverse;
            guiHandler.OnDamaged(false);
        }
        if (Input.GetKeyDown(controls.debug_increaseHealthMax))
        {
            maxHealth.AddModifier(new StatModifier(50*inverse, StatModType.Flat, this));
            guiHandler.RefreshUIComponents();
        }
        if (Input.GetKeyDown(controls.debug_increaseStaminaMax))
        {
            maxMana += 50 * inverse;
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
