using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // [SerializeField] Projectile projectile;
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Basic Stats")]
    [SerializeField] protected new string name;
    [SerializeField] protected float speed, damage, knockback;
    [SerializeField] protected int pierce;

    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected TrailRenderer trailRenderer;
    [SerializeField] protected ParticleSystem hitParticles;
    
    protected bool projectileIsSet = false;
    protected int pierceDeathCounter = 0;
    protected float charge;

    public virtual void Start() {
        trailRenderer.emitting = false;
    }

    public virtual void Update() {
        if(!projectileIsSet) return;
        StartCoroutine("Lifetime");
    }

    public virtual void FixedUpdate() {
        if(!projectileIsSet) {
            rb.velocity = Vector2.zero;
            return;
        }

        HandleBehaviour();
    }

    public virtual void HandleBehaviour() {
        rb.velocity = transform.up * speed;
        trailRenderer.emitting = projectileIsSet;
    }

    IEnumerator Lifetime() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if(!projectileIsSet) return;

        if(((1<<other.gameObject.layer) & enemyLayer) != 0)
        {
            var damageToInflict = (int)(damage + (damage * pierceDeathCounter * 0.5f));

            var didDie = other.GetComponent<IDamageable>().AbsorbDamage(damageToInflict, knockback, rb.velocity.normalized);

            if(!didDie) {
                if(pierce == 0) {
                    projectileIsSet = false;
                    transform.SetParent(other.transform, true);
                } else { 
                    pierce--; 
                }
                print(pierce);
                hitParticles.Play();
                IncreaseParticleSize();
            } else {
                PlayerHandler.i.playerStats.OnDamage(damage);
            }

            DamagePopup.Create(transform.position, (int)(damage + (damage * pierceDeathCounter * 0.5f)), charge);
            pierceDeathCounter++;
            CameraShake.i.Shake(1f + 2.5f * charge, 0.4f + charge/3f, didDie && charge >= 0.8f);
        } else {
            IncreaseParticleSize();
            projectileIsSet = false;
            hitParticles.Play();
        }
    }

    public virtual void IncreaseParticleSize() {
        var emission = hitParticles.emission;
        var main = hitParticles.main;
        emission.rateOverTime = hitParticles.emission.rateOverTime.constant + (hitParticles.emission.rateOverTime.constant * charge);
        main.startSize = main.startSize.constant + (hitParticles.main.startSize.constant * (charge * 2f));
    }

    public virtual void SetProjectile(float _charge, float _damage) {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = speed + (speed * _charge * 2f);
        damage += (damage + _damage) * _charge;
        knockback = knockback + (knockback * _charge * 3f);
        charge = _charge;

        projectileIsSet = true;
    } 
}
