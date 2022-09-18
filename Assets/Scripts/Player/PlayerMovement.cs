using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    Rigidbody2D rb;
    [SerializeField] Transform spriteParent;

    [Header("Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    private float sizeLerp;
    [SerializeField] float stretchStrength;
    [HideInInspector] public Vector2 movementInput; 

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateMovement() {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity += movementInput * moveSpeed * Time.fixedDeltaTime;
    }

    void Update() {
        HandleSquashAndStretch();
    }

    private void HandleSquashAndStretch()
    {
        if(rb.velocity != Vector2.zero) 
            sizeLerp = Mathf.Lerp(spriteParent.localScale.y, 1f + Mathf.Abs(rb.velocity.magnitude * stretchStrength), Time.deltaTime * 10f);
        else
            sizeLerp = Mathf.Lerp(spriteParent.localScale.y, 1f, Time.deltaTime * 10f);
        
        spriteParent.localScale = new Vector3(1f - (sizeLerp - 1f) * 0.5f, sizeLerp, 1f);
        
        if(rb.velocity != Vector2.zero) {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
            spriteParent.rotation = Quaternion.RotateTowards(spriteParent.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
