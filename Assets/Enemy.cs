using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float angleSpeed;
    [SerializeField] protected float knockback;
    [SerializeField] protected int damage;
    protected float health;

    [Header("References")]
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image indicator;
    [SerializeField] protected float healthbarHeight;
    protected float indicatorTimer;

    protected MaterialPropertyBlock propertyBlock;
    protected ParticleSystem deathParticles;
    [SerializeField] protected SpriteRenderer[] sprites;
    protected Rigidbody2D rb;
    protected Canvas canvas;

    
    protected bool isDead;
    private float flashAmount;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        deathParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
    
        health = maxHealth;
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();
    }

    void Update() {
        HandleBehaviour();
        HandleDeath();

        if(flashAmount >= 0f) {
            flashAmount -= Time.deltaTime * 10f;
            foreach(SpriteRenderer sprite in sprites) {
                sprite.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat("_Flash", flashAmount);
                sprite.SetPropertyBlock(propertyBlock);
            }
        } else {

        }
    }

    void LateUpdate() => RefreshUI();

    //////////////////  CALLBACKS   //////////////////

    public virtual void HandleBehaviour()
    {
        if(isDead) return;

        Vector2 playerPos = PlayerHandler.i.GetPlayerPosition();
        Vector2 direction = rb.position - playerPos;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = angleSpeed * rotateAmount;
        rb.velocity += (Vector2)transform.up * moveSpeed * Time.deltaTime;
    }

    bool IDamageable.AbsorbDamage(int damage, float knockback, Vector2 source) {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;

        flashAmount = 1f;
        Knockback(knockback, source);
        indicatorTimer = 0.5f;

        if(health <= 0) {
            StartCoroutine("InitiateDeath");
            return true;
        }

        return false;    
    }

    public void Knockback(float intensity, Vector2 source) => rb.AddForce(source * intensity, ForceMode2D.Impulse);

    public IEnumerator InitiateDeath() {
        isDead = true;
        deathParticles.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public void HandleDeath()
    {
        if(!isDead) return;
        // sprite.color = Color.Lerp(sprite.color, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f), Time.deltaTime * 2f);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3f);
        canvas.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void RefreshUI()
    {
        canvas.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        var radians = rb.rotation * Mathf.Deg2Rad;
        canvas.transform.localPosition = new Vector2((float)Mathf.Sin(radians) * healthbarHeight, (float)Mathf.Cos(radians) * healthbarHeight);

        if(indicatorTimer <= 0)
            indicator.fillAmount = Mathf.Lerp(indicator.fillAmount, healthBar.fillAmount, Time.deltaTime * 8f);
        else {
            indicatorTimer -= Time.deltaTime;
        }
    }
}
