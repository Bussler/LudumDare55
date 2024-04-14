using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhase1 Template", menuName = "ScriptableObjects/BossPhase1")]
public class Phase1 : BossPhase
{
    [SerializeField]
    private GameObject plantPrefab;

    private List<GameObject> plants;

    private BoxCollider bossCollider;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float spawnRadius;

    public override void OnStartPhase(BossController bossController)
    {
        base.OnStartPhase(bossController);

        bossCollider = bossController.GetComponent<BoxCollider>();
        if (bossCollider == null)
        {
            Debug.LogError("Boss does not have a collider");
            return;
        }
        else
        {
            bossCollider.enabled = false;
        }

        // calculate 4 spawn points in a starshape manner for the plants
        Vector3[] spawnPositions = {
            new Vector3(bossController.transform.position.x, bossController.transform.position.y, bossController.transform.position.z + spawnRadius),
            new Vector3(bossController.transform.position.x, bossController.transform.position.y, bossController.transform.position.z - spawnRadius),
            new Vector3(bossController.transform.position.x + spawnRadius, bossController.transform.position.y, bossController.transform.position.z),
            new Vector3(bossController.transform.position.x - spawnRadius, bossController.transform.position.y, bossController.transform.position.z)
        };

        // instantiate the plants at the calculated positions
        plants = new List<GameObject>();
        foreach (Vector3 spawnPos in spawnPositions)
        {
            GameObject plant = ObjectPoolManager.Instance.SpawnObject(plantPrefab, spawnPos, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
            plants.Add(plant);
        }
    }

    public override void ExecutePhase()
    {
        // rotate the plants around the boss
        foreach (GameObject plant in plants)
        {
            plant.transform.RotateAround(boss.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Test if all plants are inactive/ destroyed
        bool allInactive = true;
        foreach (GameObject plant in plants)
        {
            if (plant.activeSelf)
            {
                allInactive = false;
                break;
            }
        }
        if (allInactive)
        {
            OnEndPhase();
        }
    }

    public override void OnEndPhase()
    {
        if (bossCollider != null)
        {
            bossCollider.enabled = true;
        }

        boss.enemyConfig.healthPoints /= 2;

        boss.EndPhase(BossController.PhaseFlag.Flag1);

        // TODO enable when phase 2 is implemented
        //boss.StartPhase(BossController.PhaseFlag.Flag2, 1);
        ProgressionManager.Instance.EndGame();
    }
}
