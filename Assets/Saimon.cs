using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Saimon : Enemy, IDamageable
{

    enum SaimonState {
        Idle,
        Chase,
        LaserAttack
    }

    SaimonState state;
    

}
