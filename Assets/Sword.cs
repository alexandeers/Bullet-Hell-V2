using UnityEngine;

public class Sword : MonoBehaviour, IUseable
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Transform sword;
    [SerializeField] float speed, amplitudeX, amplitudeY;
    [SerializeField] float damage, knockback;
    const float RADIUS = 2.64f;
    float evolution = 1f, lerpedEvolution;

    void Update() {
        lerpedEvolution = Mathf.SmoothStep(lerpedEvolution, evolution, speed * Time.deltaTime);
        float x = Mathf.Cos(lerpedEvolution * RADIUS) * amplitudeX;
        float y = Mathf.Sin(lerpedEvolution * RADIUS) * amplitudeY;
        sword.localPosition = new Vector3(x, y);
        transform.parent.localEulerAngles = new Vector3(0f, 0f, (1f - lerpedEvolution) * -180f);
    }

    public void Use()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            evolution *= -1;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(((1<<other.gameObject.layer) & enemyLayer) != 0)
        {
            if(!other.GetComponent<IDamageable>().AbsorbDamage((int)(damage), knockback, transform.position) ) {
                PlayerHandler.i.playerStats.RegenerateShield(damage);
                DamagePopup.Create(transform.position, (int)(damage), 1f);
                CameraShake.i.Shake(1f + 2.5f, 0.4f);
            }
        }
    }
}
