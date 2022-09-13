using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable {
    public virtual void Use() { }
}

public interface IChargeable {
    public virtual void Use() { }
}

public class Item : MonoBehaviour
{
    [Header("Basic Stats")]
    public new string name;
    public string description;

}