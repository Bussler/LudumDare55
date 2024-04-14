using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeBulletsPerShotEffect", order = 1)]
public class ChangeBuletsperShotEffect : GunEffect
{
    // Start is called before the first frame update

    public override void ApplyGunEffect(Gun gun)
    {
        gun.BulletAmount += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.BulletAmount -= Change;
    }
}
