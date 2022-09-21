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

    //Experience
    public int level = 0;
    public int experience, experienceNeededToLevel;
    public int skillPoints;

    public float additionMultiplier, powerMultiplier, divisonMultiplier;

    public bool isShieldEnabled = true;

    [SerializeField] Controls controls;
    UIBarsHandler guiHandler;

    public event Action<float> onDamageInflicted;

    void Start() {
        GetReferences();
        LevelUp();

        shield = 0;
    }

    void OnValidate() {
        GetReferences();
        health *= maxHealth.Value;
        shield *= maxShield.Value;
        
        health = Mathf.Clamp(health, 0f, maxHealth.Value);
        shield = Mathf.Clamp(shield, 0f, maxShield.Value);
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

    void LateUpdate() {
        health = Mathf.Clamp(health, 0f, maxHealth.Value);
        shield = Mathf.Clamp(shield, 0f, maxShield.Value); 
    }

    public bool AbsorbDamage(int damage, float knockback, Vector2 sourceDirection) {
        if(shield - damage >= 0) {
            shield -= damage;
            guiHandler.OnDamaged(false, true);
        } else {
            var remainingDamage = damage - shield;
            shield = 0f;
            health -= remainingDamage;
            guiHandler.OnDamaged(true, true);
        }

        Knockback(knockback, sourceDirection);

        return true;
    }

    public void Knockback(float intensity, Vector2 source) => GetComponent<Rigidbody2D>().AddForce(source * intensity, ForceMode2D.Impulse);

    public void GainExperienceFlat(int xpGained) => experience += xpGained;

    void LevelUp() {
        level++;
        experience = experience -= experienceNeededToLevel;
        IncreaseMaxHealth();
        IncreaseMaxMana();
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

    void IncreaseMaxMana() {
        // maxShield += (int)(((float)maxShield * 0.01f) * ((100 - level) * 0.065f));
        float fraction = shield / maxShield.Value;
        shield = maxShield.Value * fraction;
    }

    public void OnDamage(float amount) {
        if(shield >= maxShield.Value) {
            health += amount * 0.1f;
        } else {
            shield += amount * 0.5f;
        }

        onDamageInflicted?.Invoke(amount);
    }

    private void DebugControls()
    {
        int inverse = Input.GetKey(controls.debug_inverseEffect) ? -1 : 1;

        if (Input.GetKeyDown(controls.debug_increaseHealth))
        {
            health += 50 * inverse;
            guiHandler.OnDamaged(true, false);
        }
        if (Input.GetKeyDown(controls.debug_increaseStamina))
        {
            shield += 50 * inverse;
            guiHandler.OnDamaged(false, true);
        }
        if (Input.GetKeyDown(controls.debug_increaseHealthMax))
        {
            maxHealth.AddModifier(new StatModifier(50*inverse, StatModType.Flat, this));
        }
        if (Input.GetKeyDown(controls.debug_increaseStaminaMax))
        {
            maxShield.BaseValue += 50 * inverse;
        }

        if(Input.GetKeyDown(KeyCode.G)) {
            maxHealth.AddModifier(new StatModifier(0.5f, StatModType.PercentAdd, this));
        }
    }

    private void GetReferences(){
        guiHandler = GetComponent<UIBarsHandler>();
    }
}
