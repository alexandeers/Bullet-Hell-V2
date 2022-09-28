using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    [Space]
    [SerializeField] float dashVelocity;
    // [SerializeField] int maxCharges;
    [SerializeField] float rechargeRate, cooldown;
    UIDash dashUI;

    float recharge = 0f, charges = 0f, cooldownDuration = 0f;

    public override void Activate(GameObject player)
    {
        charges = Mathf.Clamp(charges, 0f, PlayerHandler.i.playerStats.dashCharges.Value);
        if(charges > 0) {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            rb.velocity = movement.movementInput * dashVelocity;
            
            recharge = 0f;
            dashUI.UseDash(charges, (int)PlayerHandler.i.playerStats.dashCharges.Value);
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
        else if (charges != (int)PlayerHandler.i.playerStats.dashCharges.Value) 
        {
            recharge += Time.deltaTime / rechargeRate;
            if(recharge >= 1f) {
                charges++;
                recharge = 0f;
                dashUI.FlashUI();
            }
        }
        
        dashUI.SetDashUI((float)charges, (int)PlayerHandler.i.playerStats.dashCharges.Value, recharge);
    }

    public override void OnDamage(float value)
    {
        if(charges == (int)PlayerHandler.i.playerStats.dashCharges.Value) return;
        charges++;
        dashUI.FlashUI();
    }
}
