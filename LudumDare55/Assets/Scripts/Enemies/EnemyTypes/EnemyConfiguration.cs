using System;
using System.Diagnostics;
using UnityEngine;

public enum EnemyBehaviour
{
    CloseCombatToPlayer,
    ShootingToPlayer,
    ChargingToPlayersLastPosition,
    StationaryShootingToPlayer
};

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration", order = 1)]
public class EnemyConfiguration : ScriptableObject
{

    [Header("General Settings")]
    [Range(0, 5)]
    public float walkingSpeed;

    // How fast an enemy rotates towards the player (used whenever the NavMeshAgent is disabled)
    [Range(0, 5)]
    public float enemyRotationSpeed;

    [Range(0, 100)]
    public int collisionDamage;

    [Range(0, 100)]
    public int healthPoints;

    public bool dropsBlood;

    [Range(0, 100)]
    public int amountBloodDropped;

    public EnemyBehaviour enemyBehaviour;

    [Header("Charging Enemy Settings")]
    // The speed at which the enemy charges towards the player's last position
    [Range(15, 30)]
    public float chargingSpeed;

    [Range(5, 20)]
    public float chargingAcceleration;

    // When the enemy is this close, it will start charging
    [Range(0, 30)]
    public float playerChargingDistance;

    [Range(0, 10)]
    public float playerChargingPreparationSeconds;

    [Range(0, 10)]
    public float playerChargingCooldownSeconds;

    [Header("Stationary Enemy Settings")]
    // When the enemy is this close and sees the player, it will start shooting
    [Range(0, 20)]
    public float playerDistanceRecognition;

    [Header("Shooting Enemy Settings")]

    [Range(7, 20)]
    public float playerDistanceToKeepForShooting;

    [Range(5, 20)]
    public float playerDistanceToStartShooting;
}
