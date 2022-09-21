using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    [Space]
    [SerializeField] float dashVelocity;
    [SerializeField] int maxCharges;
    [SerializeField] float rechargeRate, cooldown;
    UIDash dashUI;

    float recharge = 0f, charges = 0f, cooldownDuration = 0f;

    public override void Activate(GameObject player)
    {
        if(charges > 0) {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            rb.velocity = movement.movementInput * dashVelocity;
            
            recharge = 0f;
            dashUI.UseDash(charges, maxCharges);
            charges--;
            cooldownDuration = cooldown;
            
        }
    }

    public override void Tick(GameObject gameObject)
    {
        if(!dashUI)
            dashUI = gameObject.GetComponent<UIDash>();

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
                dashUI.FlashUI();
            }
        }
        
        dashUI.SetDashUI((float)charges, maxCharges, recharge);
    }

    public override void OnDamage(float value)
    {
        if(charges == maxCharges) return;
        charges++;
        dashUI.FlashUI();
    }
}
