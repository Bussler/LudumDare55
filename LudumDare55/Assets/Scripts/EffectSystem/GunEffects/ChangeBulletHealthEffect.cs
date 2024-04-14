using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeBulletHealthEffect", order = 1)]
public class ChangeBulletHealthEffect : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.BulletHealth += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.BulletHealth -= Change;
    }
}
