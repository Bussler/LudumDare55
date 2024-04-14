using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeBulletSpeedEffect", order = 1)]
public class ChangeBulletSpeedEfffect : GunEffect
{
    public override void ApplyGunEffect(Gun gun)
    {
        gun.BulletSpeed += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.BulletSpeed -= Change;
    }
}
