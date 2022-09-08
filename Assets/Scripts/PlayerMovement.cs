using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;

    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AnimationClip walkUp, walkDown, walkLeft, walkRight;
    [SerializeField] AnimationClip idleUp, idleDown, idleLeft, idleRight;

    [Header("Variables")]
    [SerializeField] private float moveSpeed;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator.speed = 0.15f;
    }

    public void UpdateMovement() {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    void Update() {
        // HandleAnimator();
    }

    public void HandleAnimator() {
        var isWalking = rb.velocity.x != 0 || rb.velocity.y != 0;
        var x = rb.velocity.x;
        var y = rb.velocity.y;

        int walkDirection = y != 0 ? (int)Mathf.Sign(y) : 0;

        if (isWalking) {
            animator.SetBool("isWalking", true);
            animator.SetFloat("x", x);
            animator.SetFloat("y", y);
            animator.SetInteger("walkDirection", walkDirection);
        } else {
            animator.SetBool("isWalking", false);
        }   
    }
}
