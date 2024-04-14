using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeWeaponTimeEffect", order = 1)]
public class ChangeWeaponTime : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.TimeLimit += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.TimeLimit -= Change;
    }
}
