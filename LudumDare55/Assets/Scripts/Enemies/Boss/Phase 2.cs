using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhase2 Template", menuName = "ScriptableObjects/BossPhase2")]
public class Phase2 : BossPhase
{
    public override void OnStartPhase()
    {
        Debug.Log("Phase 2 started");
    }

    public override void ExecutePhase()
    {
        Debug.Log("Phase 2 executing");
    }

    public override void OnEndPhase()
    {
        Debug.Log("Phase 2 ended");
    }
}