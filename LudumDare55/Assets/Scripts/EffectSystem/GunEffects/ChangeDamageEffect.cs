using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeDamageEffect", order = 1)]
public class ChangeDamageEffect : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.Damage += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.Damage -= Change;
    }
}
