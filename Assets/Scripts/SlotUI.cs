using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    Image image;
    public ItemSlot itemSlot;

    Color originalColor;
    Color highlightColor = new Color(1f, 1f, 1f, 0.6f);

    bool isHighlighted = false;

    void Start() {
        image = GetComponent<Image>();
        originalColor = image.color;
        itemSlot.OnPointerEnterEvent += HighlightOnHover;
        itemSlot.OnPointerExitEvent += UnHighlight;
    }

    void UnHighlight(ItemSlot obj)
    {
        isHighlighted = false;
    }

    void HighlightOnHover(ItemSlot obj)
    {
        isHighlighted = true;
    }

    void Update() 
    {
        if(isHighlighted) 
        {
            image.color = Color.Lerp(image.color, highlightColor, Time.deltaTime*20f);
        }
        else
        {
            image.color = Color.Lerp(image.color, originalColor, Time.deltaTime*20f);
        }
    }
}
