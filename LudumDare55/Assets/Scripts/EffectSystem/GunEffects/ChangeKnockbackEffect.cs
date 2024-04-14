using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeKnockBackEffect", order = 1)]
public class ChangeKnockbackEffect : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.BulletKnockback += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.BulletKnockback -= Change;
    }
}
