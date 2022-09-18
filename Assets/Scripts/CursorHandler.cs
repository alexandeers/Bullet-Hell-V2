using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    RectTransform reticleVisual;
    Canvas canvas;

    void Start() {
        reticleVisual = transform.GetChild(0).GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();

        PlayerHandler.i.toggleInventory += ToggleReticle;
    }

    void ToggleReticle(bool isEnabled)
    {
        if(isEnabled)
            reticleVisual.gameObject.SetActive(false);
        else
            reticleVisual.gameObject.SetActive(true);
    }

    void Update() {
        var inventoryOpen = PlayerHandler.i.inventoryOpen;
        Cursor.visible = inventoryOpen;

        if(!inventoryOpen) {
            Vector2 rotationPivot = (Vector2)Camera.main.WorldToScreenPoint(PlayerHandler.i.GetPlayerPosition());
            Vector2 direction = (Vector2)Input.mousePosition - rotationPivot;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera,
                out movePos);

            reticleVisual.position = canvas.transform.TransformPoint(movePos);
            reticleVisual.rotation = rotation;
        }
    }
}
