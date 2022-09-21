using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBarsHandler : MonoBehaviour
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
    Image xpBarImage;

    PlayerStats playerStats;
    PlayerHandler playerHandler;

    public delegate void OnGUIEvent();
    public event OnGUIEvent OnGUIUpdate;

    void Start() {
        GetReferences();
        OnGUIUpdate += RefreshUIComponents;
    }

    void OnValidate() {
        GetReferences();
    }

    public void RefreshUIComponents() {
        // var hpBarSizeSolver = Mathf.Log10(playerStats.maxHealth.Value/3) * (0.2f * playerStats.maxHealth.Value) + 90f;
        // var hpBarSizeSolver = 800f*(1-Mathf.Exp(-0.001f*playerStats.maxHealth.Value)) + 120f;
        // var manaBarSizeSolver = Mathf.Log10(playerStats.maxMana/3) * (0.2f * playerStats.maxMana) + 70f;
        var hpBarSizeSolver = (playerStats.health * 0.5f);
        var manaBarSizeSolver = (playerStats.maxShield.Value * 0.2f) + 50f;
        var totalHealthMana = manaBarSizeSolver + (playerStats.maxHealth.Value * 0.5f);
        var healthNormalized = playerStats.health / playerStats.maxHealth.Value;
        var manaNormalized = playerStats.shield / playerStats.maxShield.Value;

        if(playerStats.isShieldEnabled) {
            manaBackground.gameObject.SetActive(true);
            container.anchoredPosition = new Vector2(((-totalHealthMana) / 2f) - 5f, container.anchoredPosition.y);
        } else {
            manaBackground.gameObject.SetActive(false);
            container.anchoredPosition = new Vector2(-(playerStats.maxHealth.Value * 0.5f) / 2f, container.anchoredPosition.y);
        } 
        

        hpBackground.sizeDelta = new Vector2(Mathf.Lerp(hpBackground.sizeDelta.x, hpBarSizeSolver, Time.deltaTime * 3f), hpBackground.sizeDelta.y);
        hpBar.sizeDelta = new Vector2(hpBarSizeSolver, hpBar.sizeDelta.y);

        manaBackground.sizeDelta = new Vector2(manaBarSizeSolver, manaBackground.sizeDelta.y);
        manaBar.sizeDelta = new Vector2(manaBackground.sizeDelta.x * manaNormalized, manaBar.sizeDelta.y);

        // container.anchoredPosition = new Vector2(-container.GetComponent<HorizontalLayoutGroup>().minWidth / 2f, container.anchoredPosition.y);

        xpBackground.sizeDelta = new Vector2(-container.anchoredPosition.x*2, xpBackground.sizeDelta.y);
        xpBar.sizeDelta = xpBackground.sizeDelta;

        if(playerStats.experienceNeededToLevel != 0) {
            xpBarImage.fillAmount = Mathf.Lerp(xpBarImage.fillAmount, (float)playerStats.experience / (float)playerStats.experienceNeededToLevel, Time.deltaTime * 15f);
        }

        topRowBackground.sizeDelta = new Vector2(-container.anchoredPosition.x*2, topRowBackground.sizeDelta.y);

        hpText.text = $"<size=10>HP </size><b>{Mathf.Round(playerStats.health)}";
        manaText.text = $"<size=10>SH </size><b>{Mathf.Round(playerStats.shield)}";
        xpText.text = $"<alpha=#AA><size=15>XP <alpha=#FF></size><b>{playerStats.experience}<size=15></b><alpha=#AA>/{playerStats.experienceNeededToLevel}";
        levelText.text = $"<size=15><alpha=#AA>LVL <alpha=#FF></size><b>{playerStats.level}";


        
    }

    public void OnDamaged(bool healthDamage, bool shieldDamage) { //Väldigt ooptimerad funktion men jag orkar inte.
        if(healthDamage) {
            RectTransform damagedBar = Instantiate(damagedBarTemplate, transform).GetComponent<RectTransform>();
            var beforeDamageFillAmount = hpBar.sizeDelta.x;
            RefreshUIComponents();
            damagedBar.SetParent(hpBackground);
            damagedBar.localScale = new Vector3(1f, 1f, 1f);
            damagedBar.gameObject.SetActive(true);
            damagedBar.anchoredPosition = new Vector2(hpBar.sizeDelta.x, 0);
            damagedBar.sizeDelta = new Vector2(beforeDamageFillAmount - hpBar.sizeDelta.x, damagedBar.sizeDelta.y);
        }

        if(shieldDamage) {
            RectTransform damagedBar = Instantiate(damagedBarTemplate, transform).GetComponent<RectTransform>();
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

    private void GetReferences()
    {
        playerStats = GetComponent<PlayerStats>();
        playerHandler = GetComponent<PlayerHandler>();
        xpBarImage = xpBar.GetComponent<Image>();

    }
}
