using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("Basic Stats")]
    public new string name;
    public string description;
    public int level;
    public LevelValues[] levelValues; // consider an array here

    [Header("Projectile Stats")]
    public Projectile projectile;
    public int projectileDamage, projectileCount; 
    public float projectileSpeed;

    public virtual void HandleBehaviour(){}
    public virtual void OnUpgrade(){}

}

[System.Serializable]
public class LevelValues 
{
    public int[] stats;
}