using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;

    void OnValidate() {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        nameText = texts[0];
        valueText = texts[1];
    }

}

