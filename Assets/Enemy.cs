using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float moveSpeed, angleSpeed;

    [Header("References")]
    [SerializeField] Image healthBar;
    [SerializeField] Image indicator;
    float indicatorTimer;

    ParticleSystem deathParticles;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Canvas canvas;

    float health;
    bool isDead;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        deathParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
    
        health = maxHealth;
    }

    void Update() {
        HandleMovement();
        HandleDeath();
    }

    void LateUpdate() => RefreshUI();

    //////////////////  CALLBACKS   //////////////////

    void HandleMovement()
    {
        if(isDead) return;

        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = rb.position - playerPos;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = angleSpeed * rotateAmount;
        rb.velocity += (Vector2)transform.up * moveSpeed * Time.deltaTime;
    }

    bool IDamageable.AbsorbDamage(int damage, float knockback) {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;

        // flashAmount = 1f;
        Knockback(knockback);
        indicatorTimer = 0.5f;

        if(health <= 0) {
            StartCoroutine("InitiateDeath");
            return true;
        }

        return false;    
    }

    void Knockback(float intensity) {
        Vector2 direction = rb.position - PlayerHandler.i.GetPlayerPosition();
        direction.Normalize();
        rb.AddForce(direction * intensity, ForceMode2D.Impulse);
    }

    IEnumerator InitiateDeath() {
        isDead = true;
        deathParticles.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void HandleDeath()
    {
        if(!isDead) return;
        // sprite.color = Color.Lerp(sprite.color, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f), Time.deltaTime * 2f);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3f);
        canvas.GetComponent<CanvasGroup>().alpha = 0f;
    }

    void RefreshUI()
    {
        canvas.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        var radians = rb.rotation * Mathf.Deg2Rad;
        canvas.transform.localPosition = new Vector2((float)Mathf.Sin(radians), (float)Mathf.Cos(radians));

        if(indicatorTimer <= 0)
            indicator.fillAmount = Mathf.Lerp(indicator.fillAmount, healthBar.fillAmount, Time.deltaTime * 8f);
        else {
            indicatorTimer -= Time.deltaTime;
        }
    }
}
