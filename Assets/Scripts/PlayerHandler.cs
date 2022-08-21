using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;
    PlayerMovement playerMovement;
    PlayerStats playerStats;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();

        if(instance == null) {
            instance = this;
        } else {
            Debug.Log("PlayerHandler already exists.");
        }
    }
    
    void FixedUpdate() {
        playerMovement.UpdateMovement();
    }

    public void TakeDamage(int damage) {
        playerStats.health -= damage;
    }

}
