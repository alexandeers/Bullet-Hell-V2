using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{

    [SerializeField] Projectile projectile;
    [SerializeField] LayerMask enemyLayer;

    Transform target;
    new Rigidbody2D rigidbody;
    new CircleCollider2D collider;

    TrailRenderer trailRenderer;
    ParticleSystem hitParticles;
    

    float speed, damage, knockback;
    bool projectileIsSet = false;
    int pierceCounter = 1;
    float charge;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
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
            rigidbody.velocity = Vector2.zero;
            return;
        }

        HandleBehaviour();
    }

    private void HandleBehaviour() {
        if(projectile.isHoming) {
            GetTarget();
            Vector2 direction = (Vector2)target.position - rigidbody.position;
            direction.Normalize ();
            float rotateAmount = Vector3.Cross (direction, transform.up).z;
            rigidbody.angularVelocity = -projectile.angleSpeed * rotateAmount;
            rigidbody.velocity = transform.up * projectile.speed;
        } else {
            rigidbody.velocity = transform.up * speed;
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
            if( !other.GetComponent<IDamageable>().AbsorbDamage((int)damage * pierceCounter, knockback) ) {
                projectileIsSet = false;
                transform.SetParent(other.transform, true);
                hitParticles.Play();
                IncreaseParticleSize();
            }
            
            pierceCounter++;
            PlayerHandler.i.playerStats.RegenerateShieldOnDamage(damage);
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

    public void SetProjectile(Projectile _projectile, float _charge) {
        GetComponent<SpriteRenderer>().color = Color.white;
        projectile = _projectile;
        speed = projectile.speed + (projectile.speed * _charge * 2f);
        damage = projectile.damage + (projectile.damage * _charge * 1.5f);
        knockback = projectile.knockback + (projectile.knockback * _charge * 3f);
        charge = _charge;
        

        projectileIsSet = true;
    } 
}
