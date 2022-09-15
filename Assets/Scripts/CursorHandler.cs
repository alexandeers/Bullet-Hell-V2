using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    RectTransform cursorVisual;
    Canvas canvas;

    void Start() {
        cursorVisual = transform.GetChild(0).GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();

        Cursor.visible = false;
    }

    void Update() {
        Vector2 rotationPivot = (Vector2)Camera.main.WorldToScreenPoint(PlayerHandler.i.GetPlayerPosition());
        Vector2 direction = (Vector2)Input.mousePosition - rotationPivot;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);

        cursorVisual.position = canvas.transform.TransformPoint(movePos);
        cursorVisual.rotation = rotation;
    }

}
