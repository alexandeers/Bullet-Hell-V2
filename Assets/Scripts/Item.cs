using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("Basic Stats")]
    public new string name;
    public string description;

    [Header("Projectile Stats")]
    public Projectile projectile;

    public virtual void HandleBehaviour(Transform bulletPosition){}

}