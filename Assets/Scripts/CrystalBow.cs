using UnityEngine;

public class CrystalBow : Item, IUseable
{

    [SerializeField] Projectile projectile;
    [SerializeField] Transform bulletPosition;
    [SerializeField] Transform chargeTransform;
    BowState state = BowState.Ready;
    float charge, fullCharge;
    float shootOffset;
    ProjectileHandler loadedArrow;
    Material chargeMaterial => chargeTransform.GetComponent<SpriteRenderer>().material;
    Color initialColor = new Color(1f, 1f, 1f, 0f);

    public void Use() {
        switch (state)
        {
            case BowState.Ready:
                charge = 0f;

                if(!loadedArrow) {
                    loadedArrow = ReadyArrow();
                    loadedArrow.GetComponent<Transform>().SetParent(chargeTransform);
                    loadedArrow.GetComponent<SpriteRenderer>().color = initialColor;
                }
                if(Input.GetKey(KeyCode.Mouse0)) {
                    state = BowState.Charging;
                }
                break;

            case BowState.Charging:
                if(Input.GetKey(KeyCode.Mouse0)) {
                    charge = Mathf.Clamp01(charge + Time.deltaTime);
                } else {
                    ShootArrow();
                    charge = 1f;
                    shootOffset = 1f + charge * 0.5f;
                    state = BowState.Cooldown;
                    shootOffset = charge;
                }
                break;

            case BowState.Cooldown:
                charge -= Time.deltaTime * 2f;
                if(charge <= 0) {
                    state = BowState.Ready;
                }
                break;
        }
    }


    private void Update() {
        if(loadedArrow)
            loadedArrow.GetComponent<SpriteRenderer>().color = Color.Lerp(loadedArrow.GetComponent<SpriteRenderer>().color, new Color(1f, 1f, 1f, 1f), Time.deltaTime * 5f);

        shootOffset = Mathf.Clamp01(shootOffset - Time.deltaTime);
        fullCharge = Mathf.Clamp01((charge - 0.85f) * 5f);

        chargeMaterial.SetFloat("_Flash", charge);
        chargeMaterial.SetFloat("_FullCharge", fullCharge);

        // Debug.Log($"Duration: {charge}. State: {state}");
        chargeTransform.localScale = Vector2.Lerp(chargeTransform.localScale, Vector2.one + (Vector2.one * charge * 0.25f), Time.deltaTime * 20f);
        chargeTransform.localPosition = Vector2.Lerp(chargeTransform.localPosition, new Vector2(0f, -charge * 0.75f - shootOffset), Time.deltaTime * 20f);
    }

    ProjectileHandler ReadyArrow() {
        return Instantiate(projectile.prefab, bulletPosition.position, bulletPosition.rotation).GetComponent<ProjectileHandler>();
    }

    void ShootArrow() {
        loadedArrow.GetComponent<Transform>().SetParent(null);
        loadedArrow.SetProjectile(projectile, charge);
        loadedArrow = null;
    }
}
