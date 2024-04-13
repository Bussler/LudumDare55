using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration", order = 1)]
public class EnemyConfiguration : ScriptableObject
{
    [Range(0, 100)]
    public int fireRate;

    [Range(0, 5)]
    public float walkingSpeed;

    [Range(0, 100)]
    public int healthPoints;

}
