using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject hotbarSlotPrefab;
    [SerializeField] GameObject hotbarParent;
    [SerializeField] List<SummonData> summons;
    List<bool> uniqueSummonIdentifiers = new List<bool> { false, false, false, false, false, false };

    void Update() {
        int index = 0;
        foreach(SummonData summon in summons) {
            if(!uniqueSummonIdentifiers[index]) { 
                Instantiate(summon.prefab, transform.position, Quaternion.identity); 
                Transform hotbarSlot = Instantiate(hotbarSlotPrefab).transform;
                Image hotbarSlotImage = hotbarSlot.transform.GetChild(0).GetComponent<Image>();
                hotbarSlotImage.sprite = summon.icon;
                hotbarSlotImage.rectTransform.parent.localScale = new Vector3(1f, 1f, 1f);
                hotbarSlot.transform.SetParent(hotbarParent.transform, false);
                uniqueSummonIdentifiers[index] = true;
            }
            index++;
        }
    }
}
