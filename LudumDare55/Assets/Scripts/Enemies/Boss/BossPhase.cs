using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhase Template", menuName = "ScriptableObjects/BossPhase")]
public class BossPhase : ScriptableObject
{

    [SerializeField] protected BossController boss;
    public virtual void OnStartPhase(BossController bossController)
    {
        boss = bossController;
    }

    public virtual void ExecutePhase()
    {

    }

    public virtual void OnEndPhase()
    {

    }
}
