using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private EnemyConfiguration enemyConfig;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private GameObject player;

    private EnemyShootingComponent shootingComponent;

    private Action enemyBehavior;

    private ChargingStates currentChagingState = ChargingStates.notCharging;

    private float waitedTime = 0f;

    private ReactiveProperty<bool> isAlive = new ReactiveProperty<bool>(true);

    public ReadOnlyReactiveProperty<bool> IsAlive => isAlive.ToReadOnlyReactiveProperty();

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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
        shootingComponent = GetComponent<EnemyShootingComponent>();
    }

    public void Initialize()
    {
        enemyConfig.healthPoints = 100;
    }

    // Update is called once per frame
    void Update()
    {

        enemyBehavior.Invoke();
        if (navMeshAgent.enabled && currentChagingState.Equals(ChargingStates.notCharging))
        {
            navMeshAgent.SetDestination(player.transform.position);
        }

        else if (enemyConfig.healthPoints <= 0)
        {
            // Debug.Log("Enemy is dead");
        }
        enemyConfig.healthPoints -= 1;
    }

    public void Die()
    {
        ObjectPoolManager.Instance.DespawnObject(this.gameObject);
        EnemySpawner.Instance.OnEnemyDied();
        isAlive.Value = false;
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
        navMeshAgent.stoppingDistance = enemyConfig.playerDistanceToKeepForShooting;
        //TODO: Implement ShootingToPlayer behavior
        Vector3 directionToPlayer = CalculateDirectionToPlayer();

        if (CalculateDistanceToPlayer() < enemyConfig.playerDistanceToStartShooting &&
            (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, enemyConfig.playerDistanceRecognition) || hit.transform == player.transform))
        {
            shootingComponent.Shoot(directionToPlayer);
        }

        // If the enemy is close enough to the player, stop moving and rotate towards the player so we do not get overwhelmed by the player too easily
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.updateRotation = false;
            //insert your rotation code here
            turnTowardsPlayer(CalculateDirectionToPlayer(), enemyConfig.stationaryRotationSpeed);
        }
        else
        {
            navMeshAgent.updateRotation = true;
        }

    }

    public void ChargingToPlayersLastPositionBehavior()
    {

        switch (currentChagingState)
        {
            case ChargingStates.notCharging:
                performNotCharging();
                break;
            case ChargingStates.preparingToCharge:
                performPrepareCharging();
                break;
            case ChargingStates.charging:
                performCharging();
                break;
            case ChargingStates.coolingDown:
                performCollingDown();
                break;
        }
    }

    private void performNotCharging()
    {
        if (isCloseEnoughForCharging() && canSeeThePlayer(CalculateDirectionToPlayer()))
        {
            currentChagingState = ChargingStates.preparingToCharge;
            navMeshAgent.enabled = false;
            turnTowardsPlayer(CalculateDirectionToPlayer(), enemyConfig.stationaryRotationSpeed);
        }
    }

    private void performPrepareCharging()
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
        }
        else
        {
            turnTowardsPlayer(CalculateDirectionToPlayer(), enemyConfig.stationaryRotationSpeed);
        }
    }

    private void performCharging()
    {
        // Once enemy is close enough to player location stop charging
        if (CalculateDistanceToLocation(navMeshAgent.destination) < 1)
        {
            currentChagingState = ChargingStates.coolingDown;
            navMeshAgent.speed = enemyConfig.walkingSpeed;
        }
    }

    private void performCollingDown()
    {
        // TODO verify if we should enable here that the enemy can still slide forward from the movement or not (without this state was possible)
        if (isFinishedCoolingDown())
        {
            currentChagingState = ChargingStates.notCharging;
            waitedTime = 0f;
        }
    }

    bool isCloseEnoughForCharging()
    {
        return CalculateDistanceToPlayer() < enemyConfig.playerChargingDistance;
    }

    bool canSeeThePlayer(Vector3 directionToPlayer)
    {
        return !Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, enemyConfig.playerDistanceRecognition) || hit.transform == player.transform;
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
                turnTowardsPlayer(directionToPlayer, enemyConfig.stationaryRotationSpeed);
                shootingComponent.Shoot(directionToPlayer);
            }
        }
    }

    void turnTowardsPlayer(Vector3 directionToPlayer, float rotationSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
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
        this.GetComponent<Rigidbody>().AddForce(knockback, ForceMode.Impulse);
        yield return new WaitForFixedUpdate();
        float knockbackTime = Time.time;
        yield return new WaitUntil(
            () => this.GetComponent<Rigidbody>().velocity.magnitude < 0.25f || Time.time > knockbackTime + 0.015f
            );
        yield return new WaitForSeconds(0.015f);

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().isKinematic = true;
        navMeshAgent.Warp(transform.position);
        navMeshAgent.enabled = true;
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