using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public EnemyConfiguration enemyConfig;
    private int _currentHealth;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private GameObject player;

    private EnemyShootingComponent shootingComponent;

    private Action enemyBehavior;

    private ChargingStates currentChagingState = ChargingStates.notCharging;

    private float waitedTime = 0f;

    private bool isKnockbackable=true;

    private ReactiveProperty<bool> isAlive = new ReactiveProperty<bool>(true);

    public ReadOnlyReactiveProperty<bool> IsAlive => isAlive.ToReadOnlyReactiveProperty();

    private Animator animator;

    private float timeSinceLastShot = 0;

    public AudioClip hitSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        player = GameObject.FindWithTag("Player");
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }


        enemyBehavior = enemyConfig.enemyBehaviour switch
        {
            EnemyBehaviour.CloseCombatToPlayer => CloseCombatToPlayerBehavior,
            EnemyBehaviour.ShootingToPlayer => ShootingToPlayerBehavior,
            EnemyBehaviour.ChargingToPlayersLastPosition => ChargingToPlayersLastPositionBehavior,
            EnemyBehaviour.StationaryShootingToPlayer => StationaryShootingToPlayerBehavior,
            EnemyBehaviour.Nothing => () => { },
            _ => throw new ArgumentOutOfRangeException()
        };

        if(navMeshAgent != null)
        {
            navMeshAgent.speed = enemyConfig.walkingSpeed;
        }

        shootingComponent = GetComponent<EnemyShootingComponent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hitSound;
    }

    public void Initialize()
    {
        _currentHealth = enemyConfig.healthPoints;
        isAlive.Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyBehavior == null)
        {
            return;
        }

        enemyBehavior.Invoke();
        if (navMeshAgent != null && navMeshAgent.enabled && currentChagingState.Equals(ChargingStates.notCharging))
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        timeSinceLastShot += Time.deltaTime;
    }

    public void Die()
    {
        ItemDrop itemDrop = GetComponent<ItemDrop>();
        if (itemDrop != null)
        {
            itemDrop.Drop();
        }

        ObjectPoolManager.Instance.DespawnObject(this.gameObject);
        EnemySpawner.Instance.OnEnemyDied();
        
        isAlive.Value = false;
        EnemyScore enemyScore = GetComponent<EnemyScore>();
        if (enemyScore != null)
        {
            enemyScore.AddScore();
        }

        if (enemyConfig.dropsBlood)
        {
            player.GetComponent<PlayerStatManager>().gainBlood(enemyConfig.amountBloodDropped);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);
        _currentHealth -= damage;
        audioSource.Play();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(PlayerStatManager.Instance != null)
            {
                if (timeSinceLastShot > enemyConfig.collisionDamageCooldownInSeconds)
                {
                    timeSinceLastShot = 0;
                    PlayerStatManager.Instance.TakeDamage(enemyConfig.collisionDamage);
                }
                //Die();
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
        animator.SetBool("isShooting", false);
        if (CalculateDistanceToPlayer() < enemyConfig.playerDistanceToStartShooting &&
            (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, enemyConfig.playerDistanceRecognition) || hit.transform == player.transform))
        {
            Debug.Log("Stopping distance reached");
            animator.SetBool("isShooting", true);
            shootingComponent.Shoot(directionToPlayer);
        }

        // If the enemy is close enough to the player, stop moving and rotate towards the player so we do not get overwhelmed by the player too easily
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.updateRotation = false;
            //insert your rotation code here
            turnTowardsPlayer(CalculateDirectionToPlayer(), enemyConfig.enemyRotationSpeed);
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
            turnTowardsPlayer(CalculateDirectionToPlayer(), enemyConfig.enemyRotationSpeed);
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
            isKnockbackable = false;
            animator.enabled = true;
            animator.SetBool("isCharging", true);
        }
        else
        {
            turnTowardsPlayer(CalculateDirectionToPlayer(), enemyConfig.enemyRotationSpeed);
        }
    }

    private void performCharging()
    {
        // Once enemy is close enough to player location stop charging
        if (CalculateDistanceToLocation(navMeshAgent.destination) < 1)
        {
            currentChagingState = ChargingStates.coolingDown;
            navMeshAgent.speed = enemyConfig.walkingSpeed;
            isKnockbackable = true;
        }
    }

    private void performCollingDown()
    {
        // TODO verify if we should enable here that the enemy can still slide forward from the movement or not (without this state was possible)
        if (isFinishedCoolingDown())
        {
            currentChagingState = ChargingStates.notCharging;
            waitedTime = 0f;
            animator.SetBool("isCharging", false);
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
                turnTowardsPlayer(directionToPlayer, enemyConfig.enemyRotationSpeed);
                shootingComponent.Shoot(directionToPlayer);
            }
        }
    }

    void turnTowardsPlayer(Vector3 directionToPlayer, float rotationSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //Debug.Log("Rotating towards player");
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
        if (this.gameObject.activeSelf)
        {
            IEnumerator c = ApplyKnockbackCoroutine(knockback);
            StartCoroutine(c);
        }
    }

    private IEnumerator ApplyKnockbackCoroutine(Vector3 knockback)
    {
        if (isKnockbackable && navMeshAgent != null)
        {
            yield return null;
            float x = 0;
            while (x < 0.3f)
            {
                navMeshAgent.Warp(transform.position + knockback * Time.deltaTime);
                x += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();

            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position, out hit, 15, 1);
            navMeshAgent.Warp(hit.position);

            yield return null;
        }
    }


}

public enum ChargingStates
{
    notCharging,
    preparingToCharge,
    charging,
    coolingDown
}