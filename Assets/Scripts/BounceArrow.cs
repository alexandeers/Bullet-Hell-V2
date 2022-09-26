using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceArrow : Projectile
{
    [SerializeField] protected int bounces;
    int startBounces;
    Vector2 direction;
    Vector2 lastVelocity;   

    public override void Start() {
        base.Start();
        startBounces = bounces;
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        lastVelocity = rb.velocity;
    }

    public override void HandleBehaviour()
    {
        if(bounces == startBounces) 
        {
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = direction * speed;
            transform.eulerAngles = direction;
        } 

        trailRenderer.emitting = projectileIsSet;
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        if(!projectileIsSet) return;

        if(((1<<other.gameObject.layer) & enemyLayer) != 0)
        {
            var didDie = false;
            if(!other.gameObject.GetComponent<IDamageable>().AbsorbDamage((int)(damage + (damage * pierceDeathCounter * 0.5f)), knockback, rb.velocity.normalized) ) {
                projectileIsSet = false;
                transform.SetParent(other.transform, true);
                hitParticles.Play();
                IncreaseParticleSize();
            } else {
                PlayerHandler.i.playerStats.Leech(damage);
                didDie = true;
            }
            DamagePopup.Create(transform.position, (int)(damage + (damage * pierceDeathCounter * 0.5f)), charge);
            pierceDeathCounter++;
            CameraShake.i.Shake(1f + 2.5f * charge, 0.4f + charge/3f, didDie && charge >= 0.8f);
        } else {
            if(bounces != 0) {
                var contactPoint = other.GetComponent<Collider2D>().ClosestPoint(transform.position);
                direction = Vector2.Reflect(lastVelocity.normalized, -contactPoint.normalized);

                bounces--;
            } else {
                IncreaseParticleSize();
                projectileIsSet = false;
                hitParticles.Play();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {

    }
}