using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] InventoryManager playerInventory;
    public static PlayerHandler i;
    PlayerMovement playerMovement;
    public PlayerStats playerStats;
    UIBarsHandler guiHandler;

    [SerializeField] CanvasGroup canvas;
    [SerializeField] RectTransform container;
    [SerializeField] Volume volume;
    [SerializeField] CameraMouseTarget cameraMouseTarget;

    public event Action<bool> toggleInventory;

    public bool inventoryOpen;
    float pressTimer = 0.3f, duration = 0f;


    void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        guiHandler = GetComponent<UIBarsHandler>();

        if(i == null) {
            i = this;
        } else {
            Debug.Log("PlayerHandler already exists.");
        }
    }

    void Start() {
        duration = pressTimer;
        inventoryOpen = !inventoryOpen;
        canvas.blocksRaycasts = inventoryOpen;
        toggleInventory?.Invoke(inventoryOpen);
    }

    void Update() => HandleInventory();
    void OnValidate() => HandleInventory();

    private void HandleInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && duration <= 0)
        {
            duration = pressTimer;
            inventoryOpen = !inventoryOpen;
            canvas.blocksRaycasts = inventoryOpen;

            toggleInventory?.Invoke(inventoryOpen);
        }

        if (duration > 0)
            duration -= Time.deltaTime;

        float enabled = inventoryOpen ? 1f : 0f;
        canvas.alpha = Mathf.Lerp(canvas.alpha, enabled, Time.deltaTime * 20f);
        container.anchoredPosition = new Vector2(0f, (canvas.alpha * 100f) - 100f);
        volume.weight = canvas.alpha;
    }

    void FixedUpdate() {
        playerMovement.UpdateMovement();
        cameraMouseTarget.UpdateCamera();
        
    }

    public Vector2 GetPlayerPosition() => transform.position;

}
