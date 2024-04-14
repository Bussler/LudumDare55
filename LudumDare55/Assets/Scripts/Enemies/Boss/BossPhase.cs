using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhase Template", menuName = "ScriptableObjects/BossPhase")]
public class BossPhase : ScriptableObject
{
    public virtual void OnStartPhase()
    {

    }

    public virtual void ExecutePhase()
    {

    }

    public virtual void OnEndPhase()
    {

    }
}
