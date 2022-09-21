using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    [Space]
    [SerializeField] float dashVelocity;
    [SerializeField] int maxCharges;
    [SerializeField] float rechargeRate, cooldown;

    float recharge = 0f, charges = 0f, cooldownDuration = 0f;

    public override void Activate(GameObject player)
    {
        if(charges > 0) {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            rb.velocity = movement.movementInput * dashVelocity;

            charges--;
            cooldownDuration = cooldown;
        }
    }

    public override void Tick(GameObject gameObject)
    {
        if(cooldownDuration > 0f) 
        {
            cooldownDuration -= Time.deltaTime;
        } 
        else if (charges != maxCharges) 
        {
            recharge += Time.deltaTime / rechargeRate;
            if(recharge >= 1f) {
                charges++;
                recharge = 0f;
            }
        }

        UIDash dashUI = gameObject.GetComponent<UIDash>();
        dashUI.SetDashUI((float)charges, maxCharges, recharge);
    }
}
