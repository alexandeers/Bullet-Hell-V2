using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Controls", menuName = "Create New Item/Controls", order = 0)]
public class Controls : ScriptableObject
{

    public KeyCode debug_increaseHealth;
    public KeyCode debug_increaseStamina;

    public KeyCode debug_increaseHealthMax;
    public KeyCode debug_increaseStaminaMax;
    
    public KeyCode debug_inverseEffect;

}
