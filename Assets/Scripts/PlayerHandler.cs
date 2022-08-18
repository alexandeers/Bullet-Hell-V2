using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    PlayerMovement playerMovement;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void FixedUpdate() {
        playerMovement.UpdateMovement();
    }

}
