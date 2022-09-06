using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour
{

    [SerializeField] CutoutMask hpBar;
    [SerializeField] CutoutMask hpBackground;
    [SerializeField] CutoutMask hpSegments;
    [SerializeField] CutoutMask staminabar;
    [SerializeField] CutoutMask staminabarMax;
    [SerializeField] CutoutMask staminabarSegments;

    [SerializeField] Image healthBg;
    [SerializeField] Image healthBar;
    [SerializeField] Image manaBg;
    [SerializeField] Image manaBar;

    [SerializeField] Image xpCounter;
    [SerializeField] UnityEngine.U2D.Animation.SpriteLibraryAsset spriteLibrary;

    PlayerStats playerStats;
    PlayerHandler playerHandler;
    float health, maxHealth, mana, maxMana;

    public delegate void OnGUIEvent();
    public event OnGUIEvent OnGUIUpdate;

    void Start() {
        GetReferences();
    }

    public void RefreshUIComponents() {
        GetReferences();
        healthBg.rectTransform.sizeDelta = new Vector2(maxHealth, healthBg.rectTransform.sizeDelta.y);
        healthBar.rectTransform.sizeDelta = new Vector2((health/maxHealth)*healthBg.rectTransform.sizeDelta.x-13f, healthBar.rectTransform.sizeDelta.y);
        manaBg.rectTransform.sizeDelta = new Vector2(maxMana, manaBg.rectTransform.sizeDelta.y);
        manaBar.rectTransform.sizeDelta = new Vector2((mana/maxMana)*manaBg.rectTransform.sizeDelta.x-13f, manaBar.rectTransform.sizeDelta.y);

        HandleXPCounter();

    }

    public void RefreshUITwo() {
        GetReferences();
        hpBackground.rectTransform.sizeDelta = new Vector2(maxHealth, 20);
        hpBar.rectTransform.sizeDelta = new Vector2(hpBackground.rectTransform.sizeDelta.x - 4, 15); 
        hpBar.fillAmount = health / maxHealth;
        hpSegments.rectTransform.sizeDelta = new Vector2(hpBar.rectTransform.sizeDelta.x * hpBar.fillAmount, 15);

        staminabarMax.rectTransform.sizeDelta = new Vector2(maxMana, 20);
        staminabar.rectTransform.sizeDelta = new Vector2(staminabarMax.rectTransform.sizeDelta.x - 4, 15);
        staminabar.fillAmount = mana / maxMana;
        staminabarSegments.rectTransform.sizeDelta = new Vector2(staminabar.rectTransform.sizeDelta.x * staminabar.fillAmount, 15);

    }

    private void HandleXPCounter() {
        Sprite sprite = spriteLibrary.GetSprite("XPCounter", $"XPCounter with Levelse_{playerStats.level}");
        xpCounter.sprite = sprite;
    }

    private void GetReferences()
    {
        playerStats = GetComponent<PlayerStats>();
        playerHandler = GetComponent<PlayerHandler>();
        health = playerStats.health;
        maxHealth = playerStats.maxHealth;
        mana = playerStats.mana;
        maxMana = playerStats.maxMana;
    }
}
