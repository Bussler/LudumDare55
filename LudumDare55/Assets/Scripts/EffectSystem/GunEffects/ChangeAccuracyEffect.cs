using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangeAccuracyEffect", order = 1)]
public class ChangeAccuracyEffect : GunEffect
{
    // Start is called before the first frame update

    public override void ApplyGunEffect(Gun gun)
    {
        gun.Accuracy += Change;
    }

    public override void RemoveGunEffect(Gun gun)
    {
        gun.Accuracy -= Change;
    }
}
