using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhase1 Template", menuName = "ScriptableObjects/BossPhase1")]
public class Phase1 : BossPhase
{
    public override void OnStartPhase(BossController bossController)
    {
        Debug.Log("Phase 1 started");
        base.OnStartPhase(bossController);
    }

    public override void ExecutePhase()
    {
        Debug.Log("Phase 1 executing");
    }

    public override void OnEndPhase()
    {
        Debug.Log("Phase 1 ended");
    }
}
