using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Enemy enemyStats;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sprite;

    //"Variable" variables
    RuntimeAnimatorController animatorController;
    int health;

    //Flags
    bool isAttacking = false;

    #region Prerequisites

    private void GetReferences()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator.runtimeAnimatorController = animatorController;
    }

    void InitializeStats() //Copies over values from enemyStats to not change the original
    {
        health = enemyStats.health;
        animatorController = enemyStats.animatorController;
    }

    #endregion

    void Start()
    {
        InitializeStats();
        GetReferences();
    }

    void Update() {
        HandleMovementAndAnimation();
    }

    #region EnemyAI

    public void DealDamage() 
    {
        PlayerHandler.instance.AbsorbDamage(enemyStats.damage);
        isAttacking = false;
    }

    public void AbsorbDamage(int damage) {
        health -= damage;

        if(health <= 0) {
            Destroy(gameObject);
            return;
        }        
    }

    void HandleMovementAndAnimation()
    {
        Vector2 playerPos = PlayerHandler.instance.transform.position;
        sprite.flipX = playerPos.x < transform.position.x;

        //Check if player is in range for an attack
        if (Vector2.Distance(transform.position, playerPos) < enemyStats.attackRange)
        {

            animator.SetBool("Run", false);
            if(!isAttacking)
            {
                animator.SetTrigger("Attack");
                Invoke("DealDamage", animator.GetCurrentAnimatorStateInfo(0).length);
                isAttacking = true;
            }
        }
        else //Move towards player
        {
            if(!animator.GetNextAnimatorStateInfo(0).IsName("Attack")) {
                //Move towards player
                animator.SetBool("Run", true);
                rb.velocity = (playerPos - (Vector2)transform.position).normalized * enemyStats.speed;

            }
        }
    }

    #endregion

    public int GetHealth() => health;
}
