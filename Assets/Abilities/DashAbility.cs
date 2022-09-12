using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{

    [SerializeField] float dashVelocity;

    public override void Activate(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        rb.velocity = movement.movementInput * dashVelocity;
    }
}
