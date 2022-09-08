using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIHandler : MonoBehaviour
{
    [SerializeField] RectTransform hpBackground;
    [SerializeField] RectTransform manaBackground;
    [SerializeField] RectTransform hpBar;
    [SerializeField] RectTransform manaBar;
    [SerializeField] RectTransform xpBar;
    [SerializeField] RectTransform xpBackground;
    [SerializeField] RectTransform container;
    [SerializeField] RectTransform topRowBackground;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI xpText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Transform damagedBarTemplate;

    PlayerStats playerStats;
    PlayerHandler playerHandler;

    public delegate void OnGUIEvent();
    public event OnGUIEvent OnGUIUpdate;

    void Start() {
        GetReferences();
        OnGUIUpdate += RefreshUIComponents;
    }

    public void RefreshUIComponents() {
        GetReferences();

        var hpBarSizeSolver = Mathf.Log10(playerStats.maxHealth/3) * (0.2f * playerStats.maxHealth) + 90f;
        var manaBarSizeSolver = Mathf.Log10(playerStats.maxMana/3) * (0.2f * playerStats.maxMana) + 90f;
        var totalHealthMana = manaBarSizeSolver + hpBarSizeSolver;
        var healthNormalized = playerStats.health / playerStats.maxHealth;
        var manaNormalized = playerStats.mana / playerStats.maxMana;

        container.anchoredPosition = new Vector2(((-totalHealthMana) / 2f) - 5f, container.anchoredPosition.y);

        hpBackground.sizeDelta = new Vector2(hpBarSizeSolver, hpBackground.sizeDelta.y);
        hpBar.sizeDelta = new Vector2(hpBackground.sizeDelta.x * healthNormalized, hpBar.sizeDelta.y);

        manaBackground.sizeDelta = new Vector2(manaBarSizeSolver, manaBackground.sizeDelta.y);
        manaBar.sizeDelta = new Vector2(manaBackground.sizeDelta.x * manaNormalized, manaBar.sizeDelta.y);

        xpBackground.sizeDelta = new Vector2(-container.anchoredPosition.x*2, xpBackground.sizeDelta.y);

        topRowBackground.sizeDelta = new Vector2(-container.anchoredPosition.x*2, topRowBackground.sizeDelta.y);

        hpText.text = $"<size=10>HP </size><b>{Mathf.Round(playerStats.health)}";
        manaText.text = $"<size=10>MP </size><b>{Mathf.Round(playerStats.mana)}";
        xpText.text = $"<alpha=#AA><size=15>XP <alpha=#FF></size><b>{playerStats.experience}<size=15></b><alpha=#AA>/{playerStats.experienceNeededToLevel}";
        levelText.text = $"<size=15><alpha=#AA>LVL <alpha=#FF></size><b>{playerStats.level}";
        
    }

    public void OnDamaged(bool isHealth) { //Väldigt ooptimerad funktion men jag orkar inte.
        RectTransform damagedBar = Instantiate(damagedBarTemplate, transform).GetComponent<RectTransform>();

        if(isHealth) {
            var beforeDamageFillAmount = hpBar.sizeDelta.x;
            RefreshUIComponents();
            damagedBar.SetParent(hpBackground);
            damagedBar.localScale = new Vector3(1f, 1f, 1f);
            damagedBar.gameObject.SetActive(true);
            damagedBar.anchoredPosition = new Vector2(hpBar.sizeDelta.x, 0);
            damagedBar.sizeDelta = new Vector2(beforeDamageFillAmount - hpBar.sizeDelta.x, damagedBar.sizeDelta.y);
        } else {
            var beforeDamageFillAmount = manaBar.sizeDelta.x;
            RefreshUIComponents();
            damagedBar.SetParent(manaBackground);
            damagedBar.GetComponent<Image>().color = Color.white;
            damagedBar.localScale = new Vector3(1f, 1f, 1f);
            damagedBar.gameObject.SetActive(true);
            damagedBar.anchoredPosition = new Vector2(manaBar.sizeDelta.x, 0);
            damagedBar.sizeDelta = new Vector2(beforeDamageFillAmount - manaBar.sizeDelta.x, damagedBar.sizeDelta.y);
        }
    }

    public float GetExperienceFillAmount() => xpBar.sizeDelta.x / xpBackground.sizeDelta.y;

    private void GetReferences()
    {
        playerStats = GetComponent<PlayerStats>();
        playerHandler = GetComponent<PlayerHandler>();
    }
}
