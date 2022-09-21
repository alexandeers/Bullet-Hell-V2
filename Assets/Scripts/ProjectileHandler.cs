using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{

    [SerializeField] Projectile projectile;
    [SerializeField] LayerMask enemyLayer;

    Transform target;
    Rigidbody2D rb;
    new CircleCollider2D collider;

    TrailRenderer trailRenderer;
    ParticleSystem hitParticles;
    

    float speed, damage, knockback;
    bool projectileIsSet = false;
    int pierceCounter = 0;
    float charge;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        trailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
        hitParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        trailRenderer.emitting = false;
    }

    void Update() {
        if(!projectileIsSet) return;
        StartCoroutine("Lifetime");
    }

    void FixedUpdate() {
        if(!projectileIsSet) {
            rb.velocity = Vector2.zero;
            return;
        }

        HandleBehaviour();
    }

    private void HandleBehaviour() {
        if(projectile.isHoming) {
            GetTarget();
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize ();
            float rotateAmount = Vector3.Cross (direction, transform.up).z;
            rb.angularVelocity = -projectile.angleSpeed * rotateAmount;
            rb.velocity = transform.up * projectile.speed;
        } else {
            rb.velocity = transform.up * speed;
            trailRenderer.emitting = projectileIsSet;
        }
    }

    IEnumerator Lifetime() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void GetTarget()
    {
        if(target) return;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 50f, enemyLayer);

        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach(Collider2D possibleTarget in enemies)
        {
            float dist = Vector3.Distance(possibleTarget.transform.position, currentPos);
            if (dist < minDistance)
            {
                target = possibleTarget.transform;
                minDistance = dist;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!projectileIsSet) return;

        if(((1<<other.gameObject.layer) & enemyLayer) != 0)
        {
            var didDie = false;
            if(!other.GetComponent<IDamageable>().AbsorbDamage((int)(damage + (damage * pierceCounter * 0.5f)), knockback, rb.velocity.normalized) ) {
                projectileIsSet = false;
                transform.SetParent(other.transform, true);
                hitParticles.Play();
                IncreaseParticleSize();
            } else {
                PlayerHandler.i.playerStats.RegenerateShield(damage);
                didDie = true;
            }
            
            DamagePopup.Create(transform.position, (int)(damage + (damage * pierceCounter * 0.5f)), charge);
            pierceCounter++;
            CameraShake.i.Shake(1f + 2.5f * charge, 0.4f + charge/3f, didDie && charge >= 0.8f);
        } else {
            IncreaseParticleSize();
            projectileIsSet = false;
            hitParticles.Play();
        }
    }

    void IncreaseParticleSize() {
        var emission = hitParticles.emission;
        var main = hitParticles.main;
        emission.rateOverTime = hitParticles.emission.rateOverTime.constant + (hitParticles.emission.rateOverTime.constant * charge);
        main.startSize = main.startSize.constant + (hitParticles.main.startSize.constant * (charge * 2f));
    }

    public void SetProjectile(Projectile _projectile, float _charge, float _damage) {
        GetComponent<SpriteRenderer>().color = Color.white;
        projectile = _projectile;
        speed = projectile.speed + (projectile.speed * _charge * 2f);
        damage = (projectile.damage + _damage) + ((projectile.damage + damage) * _charge * 1.5f);
        knockback = projectile.knockback + (projectile.knockback * _charge * 3f);
        charge = _charge;
        

        projectileIsSet = true;
    } 
}
