using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/New Enemy")]
public class EnemyStats : ScriptableObject
{
    //Variables
    [Header("Basic Stats")]
    public new string name;
    public RuntimeAnimatorController animatorController;

    public int health;
    public int damage;
    public float speed;
    public GameObject enemyPrefab;
    public float attackRange;

    [Header("Drops")]
    public int xpDrop;

    [Header("Optional Tags")]
    public bool isBoss;
    public bool isRanged;

    [Header("Ranged Stats")]
    public GameObject bullet;

}
