using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseTarget : MonoBehaviour
{
    [SerializeField] float followStrength;

    public void UpdateCamera()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width/2f, Input.mousePosition.y - Screen.height/2f) * followStrength * 0.01f;
        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        transform.localPosition = new Vector3((mousePos.x + (playerPos.x * 0f)), (mousePos.y * 1.78f + (playerPos.y * 0f)));
    }
}
