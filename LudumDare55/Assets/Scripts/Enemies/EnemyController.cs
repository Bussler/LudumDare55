using System;
using System.Collections;
using UnityEngine;
using System.Collections;
public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private EnemyConfiguration enemyConfig;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private GameObject player;

    private Action enemyBehavior;

    private ChargingStates currentChagingState = ChargingStates.notCharging;

    private float waitedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fire rate: " + enemyConfig.fireRate);
        Debug.Log("Walking speed: " + enemyConfig.walkingSpeed);
        Debug.Log("Health points: " + enemyConfig.healthPoints);
        player = GameObject.FindWithTag("Player");
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }


        enemyBehavior = enemyConfig.enemyBehaviour switch
        {
            EnemyBehaviour.CloseCombatToPlayer => CloseCombatToPlayerBehavior,
            EnemyBehaviour.ShootingToPlayer => ShootingToPlayerBehavior,
            EnemyBehaviour.ChargingToPlayersLastPosition => ChargingToPlayersLastPositionBehavior,
            EnemyBehaviour.StationaryShootingToPlayer => StationaryShootingToPlayerBehavior,
            _ => throw new ArgumentOutOfRangeException()
        };

        navMeshAgent.speed = enemyConfig.walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        enemyBehavior.Invoke();
        if (navMeshAgent.enabled && currentChagingState.Equals(ChargingStates.notCharging))
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
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
        //Debug.Log("Close combat to player");
        //TODO: Check  if we need any additional behavior other than running towards the player
    }

    public void ShootingToPlayerBehavior()
    {
        // Implement ShootingToPlayer behavior here
        Debug.Log("Shooting to player");
        //TODO: Implement ShootingToPlayer behavior

    }

    public void ChargingToPlayersLastPositionBehavior()
    {
        if (currentChagingState.Equals(ChargingStates.charging))
        {
            // Once enemy is close enough to player location stop charging
            if (CalculateDistanceToLocation(navMeshAgent.destination) < 1)
            {
                currentChagingState = ChargingStates.coolingDown;
                navMeshAgent.speed = enemyConfig.walkingSpeed;
            }
        } else if(currentChagingState.Equals(ChargingStates.coolingDown)) {
            // TODO verify if we should enable here that the enemy can still slide forward from the movement or not (without this state was possible)
            if (isFinishedCoolingDown())
            {
                currentChagingState = ChargingStates.notCharging;
                waitedTime = 0f;
            }
        }
        else
        {
            // only start charging once we are close to the player and see the player
            Vector3 directionToPlayer = CalculateDirectionToPlayer();

            if (
                currentChagingState.Equals(ChargingStates.preparingToCharge) ||
                (CalculateDistanceToPlayer() < enemyConfig.playerChargingDistance &&
                (
                    !Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, enemyConfig.playerDistanceRecognition) ||
                    hit.transform == player.transform
                ))
            )
            {
                if (isFinishedCharging())
                {
                    // Charge once we are close to the player
                    navMeshAgent.enabled = true;
                    Debug.Log("Charging to player's last position");
                    navMeshAgent.speed = enemyConfig.chargingSpeed;
                    navMeshAgent.acceleration = enemyConfig.chargingAcceleration;
                    currentChagingState = ChargingStates.charging;
                    navMeshAgent.SetDestination(player.transform.position);
                    waitedTime = 0f;
                } else {
                    currentChagingState = ChargingStates.preparingToCharge;
                    navMeshAgent.enabled = false;
                    turnTowardsPLayer(CalculateDirectionToPlayer(), navMeshAgent.angularSpeed);
                }
            }
        }
    }

    bool isFinishedCharging()
    {
        waitedTime += Time.deltaTime;
        return waitedTime > enemyConfig.playerChargingPreparationSeconds;
    }

    bool isFinishedCoolingDown()
    {
        waitedTime += Time.deltaTime;
        return waitedTime > enemyConfig.playerChargingCooldownSeconds;
    }

    public void StationaryShootingToPlayerBehavior()
    {
        if (CalculateDistanceToPlayer() < enemyConfig.playerDistanceRecognition)
        {
            Vector3 directionToPlayer = CalculateDirectionToPlayer();

            // only rotate if the player is not in sight
            if (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, enemyConfig.playerDistanceRecognition) || hit.transform == player.transform)
            {
                turnTowardsPLayer(directionToPlayer, enemyConfig.stationaryRotationSpeed);
            }
        }
    }

    void turnTowardsPLayer(Vector3 directionToPlayer, float rotationSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, enemyConfig.stationaryRotationSpeed * Time.deltaTime);
        Debug.Log("Rotating towards player");
    }

    float CalculateDistanceToPlayer()
    {
        return CalculateDistanceToLocation(player.transform.position);
    }

    float CalculateDistanceToLocation(Vector3 location)
    {
        return Vector3.Distance(transform.position, location);
    }

    Vector3 CalculateDirectionToPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        return direction;
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        IEnumerator c = ApplyKnockbackCoroutine(knockback);
        StartCoroutine(c);
    }

    private IEnumerator ApplyKnockbackCoroutine(Vector3 knockback)
    {
        yield return null;

        navMeshAgent.enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Force);
        yield return new WaitForFixedUpdate();
        float knockbackTime = Time.time;
        yield return new WaitUntil(
            () => this.GetComponent<Rigidbody>().velocity.magnitude < 0.1f || Time.time > knockbackTime + 1
            ) ;
        yield return new WaitForSeconds(0.25f);

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity= Vector3.zero;
        this.GetComponent<Rigidbody>().isKinematic= true;
        navMeshAgent.Warp(transform.position);
        navMeshAgent.enabled= true;
        yield return null;

    }


}

public enum ChargingStates
{
    notCharging,
    preparingToCharge,
    charging,
    coolingDown
}