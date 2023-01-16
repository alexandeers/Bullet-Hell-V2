using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // VARIABLES
    [SerializeField] float FUSE_DURATION;
    [SerializeField] float damage = 5;
    float fuseTimer;

    // VISUALS
    [SerializeField] SpriteRenderer border;
    [SerializeField] SpriteRenderer fuseTimerVisual;

    void Start() {
        fuseTimer = FUSE_DURATION;
    }

    void Update() => Tick();

    void Tick()
    {
        fuseTimer -= Time.deltaTime;
        if(FUSE_DURATION <= 0) Ignite();

        var scale = (FUSE_DURATION - (fuseTimer * FUSE_DURATION));
        fuseTimerVisual.transform.localScale = new Vector2(scale, scale);

    }

    void Ignite()
    {
        
    }
}
