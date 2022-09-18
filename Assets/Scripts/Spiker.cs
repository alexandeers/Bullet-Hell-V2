using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spiker : Enemy, IDamageable
{
    [Header("Spiker Stats")]
    [SerializeField] float attackRange;
    [SerializeField] float dashIntensity;
    [SerializeField] float dashChance;
    [SerializeField] float cooldownDuration;
    float cooldown;
    SpikerState state = SpikerState.Chase;
    [SerializeField] LayerMask targetLayer;
    private float hitTimer;

    enum SpikerState {
        Chase,
        Dash
    }

    public override void HandleBehaviour()
    {
        switch (state)
        {
            case SpikerState.Chase:
                Chase();
                break;
            case SpikerState.Dash:
                Dash();
                break;
        }
    }

    void Chase()
    {
        if(isDead) return;

        if(cooldown > 0)
            cooldown -= Time.deltaTime;

        if(hitTimer > 0)
            hitTimer -= Time.deltaTime;

        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = Vector2.zero;
        float rotateAmount = 0f;
        direction = rb.position - playerPos;
        direction.Normalize();

        if(Vector2.Distance(transform.position, playerPos) < attackRange ) {
            rotateAmount = Vector2.Dot(-direction, transform.up);

            if(UnityEngine.Random.Range(0f, dashChance) <= 1f && cooldown <= 0) {
                state = SpikerState.Dash;
            }
        } else {
            rotateAmount = Vector3.Cross(direction, transform.up).z;
        }

        rb.angularVelocity = angleSpeed * rotateAmount;
        rb.velocity += (Vector2)transform.up * moveSpeed * Time.deltaTime;
    }

    void Dash()
    {
        rb.velocity = Vector2.zero;
        cooldown = cooldownDuration;
        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = playerPos - rb.position;
        direction.Normalize();
        rb.AddForce(direction * dashIntensity, ForceMode2D.Impulse);

        state = SpikerState.Chase;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(hitTimer > 0) return;

        if(((1<<other.gameObject.layer) & targetLayer) != 0) {
            other.collider.GetComponent<IDamageable>().AbsorbDamage(damage, knockback, rb.velocity.normalized);
            Debug.Log("TRÄFFA");
            hitTimer = 0.8f;
        }
    }
}
