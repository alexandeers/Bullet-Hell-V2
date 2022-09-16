using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{

    public bool AbsorbDamage(int damage, float knockback);

}

public interface IDamager 
{

    public void DealDamage(int damage);

}