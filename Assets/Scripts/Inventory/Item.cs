using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public new string name;
    public Sprite icon;
    [TextArea(5, 5)]public string description;
}