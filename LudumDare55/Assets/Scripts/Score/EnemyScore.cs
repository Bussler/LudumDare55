using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnemyScore : MonoBehaviour
{
    private int points = 100;

    [SerializeField] private EnemyController enemy;

    public void Awake() {
        enemy = enemy == null ? GetComponent<EnemyController>() : enemy;
        if (!enemy.IsAlive.Value) {
            return;
        }
        points = enemy.enemyConfig.highscore;
        //enemy.IsAlive.Subscribe(_ => OnScore()).AddTo(this);
    }

    public void OnScore() {
        if (LootLockerPlayermanager.Instance == null) {
            return;
        }
        LootLockerPlayermanager.Instance.AddScore(points);
    }

    public void AddScore()
    {
        if (LootLockerPlayermanager.Instance == null)
        {
            return;
        }
        LootLockerPlayermanager.Instance.AddScore(points);
    }

}
