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
    float attackCooldown;
    SpikerState state = SpikerState.Chase;
    [SerializeField] LayerMask targetLayer;
    private float onHitDealDamageCooldown;

    private float anticipationDelay;

    private enum SpikerState {
        Chase,
        Dash,
        AnticipateDash
    }

    public override void HandleBehaviour()
    {
        if(isDead) return;

        switch (state)
        {
            case SpikerState.Chase:
                Chase();
                break;
            case SpikerState.Dash:
                Dash();
                break;
            case SpikerState.AnticipateDash:
                AnticipateDash();
                break;
        }
    }

    void Chase()
    {
        if(attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        if(onHitDealDamageCooldown > 0)
            onHitDealDamageCooldown -= Time.deltaTime;

        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = Vector2.zero;
        float rotateAmount = 0f;
        direction = rb.position - playerPos;
        direction.Normalize();

        if(Vector2.Distance(transform.position, playerPos) < attackRange ) {
            rotateAmount = Vector2.Dot(-direction, transform.up);

            // if(UnityEngine.Random.Range(0f, dashChance) <= 1f && attackCooldown <= 0) {
            //     state = SpikerState.Dash;
            // }

            if(attackCooldown <= 0) {
                anticipationDelay = UnityEngine.Random.Range(0.4f, 0.7f);
                state = SpikerState.AnticipateDash;
            }
        } else {
            rotateAmount = Vector3.Cross(direction, transform.up).z;
        }

        rb.angularVelocity = angleSpeed * rotateAmount;
        rb.velocity += (Vector2)transform.up * moveSpeed * Time.deltaTime;
    }

    void AnticipateDash()
    {
        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = direction = rb.position - playerPos;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = angleSpeed * rotateAmount;
        rb.velocity += (Vector2)transform.up * (moveSpeed/3f) * Time.deltaTime;

        anticipationDelay -= Time.deltaTime;
        foreach(SpriteRenderer sprite in sprites) {
            sprite.color = Color.Lerp(Color.white, Color.red, 1f-(anticipationDelay*2f));
        }

        if(anticipationDelay <= 0) {
            foreach(SpriteRenderer sprite in sprites) {
                sprite.color = Color.white;
            }
            state = SpikerState.Dash;
        }


    }

    void Dash()
    {
        rb.velocity = Vector2.zero;
        attackCooldown = cooldownDuration;
        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = playerPos - rb.position;
        direction.Normalize();
        rb.AddForce(direction * dashIntensity, ForceMode2D.Impulse);

        state = SpikerState.Chase;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(onHitDealDamageCooldown > 0) return;
        if(isDead) return;

        if(((1<<other.gameObject.layer) & targetLayer) != 0) {
            other.collider.GetComponent<IDamageable>().AbsorbDamage((int)((float)damage * UnityEngine.Random.Range(0.95f, 1.05f)), knockback, rb.velocity.normalized);
            onHitDealDamageCooldown = 0.8f;
        }
    }
}
