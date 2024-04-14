using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : ObjectEffect
{
    public override void ApplyEffect(Object obj)
    {
        ApplyPlayerEffect((PlayerStatManager)obj);
    }

    public virtual void ApplyPlayerEffect(PlayerStatManager playerStats)
    {

    }

    public override void RemoveEffect(Object obj)
    {
        RemovePlayerEffect((PlayerStatManager)obj);
    }

    public virtual void RemovePlayerEffect(PlayerStatManager playerStats)
    {

    }
}
