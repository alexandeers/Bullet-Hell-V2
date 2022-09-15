using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item/Projectile", order = 0)]
public class Projectile : ScriptableObject
{

    [Header("Basic Stats")]
    public new string name;
    public float speed;
    public int damage; 
    public int knockback; 
    public Sprite sprite;
    public GameObject prefab;

    [Header("Optional Flags")]
    public bool isHoming;
    public bool faceDirection;

    [Header("Optional Stats")]
    public float angleSpeed;
}
