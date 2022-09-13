using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artemis : Summon
{
    private void Start() => InvokeRepeating("ShootBullet", 1f, 1f);

    void ShootBullet() {
        if(!GetEnemyInVicinity()) return;
        
        // var _bullet = Instantiate(bullet.prefab, transform.position, Quaternion.identity).GetComponent<ProjectileHandler>();
        // _bullet.SetProjectile(bullet);
    }

}
