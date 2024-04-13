using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private EnemyConfiguration enemyConfig;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fire rate: " + enemyConfig.fireRate);
        Debug.Log("Walking speed: " + enemyConfig.walkingSpeed);
        Debug.Log("Health points: " + enemyConfig.healthPoints);
        player = GameObject.FindWithTag("Player");
        navMeshAgent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyConfig.healthPoints > 0)
        {
            Debug.Log("Enemy is walking");
        }
        else if (enemyConfig.healthPoints <= 0)
        {
            Debug.Log("Enemy is dead");
        }
        enemyConfig.healthPoints -= 1;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy has collided with a player's bullet
        if (collision.gameObject.tag == "PlayerBullet")
        {
            // Reduce the enemy's health
            //enemyConfig.healthPoints -= collision.gameObject.GetComponent<Bullet>().damage;

            // If the enemy's health is 0 or less, destroy the enemy
            if (enemyConfig.healthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
