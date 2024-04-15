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
    }

    public override void ExecutePhase()
    {
        Attack();
    }

    public override void OnEndPhase()
    {
        ProgressionManager.Instance.EndGame();
    }


    public void Attack()
    {
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
    }

    public void DoDirectionalAttack()
    {
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