using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] Item[] items;

    void Update() {
        var index = 0;
        foreach(Item item in items) {
            // item.HandleBehaviour(transform.GetChild(index));
            index++;
        }
    }
}

