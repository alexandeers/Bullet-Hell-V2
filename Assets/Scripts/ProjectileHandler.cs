using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{

    [SerializeField] Projectile projectile;
    // LayerMask LayerMask.NameToLayer("Enemy");
    Transform target;
    new Rigidbody2D rigidbody;
    new CircleCollider2D collider;
    [SerializeField] LayerMask enemyLayer;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    void Update() {
        StartCoroutine("Lifetime");
    }

    IEnumerator Lifetime() {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    void FixedUpdate() {
        if(projectile.isHoming) HandleHomingBehaviour();
        else HandleDefaultBehaviour();
    }

    private void HandleDefaultBehaviour() {
        transform.rotation = transform.parent.rotation;
        rigidbody.velocity = transform.up * projectile.speed;
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
        if(((1<<other.gameObject.layer) & enemyLayer) != 0)
        {
            other.GetComponent<EnemyAI>().AbsorbDamage(projectile.damage);
            Destroy(gameObject);
        }
    }
    public void SetProjectile(Projectile _projectile) => projectile = _projectile;
}
