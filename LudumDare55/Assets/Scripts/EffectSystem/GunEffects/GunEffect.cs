using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSystem", menuName = "GunSystem/GunEffect", order = 1)]
public class GunEffect : ObjectEffect
{
   
    public override void ApplyEffect(Object obj)
    {
        ApplyGunEffect((Gun)obj);
    }

    public virtual void ApplyGunEffect(Gun gun)
    {

    }

    public override void RemoveEffect(Object obj)
    {
       RemoveGunEffect((Gun)obj);
    }

    public virtual void RemoveGunEffect(Gun gun)
    {

    }
}
