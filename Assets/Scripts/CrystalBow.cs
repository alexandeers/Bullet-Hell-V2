using UnityEngine;

public class CrystalBow : Item, IUseable
{

    [SerializeField] Projectile projectile;
    [SerializeField] Transform bulletPosition;
    [SerializeField] Transform charge;
    Vector2 cache_ChargeScale;
    float timerDuration = 1f;
    float duration;
    bool isCharging = false;

    private void Start() {
        cache_ChargeScale = charge.localScale;
    }

    public void Use() {
        if(!isCharging && Input.GetKeyDown(KeyCode.Mouse0)) { duration = 0f; }

        if(isCharging) {
            duration += Time.deltaTime;
            duration = Mathf.Clamp(duration, 0f, timerDuration);
        }

        isCharging = Input.GetKey(KeyCode.Mouse0);

        Debug.Log($"Duration: {duration}. IsCharging: {isCharging}");
    }

    private void Update() {
        // charge.localScale = cache_ChargeScale + (cache_ChargeScale * duration * 0.5f);

        charge.localScale = Vector2.Lerp(charge.localScale, cache_ChargeScale + (cache_ChargeScale * duration * 0.25f), Time.deltaTime * 20f);
        charge.localPosition = Vector2.Lerp(charge.localPosition, new Vector2(0f, -duration * 0.75f), Time.deltaTime * 20f);
    }

    void LateUpdate() {
        if(!Input.GetKey(KeyCode.Mouse0) && isCharging) {
            ShootArrow();
            isCharging = false;
            duration = 0f;
        }
    }

    void ShootArrow() {
        var _bullet = Instantiate(projectile.prefab, bulletPosition.position, bulletPosition.rotation).GetComponent<ProjectileHandler>();
        _bullet.SetProjectile(projectile, duration);
    }
}
