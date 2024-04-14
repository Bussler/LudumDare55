using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeRangeEffect", order = 1)]
public class ChangeRangeEffect : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.Range += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.Range -= Change;
    }
}
