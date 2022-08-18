using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item/Item", order = 0)]
public class Item : ScriptableObject
{

    public new string name;
    public string description;
    public Sprite icon;

}
