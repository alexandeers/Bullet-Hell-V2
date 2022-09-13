using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;
    PlayerMovement playerMovement;
    PlayerStats playerStats;
    GUIHandler guiHandler;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        guiHandler = GetComponent<GUIHandler>();

        if(instance == null) {
            instance = this;
        } else {
            Debug.Log("PlayerHandler already exists.");
        }
    }
    
    void FixedUpdate() {
        playerMovement.UpdateMovement();
    }
}
