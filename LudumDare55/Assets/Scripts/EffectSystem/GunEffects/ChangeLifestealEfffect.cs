using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeLifeStealEffect", order = 1)]
public class ChangeLifestealEfffect :GunEffect
{
    // Start is called before the first frame update
    public override void ApplyGunEffect(Gun gun)
    {
        gun.LifeSteal += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.LifeSteal -= Change;
    }
}
