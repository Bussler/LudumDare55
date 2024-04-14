using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhase2 Template", menuName = "ScriptableObjects/BossPhase2")]
public class Phase2 : BossPhase
{
    public Gun LaserGun;
    public Gun DirectionGun;
    [SerializeField] private int DirectionNumber;

    [SerializeField] private float TimeBetweenAttacks;
    [SerializeField] private EnemyShootingComponent shootingComponent;

    private int AttackCounter;

    public override void OnStartPhase(BossController bossController)
    {
        base.OnStartPhase(bossController);
        AttackCounter = 0;
      //  shootingComponent = this.GetComponent<EnemyShootingComponent>();
        Debug.Log("Phase 2 started");
    }

    public override void ExecutePhase()
    {
        Debug.Log("Phase 2 executing");
        Attack();
    }

    public override void OnEndPhase()
    {
        Debug.Log("Phase 2 ended");
    }


    public void Attack()
    {
        if (AttackCounter %2== 0)
        {
            DoDirectionalAttack();
        }
        else
        {
            DoLaserAttack();
        }
    }

    public void DoLaserAttack()
    {
        shootingComponent.equippedGun = LaserGun;
    }

    public void DoDirectionalAttack()
    {
        shootingComponent.equippedGun = DirectionGun;

        for(int i=0; i< DirectionNumber; i++)
        {
            shootingComponent.Shoot(Quaternion.AngleAxis(-45, Vector3.up)*Vector3.fwd * (360 / DirectionNumber) * i);
        }

    }
}