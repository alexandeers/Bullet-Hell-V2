using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Possessed Skull", menuName = "Create New Item/Possessed Skull", order = 0)]  
public class PossessedSkull : Item
{


    public Sprite sprite;

    void Start() => OnUpgrade();

    public override void HandleBehaviour()
    {
        
    }

    public override void OnUpgrade()
    {
        projectileDamage = levelValues[level].stats[0];
        projectileSpeed = levelValues[level].stats[1];
        projectileCount = levelValues[level].stats[2];
    }

}
