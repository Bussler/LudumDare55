using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private EnemyConfiguration enemyConfig;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private GameObject player;

    private Action enemyBehavior;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fire rate: " + enemyConfig.fireRate);
        Debug.Log("Walking speed: " + enemyConfig.walkingSpeed);
        Debug.Log("Health points: " + enemyConfig.healthPoints);
        player = GameObject.FindWithTag("Player");
        navMeshAgent.SetDestination(player.transform.position);


        enemyBehavior = enemyConfig.enemyBehaviour switch
        {
            EnemyBehaviour.CloseCombatToPlayer => CloseCombatToPlayerBehavior,
            EnemyBehaviour.ShootingToPlayer => ShootingToPlayerBehavior,
            EnemyBehaviour.ChargingToPlayersLastPosition => ChargingToPlayersLastPositionBehavior,
            EnemyBehaviour.StationaryShootingToPlayer => StationaryShootingToPlayerBehavior,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    // Update is called once per frame
    void Update()
    {
        enemyBehavior.Invoke();
        if (enemyConfig.healthPoints > 0)
        {
            // Debug.Log("Enemy is walking");
            enemyBehavior.Invoke();
        }
        else if (enemyConfig.healthPoints <= 0)
        {
            // Debug.Log("Enemy is dead");
        }
        enemyConfig.healthPoints -= 1;
        navMeshAgent.SetDestination(player.transform.position);
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

    public void CloseCombatToPlayerBehavior()
    {
        // Implement CloseCombatToPlayer behavior here
        Debug.Log("Close combat to player");
        //TODO: Implement CloseCombatToPlayer behavior
    }

    public void ShootingToPlayerBehavior()
    {
        // Implement ShootingToPlayer behavior here
        Debug.Log("Shooting to player");
        //TODO: Implement ShootingToPlayer behavior

    }

    public void ChargingToPlayersLastPositionBehavior()
    {
        // Implement ChargingToPlayersLastPosition behavior here
        Debug.Log("Charging to player's last position");
        //TODO: Implement ChargingToPlayersLastPosition behavior
    }

    public void StationaryShootingToPlayerBehavior()
    {
        // Implement StationaryShootingToPlayer behavior here
        Debug.Log("Stationary shooting to player");
        //TODO: Implement StationaryShootingToPlayer behavior
    }
}
