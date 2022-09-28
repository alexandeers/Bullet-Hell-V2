using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Handles leveling and player stats, scalable system.
public class PlayerStats : MonoBehaviour, IDamageable
{
    //Core stats
    [Range(0f, 1f)] public float health;
    public CharacterStat maxHealth;
    [Range(1f, 1f)] public float shield;
    public CharacterStat maxShield;

    public CharacterStat damage;
    public CharacterStat chargeRate;
    public CharacterStat leech;

    public CharacterStat dashCharges;

    //Experience
    public int level = 0;
    public int experience, experienceNeededToLevel;
    public int skillPoints;

    public float additionMultiplier, powerMultiplier, divisonMultiplier;

    public bool isShieldEnabled = true;

    [SerializeField] Controls controls;
    UIBarsHandler guiHandler;
    [SerializeField] int selfDamageAmount;

    public event Action<float> onDamageInflicted;
    public event Action<float, bool> triggerFlash;

    void Start() {
        GetReferences();
        LevelUp();
        shield = 0;
    }

    void OnValidate() {
        GetReferences();
        health *= maxHealth.Value;
        shield *= maxShield.Value;

        isShieldEnabled = maxShield.Value != 0;
        
        health = Mathf.Clamp(health, 0f, maxHealth.Value);
        shield = Mathf.Clamp(shield, 0f, maxShield.Value);
        guiHandler.RefreshUIComponents();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) experience = experienceNeededToLevel;
        if(Input.GetKeyDown(KeyCode.F)) AbsorbDamage(selfDamageAmount);

        isShieldEnabled = maxShield.Value != 0;

        if(experience >= experienceNeededToLevel) LevelUp();

        guiHandler.RefreshUIComponents();
    }

    void LateUpdate() {
        health = Mathf.Clamp(health, 0f, maxHealth.Value);
        shield = Mathf.Clamp(shield, 0f, maxShield.Value); 
    }

    public bool AbsorbDamage(int damage, float knockback = 0f, Vector2 sourceDirection = new Vector2()) {
        if(shield - damage >= 0) {
            shield -= damage;
            guiHandler.OnDamaged(false, true);
            triggerFlash?.Invoke(damage, true);
        } else {
            var remainingDamage = damage - shield;
            shield = 0f;
            health -= remainingDamage;
            guiHandler.OnDamaged(true, true);
            triggerFlash?.Invoke(damage, false);
        }

        if(knockback != 0f)
            Knockback(knockback, sourceDirection);
        
        return true;
    }

    public void Knockback(float intensity, Vector2 source) => GetComponent<Rigidbody2D>().AddForce(source * intensity, ForceMode2D.Impulse);

    public void GainExperienceFlat(int xpGained) => experience += xpGained;

    void LevelUp() {
        level++;
        experience = experience -= experienceNeededToLevel;
        IncreaseMaxHealth();
        experienceNeededToLevel = CalculateRequiredXP();
    }

    int CalculateRequiredXP() {
        int solveForRequiredXP = 0;
        for(int levelCycle = 1; levelCycle <= level; levelCycle++) {
            solveForRequiredXP += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisonMultiplier));
        }

        return solveForRequiredXP / 4;
    }

    void IncreaseMaxHealth() {
        // maxHealth.AddModifier( new StatModifier((int)(((float)maxHealth.BaseValue * 0.01f) * ((200 - level) * 0.01f) + 5f), StatModType.Flat, this) );
        float fraction = health / maxHealth.Value;
        maxHealth.BaseValue += (int)(maxHealth.BaseValue * 0.01f) * ((200 - level) * 0.01f) + 5f;
        health = maxHealth.Value * fraction;
    }

    public void Leech(float amount) {
        if(shield >= maxShield.Value || !isShieldEnabled) {
            health += amount * (leech.Value / 300f);
        } else {
            shield += amount * (leech.Value / 100f);
        }

        onDamageInflicted?.Invoke(amount);
        triggerFlash?.Invoke(0f, false);
    }



    private void GetReferences(){
        guiHandler = GetComponent<UIBarsHandler>();
    }

    public float GetHealthNormalized() {
        return health / maxHealth.Value;
    }
}
