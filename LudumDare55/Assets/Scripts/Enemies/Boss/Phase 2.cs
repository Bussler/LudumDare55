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
    private float time;
    [SerializeField] private EnemyShootingComponent shootingComponent;
    private GameObject player;

    private int AttackCounter=0;
    private float additionalRotation=5;

    public override void OnStartPhase(BossController bossController)
    {
        base.OnStartPhase(bossController);
        AttackCounter = 0;
        player = GameObject.FindWithTag("Player");
        shootingComponent = boss.GetComponent<EnemyShootingComponent>();
        
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
        ProgressionManager.Instance.EndGame();
    }


    public void Attack()
    {
        Debug.Log("Attack");
        time += Time.fixedDeltaTime;
        if (time > TimeBetweenAttacks)
        {
            if (AttackCounter % 2 == 0)
            {
                DoDirectionalAttack();
            }
            else
            {
                DoLaserAttack();
            }
            time = 0;
            AttackCounter++;
        }
    }

    public void DoLaserAttack()
    {
        shootingComponent.equippedGun = LaserGun;
        Debug.Log("DoLaser");
      
    }

    public void DoDirectionalAttack()
    {
        Debug.Log("DoDir");
        shootingComponent.equippedGun = DirectionGun;

        Vector3[] directions = new Vector3[DirectionNumber];
        float random = Random.Range(0, 360);
        for(int i=0; i< DirectionNumber; i++)
        {
            directions[i]=Quaternion.AngleAxis((360 / DirectionNumber) * i+random, Vector3.up)*Vector3.fwd ;
        }
        shootingComponent.SetCanShoot();
        shootingComponent.ShootMultipleTimes(directions);

    }
}