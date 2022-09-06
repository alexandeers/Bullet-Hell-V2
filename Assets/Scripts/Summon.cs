using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Projectile bullet;
    public Sprite icon;
    public int identifier;
    public Collider2D GetEnemyInVicinity() => Physics2D.OverlapCircle(transform.position, 100f, enemyLayer);

}
