using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Summon Data", menuName = "Create New Item/Summon Data", order = 0)]
public class SummonData : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
}
