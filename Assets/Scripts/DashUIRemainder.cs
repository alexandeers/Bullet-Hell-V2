using UnityEngine;
using UnityEngine.UI;

public class DashUIRemainder : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float fadeOutTime;
    MaterialPropertyBlock propertyBlock;
    float fadeOut = 0f;
    bool isEnabled = false;
    Color startingColor;

    void OnEnable() {
        isEnabled = true;
        startingColor = image.color;
    }

    void Update() {
        if(!isEnabled) return;
        if(fadeOut >= fadeOutTime) Destroy(gameObject);

        fadeOut += Time.deltaTime;
        image.color = Color.Lerp(startingColor, Color.clear, fadeOut / fadeOutTime);
        var lineWidth = Mathf.Lerp(1f, 1.5f, fadeOut / fadeOutTime);
        image.transform.localScale = Vector3.one * lineWidth;
        // image.material.SetFloat("_LineWidth", lineWidth);
    }
}
