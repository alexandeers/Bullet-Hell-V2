using UnityEngine;

public interface IDamageable
{
    public bool AbsorbDamage(int damage, float knockback = 0f, Vector2 source = new Vector2());
}

public interface IDamager 
{
    public void DealDamage(int damage);
}


public interface IUseable 
{
    public void Use();
}

public enum BowState 
{
    Ready,
    Charging,
    Cooldown
}