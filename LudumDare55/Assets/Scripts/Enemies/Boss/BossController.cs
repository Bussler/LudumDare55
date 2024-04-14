using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using static ProgressionManager;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class BossController : MonoBehaviour
{
    public EnemyConfiguration enemyConfig;

    private GameObject player;

    private EnemyShootingComponent shootingComponent;

    private Animator animator;

    private List<BossPhase> phases;

    [Flags]
    public enum PhaseFlag
    {
        None = 0,
        Flag1 = 1 << 0,  // 1
        Flag2 = 1 << 1,  // 2
    }

    PhaseFlag phaseFlag = PhaseFlag.None;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        player = GameObject.FindWithTag("Player");
        shootingComponent = GetComponent<EnemyShootingComponent>();
        animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        enemyConfig.healthPoints = 100;
        StartPhase(PhaseFlag.Flag1, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BasicShooting();

        // Check if the boss has a phase to execute
        int counter = 0;
        foreach (PhaseFlag phase in Enum.GetValues(typeof(PhaseFlag)))
        {
            if (CheckPhaseFlag(phase))
            {
                phases[counter].ExecutePhase();
            }
            counter++;
        }
    }

    /// <summary>
    /// Basic shooting logic for the boss.
    /// This is executed in every phase.
    /// </summary>
    private void BasicShooting()
    {
        // TODO implement basic shooting logic

        // IDEA: Give boss multiple weapons, each with different shooting patterns.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PlayerStatManager.Instance != null)
            {
                PlayerStatManager.Instance.TakeDamage(enemyConfig.collisionDamage);
            }
        }
    }

    public void StartPhase(PhaseFlag flag, int i)
    {
        phaseFlag |= flag;
        phases[i].OnStartPhase();
    }

    public void EndPhase(PhaseFlag flag, int i)
    {
        phaseFlag &= ~flag;
        phases[i].OnEndPhase();
    }

    public bool CheckPhaseFlag(PhaseFlag flag)
    {
        return (phaseFlag & flag) == flag;
    }

    public void Die()
    {
        ObjectPoolManager.Instance.DespawnObject(this.gameObject);

        ProgressionManager.Instance.EndGame();
    }

    public void TakeDamage(int damage)
    {
        enemyConfig.healthPoints -= damage;
        if (enemyConfig.healthPoints <= 0)
        {
            Die();
        }
    }

}
