using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Ability : ScriptableObject
{
    [Header("Ability Settings")]
    public new string name;
    public float cooldownTime, activeTime;
    public KeyCode key;
    public bool isTickable;

    public virtual void Activate(GameObject gameObject) {}
    public virtual void Tick(GameObject gameObject) {}

}
