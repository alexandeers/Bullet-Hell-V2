using UnityEngine;

public class CrystalBow : MonoBehaviour, IUseable
{

    [SerializeField] GameObject projectile;
    [SerializeField] Transform bulletPosition;
    [SerializeField] Transform chargeTransform;
    BowState state = BowState.Ready;
    protected float charge { get; private set; }
    protected float fullCharge { get; private set; }
    float shootOffset;
    [SerializeField] float cooldownTime;
    float cooldownDuration;

    Projectile loadedArrow;
    Material chargeMaterial => chargeTransform.GetComponent<SpriteRenderer>().material;
    Color initialColor = new Color(1f, 1f, 1f, 0f);

    public void Use() {
        switch (state)
        {
            case BowState.Ready:
                Ready();
                break;

            case BowState.Charging:
                Charging();
                break;

            case BowState.Cooldown:
                Cooldown();
                break;
        }
    }

    void Ready()
    {
        charge = 0f;

        if (!loadedArrow)
        {
            loadedArrow = ReadyArrow();
            loadedArrow.GetComponent<Transform>().SetParent(chargeTransform);
            loadedArrow.GetComponent<SpriteRenderer>().color = initialColor;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            state = BowState.Charging;
        }
    }

    void Charging()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            charge = Mathf.Clamp01(charge + Time.deltaTime + (PlayerHandler.i.playerStats.chargeRate.Value * Time.deltaTime / 100f));
        } else {
            ShootArrow();
            // charge = 1f;
            shootOffset = 0.5f + charge;
            cooldownDuration = cooldownTime;
            state = BowState.Cooldown;
        }
    }

    void Cooldown()
    {
        charge = Mathf.Clamp(charge - (Time.deltaTime * 2f), 0f, 5f);
        cooldownDuration -= Time.deltaTime;

        if (cooldownDuration <= 0)
        {
            state = BowState.Ready;
        }
    }

    private void Update() {
        if(loadedArrow)
            loadedArrow.GetComponent<SpriteRenderer>().color = Color.Lerp(loadedArrow.GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime * 5f);

        shootOffset = Mathf.Clamp01(shootOffset - Time.deltaTime);
        fullCharge = Mathf.Clamp01((charge - 0.80f) * 5f);

        chargeMaterial.SetFloat("_Flash", charge);
        chargeMaterial.SetFloat("_FullCharge", fullCharge);

        // Debug.Log($"Duration: {charge}. State: {state}");
        chargeTransform.localScale = Vector2.Lerp(chargeTransform.localScale, Vector2.one + (Vector2.one * charge * 0.25f), Time.deltaTime * 20f);
        chargeTransform.localPosition = Vector2.Lerp(chargeTransform.localPosition, new Vector2(0f, -charge * 0.75f - shootOffset), Time.deltaTime * 20f);
    }

    Projectile ReadyArrow() {
        return Instantiate(projectile, bulletPosition.position, bulletPosition.rotation).GetComponent<Projectile>();
    }

    void ShootArrow() {
        loadedArrow.GetComponent<Transform>().SetParent(null);
        loadedArrow.SetProjectile(charge, PlayerHandler.i.playerStats.damage.Value);
        loadedArrow = null;
    }
}
