using UnityEngine;
using UnityEngine.UI;

public class DamagedBar : MonoBehaviour
{
    RectTransform rectTransform;
    Color cachedColor;
    Image image;
    float timer = 1f;

    void OnEnable() {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        cachedColor = image.color;

        // image.material = new Material(cachedMaterial);
    }

    void Update() {
        image.color = new Color(cachedColor.r, cachedColor.g, cachedColor.b, timer);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 5f * (Time.deltaTime * (timer * 10f)));
        timer -= Time.deltaTime;

        if(timer <= 0) {
            Destroy(gameObject);
        }

    }

}
