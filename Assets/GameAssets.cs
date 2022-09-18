using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    [SerializeField] public GameObject damagePopupPrefab;
    public static GameAssets i;

    void Awake() {
        i = this;
    }
}
