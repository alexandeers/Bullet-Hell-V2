using UnityEngine;

public class FullscreenEffectHandler : MonoBehaviour
{

    [SerializeField] Material glitchMaterial;
    [SerializeField] Material edgeMaterial;
    [SerializeField]
    [Range(0f, 1f)] float lowHealthThreshold;
    float glitchStrength, edgeStrength;
    bool doLerpEdge, doLerpColor;
    Color lerpColorTarget, lerpColor;

    PlayerStats playerStats;

    Color healthColor, shieldColor;

    void Start() {
        playerStats = PlayerHandler.i.playerStats;
        playerStats.triggerFlash += HandleOnDamage;

        Color color;
        ColorUtility.TryParseHtmlString("#E2716C", out color);
        healthColor = color;
        
        ColorUtility.TryParseHtmlString("#6CB3E2", out color);
        shieldColor = color;
    }

    void Update() {
        LerpGlitchStrength();
        LerpEdgeStrength();
        LerpColor();
    }


    void HandleOnDamage(float damage, bool shieldRemaining) {

        if(shieldRemaining) 
        {
            if(playerStats.GetHealthNormalized() > lowHealthThreshold) 
            {
                doLerpEdge = true;
                doLerpColor = false;
                
                edgeStrength = 1f;
                edgeMaterial.SetColor("_Color", shieldColor);
                glitchStrength = damage / (playerStats.maxHealth.Value + playerStats.maxShield.Value);
            }
            else 
            {
                doLerpEdge = false;
                doLerpColor = true;

                edgeStrength = 1f - playerStats.GetHealthNormalized() * (1f/lowHealthThreshold);
                lerpColor = shieldColor;
                lerpColorTarget = healthColor;
                edgeMaterial.SetColor("_Color", lerpColor);
                glitchStrength = damage / (playerStats.maxHealth.Value + playerStats.shield);
            }
        } 
        else 
        {
            doLerpEdge = false;
            doLerpColor = false;

            edgeStrength = 1f - playerStats.GetHealthNormalized() * (1f/lowHealthThreshold);
            edgeMaterial.SetColor("_Color", healthColor);
            glitchStrength = damage / (playerStats.maxHealth.Value);
        }
    }

    void LerpColor()
    {
        if(!doLerpColor) return;
        lerpColor = Color.Lerp(lerpColor, lerpColorTarget, 2f * Time.deltaTime);
        edgeMaterial.SetColor("_Color", lerpColor);
    }

    void LerpEdgeStrength()
    {
        if(doLerpEdge)
        {
            if(edgeStrength >= 0.02 && playerStats.shield > 0) {
                edgeStrength = Mathf.Lerp(edgeStrength, 0f, Time.deltaTime * 2f);
            } else if(edgeStrength != 0) {
                edgeStrength = 0f;
            }
        }

        edgeMaterial.SetFloat("_FullscreenIntensity", Mathf.Clamp01(edgeStrength));
    }

    void LerpGlitchStrength() {
        if(glitchStrength >= 0.02) {
            glitchStrength = Mathf.Lerp(glitchStrength, 0f, Time.deltaTime * 10f);
        } else if (glitchStrength != 0){
            glitchStrength = 0f;
        }
        glitchMaterial.SetFloat("_Strength", glitchStrength);
    }

}
