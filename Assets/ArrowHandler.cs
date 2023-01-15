using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowHandler : MonoBehaviour
{
    // ARROW INVENTORY
    [SerializeField] GameObject[] arrowInventory;
    [SerializeField] int[] arrowInventoryCount;
    public GameObject equippedArrow;
    int equippedIndex = 0;

    // UI
    [SerializeField] GameObject[] arrowContainerSlots;
    [SerializeField] Image equippedArrowImage;
    [SerializeField] TextMeshProUGUI arrowCounter;

    public Action onChangeArrow;

    void Start() {
        EquipArrow();
        RefreshSlots();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Q) && !Input.GetKey(KeyCode.Mouse0)) {
            EquipArrow();
            RefreshSlots();
            onChangeArrow?.Invoke();
        }
    }

    void RefreshSlots()
    {
        equippedArrowImage.sprite = equippedArrow.GetComponent<SpriteRenderer>().sprite;
        int index = 0;
        foreach(GameObject arrow in arrowInventory) {
            if(arrow == equippedArrow) {
                arrowCounter.text = arrowInventoryCount[index].ToString();
                continue;
            };
            arrowContainerSlots[index].transform.GetChild(0).GetComponent<Image>().sprite = arrow.GetComponent<SpriteRenderer>().sprite;
            index++;
        }
    }

    void EquipArrow() {
        equippedIndex = equippedIndex >= arrowInventory.Length - 1 ? 0 : equippedIndex += 1;
        equippedArrow = arrowInventory[equippedIndex];
    }

    internal bool DepleteArrow(Projectile loadedArrow)
    {
        int index = 0;
        foreach (GameObject arrow in arrowInventory)
        {
            if(arrow.tag == loadedArrow.tag) {
                if(arrowInventoryCount[index] > 0) {
                    arrowInventoryCount[index]--;
                    arrowCounter.text = $"{arrowInventoryCount[index]}";
                    return true;
                } else {
                    return false;
                }
            }
            index++;
        }
        return false;
    }
}