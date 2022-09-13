using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{

    [SerializeField] Projectile projectile;
    Transform target;
    new Rigidbody2D rigidbody;
    new CircleCollider2D collider;
    [SerializeField] LayerMask enemyLayer;
    float projectileSpeed;

    bool projectileIsSet = false;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    void Update() {
        if(!projectileIsSet) return;
        StartCoroutine("Lifetime");
    }

    IEnumerator Lifetime() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void FixedUpdate() {
        if(!projectileIsSet) {
            rigidbody.velocity = Vector2.zero;
            return;
        }

        if(projectile.isHoming) HandleHomingBehaviour();
        else HandleDefaultBehaviour();
    }

    private void HandleDefaultBehaviour() {        
        rigidbody.velocity = transform.up * projectileSpeed;
    }
    private void HandleHomingBehaviour() {
        GetTarget();
        Vector2 direction = (Vector2)target.position - rigidbody.position;
        direction.Normalize ();
        float rotateAmount = Vector3.Cross (direction, transform.up).z;
        rigidbody.angularVelocity = -projectile.angleSpeed * rotateAmount;
        rigidbody.velocity = transform.up * projectile.speed;
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
            other.GetComponent<EnemyAI>().AbsorbDamage(projectile.damage);
            transform.SetParent(other.transform, true);
            projectileIsSet = false;
        } else {
            projectileIsSet = false;

        }
    }
    public void SetProjectile(Projectile _projectile, float variableSpeed) {
        projectile = _projectile;
        projectileSpeed = projectile.speed + (projectile.speed * variableSpeed * 2f);

        projectileIsSet = true;
    } 
}
