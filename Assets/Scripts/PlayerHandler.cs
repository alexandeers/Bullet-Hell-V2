using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler i;
    PlayerMovement playerMovement;
    public PlayerStats playerStats;
    GUIHandler guiHandler;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        guiHandler = GetComponent<GUIHandler>();

        if(i == null) {
            i = this;
        } else {
            Debug.Log("PlayerHandler already exists.");
        }
    }
    
    void FixedUpdate() {
        playerMovement.UpdateMovement();
    }

    public Vector2 GetPlayerPosition() => transform.position;

}
