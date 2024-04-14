using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeFireRateEffect", order = 1)]
public class ChangeFireRateEffect : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.FireRate += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.FireRate -= Change;
    }
}
