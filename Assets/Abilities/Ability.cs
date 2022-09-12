using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Ability : ScriptableObject
{

    public new string name;
    public float cooldownTime, activeTime;
    public KeyCode key;

    public virtual void Activate(GameObject player) {}

}
