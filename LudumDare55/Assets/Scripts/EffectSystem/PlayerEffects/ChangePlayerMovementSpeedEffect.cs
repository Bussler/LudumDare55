using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSystem", menuName = "EffectSystem/ChangePlayerMovementSpeedEffect", order = 1)]
public class ChangePlayerMovementSpeedEffect : PlayerEffects
{
    public override void ApplyPlayerEffect(PlayerStatManager playerStats)
    {
        playerStats.MovementSpeed += Change;
    }

    public override void RemovePlayerEffect(PlayerStatManager playerStats)
    {
        playerStats.MovementSpeed -= Change;
    }
}
