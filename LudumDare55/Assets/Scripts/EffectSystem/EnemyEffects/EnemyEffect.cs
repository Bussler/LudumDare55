using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : ObjectEffect
{
    public override void ApplyEffect(Object obj)
    {
        ApplyEnemyEffect((EnemyController)obj);
    }

    public virtual void ApplyEnemyEffect(EnemyController enemy)
    {

    }

    public override void RemoveEffect(Object obj)
    {
        RemoveEnemyEffect((EnemyController)obj);
    }

    public virtual void RemoveEnemyEffect(EnemyController enemy)
    {

    }
}
