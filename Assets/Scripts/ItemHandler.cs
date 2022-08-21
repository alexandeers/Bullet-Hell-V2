using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] Item[] items;

    void Update() {
        foreach(Item item in items) {
            item.HandleBehaviour();
        }
    }
}

