using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBow : Item
{

    public override void HandleBehaviour(Transform bulletPosition)
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            var _bullet = Instantiate(projectile.prefab, bulletPosition.position, Quaternion.identity).GetComponent<ProjectileHandler>();
            _bullet.SetProjectile(projectile);
        }
    }

}
