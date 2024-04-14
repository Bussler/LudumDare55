using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave Template", menuName = "ScriptableObjects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public List<EnemyGroup> enemyGroups; // List of enemies in this wave
    public List<SpawnPoint> spawnPoints; // List of spawn points for enemies
    public float spawnInterval = 1; // How often enemies should spawn (in seconds)

    [System.NonSerialized]
    public int waveQuota = 0; // total number of enemies in this wave. Is calculated automatically, if set to 0

    [System.NonSerialized]
    public float spawnCount = 0; // How many enemies have been spawned

    public void CalculateWaveQuota() // Calculates waveQuota based on enemyGroups
    {
        waveQuota = 0;
        foreach (EnemyGroup enemyGroup in enemyGroups)
        {
            waveQuota += enemyGroup.enemyCount;
        }
    }
    
    /// <summary>
    /// Create a deep copy of this wave.
    /// </summary>
    /// <returns></returns>
    public EnemyWave Clone()
    {
        EnemyWave clone = CreateInstance<EnemyWave>();
        clone.enemyGroups = new List<EnemyGroup>();
        foreach (EnemyGroup enemyGroup in enemyGroups)
        {
            clone.enemyGroups.Add(new EnemyGroup
            {
                enemyCount = enemyGroup.enemyCount,
                spawnIntensity = enemyGroup.spawnIntensity,
                enemyPrefab = enemyGroup.enemyPrefab
            });
        }
        clone.spawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            clone.spawnPoints.Add(new SpawnPoint
            {
                spawnPoint = spawnPoint.spawnPoint,
                spawnRadius = spawnPoint.spawnRadius
            });
        }
        clone.spawnInterval = spawnInterval;
        clone.waveQuota = waveQuota;
        clone.spawnCount = spawnCount;
        return clone;
    }
}

[System.Serializable]
public class EnemyGroup
{
    public int enemyCount; // How many enemies in this group should be spawned
    public int spawnIntensity = 1; // How many enemies in this group should be spawned at once
    public GameObject enemyPrefab; // Enemy prefab to spawn

    [System.NonSerialized]
    public float spawnCount = 0; // How many enemies in this group have been spawned
}

[System.Serializable]
public class SpawnPoint
{
    public Vector3 spawnPoint; // Where to spawn enemies
    public float spawnRadius = 1; // Define a square around the spawnPoint where enemies can spawn
}