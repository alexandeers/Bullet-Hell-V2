using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ScriptableObject
{

    [Header("Basic Stats")]
    public new string name;
    public Sprite sprite;

    [Header("Optional Flags")]
    public bool isHoming;

    //Internal variables
    Transform target;

    public virtual void HandleBehaviour()
    {

    }

}
