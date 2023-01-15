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

    void Update() {
        switch (state)
        {
            case SaimonState.Chase:
                Chase();
                break;
            case SaimonState.Idle:
                Idle();
                break;
            case SaimonState.LaserAttack:
                LaserAttack();
                break;
        }
    }

    void Chase() {

    }

    void Idle() {

    }

    void LaserAttack() {
        
    }
}
