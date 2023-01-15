using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro text;
    float dissapearTimer;
    private Color textColor;

    public static DamagePopup Create(Vector3 position, int damage, float size) {
        var damagePopupTransform = Instantiate(GameAssets.i.damagePopupPrefab, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.SetInformation(damage, size);
        return damagePopup;
    }

    void Awake() {
        text = GetComponent<TextMeshPro>();
    }

    void SetInformation(int damage, float size) {
        text.SetText(damage.ToString());
        text.fontSize += (text.fontSize) * size * 0.5f;
        textColor = text.color;
        dissapearTimer = 0.5f;
    }

    void Update() {
        float moveYSpeed = 5f;
        transform.position += new Vector3(0,  moveYSpeed) * Time.deltaTime;

        dissapearTimer -= Time.deltaTime;
        if(dissapearTimer <= 0) {
            textColor.a -= Time.deltaTime * 3f;
            text.color = textColor;
            if(textColor.a <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
