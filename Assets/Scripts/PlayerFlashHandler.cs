using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashHandler : MonoBehaviour
{
    [SerializeField] Material playerMaterial;
    float timer, timerDuration = 1.25f;
    bool isTimerOn;

    void Start() {
        PlayerHandler.i.playerStats.triggerFlash += HandleFlash;
    }

    void Update() {
        if(!isTimerOn) return;

        if(PlayerHandler.i.playerStats.shield > 0)
            playerMaterial.SetFloat("_FlashAmount", timer / timerDuration);
        else
            playerMaterial.SetFloat("_FlashAmount", timer / timerDuration);
        timer -= Time.deltaTime;

        if(timer < 0f) {
            isTimerOn = false;
            timer = 0;
        }
    }

    void HandleFlash(float damage, bool isShieldEnabled) 
    {
        if(damage == 0) return;
        
        isTimerOn = true;
        timer = timerDuration;

        if(isShieldEnabled)
            playerMaterial.SetInteger("_isShielded", 1);
        else 
            playerMaterial.SetInteger("_isShielded", 0);
    }

}
