using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{

    CircleCollider2D col;
    [SerializeField] int damage, knockback;
    [SerializeField] LayerMask enemyLayer;

    void Awake() {
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;
    }

    public void OnHit() {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, col.radius);
        
        foreach(Collider2D other in results) {
            if(((1<<other.gameObject.layer) & enemyLayer) != 0) {

                print("HIT");
                Vector2 direction = other.transform.position - transform.position;
                other.GetComponent<IDamageable>().AbsorbDamage(damage, knockback, direction.normalized);
            }
        }
    }

}
