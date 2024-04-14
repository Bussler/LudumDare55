using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{

    public static PlayerStatManager Instance;

    private bool _isTargetable = true;
    public bool IsTargetable { get => _isTargetable; set => _isTargetable = value; }

    [Header("Health")]
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField]
    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value <= 0)
                _currentHealth = 0;
            else if (value >= _maxHealth)
                _currentHealth = _maxHealth;
            else
                _currentHealth = value;
        }
    }

    [Header("Blood")]
    [SerializeField]
    private int _maxBlood;
    public int MaxBlood
    {
        get => _maxBlood;
        set => _maxBlood = value;
    }

    [SerializeField]
    private int _startBlood;

    [SerializeField]
    private int _currentBlood;
    public int CurrentBlood
    { 
        get => _currentBlood;
        set
        {
            if (value <= 0)
            {
                _currentBlood = 0;
            }
            else if (value >= _maxBlood)
            {
                _currentBlood = _maxBlood;
            }
            else
            {
                _currentBlood = value;
            }
        }
    }

    [SerializeField]
    private int _bloodDrain;
    public int BloodDrain
    {
        get => _bloodDrain;
        set
        {
           _bloodDrain = value;
        }
    }

    private int _circleUsedBlood;
    public int CircleUsedBlood
    {
        get => _circleUsedBlood;
    }

    [Header("Movement")]
    [SerializeField]
    private float _movementSpeed;
    public float MovementSpeed
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }

    [SerializeField]
    private int _dashingPower = 15; // Speed at which the player dashes
    public int DashingPower { get => _dashingPower; set => _dashingPower = value; }

    [SerializeField]
    private float _dashingTime = 0.3f; // How long the player dashes
    public float DashingTime { get => _dashingTime; set => _dashingTime = value; }

    [SerializeField]
    private float _dashingCooldown = 1.0f; // Cooldown between dashes
    public float DashingCooldown { get => _dashingCooldown; set => _dashingCooldown = value; }

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Initialize();
    }

    public void Initialize()
    {
        CurrentHealth = MaxHealth;
        CurrentBlood = _startBlood;
    }

    public void TakeDamage(int amount)
    {
        if (!IsTargetable)
        {
            return;
        }
        CurrentHealth -= amount;

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal( int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth+ amount,MaxHealth);
    }

    public void HealToMax()
    {
        CurrentHealth = MaxHealth;
    }

    public void Die()
    {
        Debug.Log("YOU DED");
        ProgressionManager.Instance.EndGame();
    }

    public void gainBlood(int amount)
    {
        CurrentBlood += amount;
        Debug.Log("Player gained blood: " + amount);
    }

    public void useBlood(int amount)
    {
        _circleUsedBlood += amount;
    }

    public void commitUsedBlood()
    {
        CurrentBlood -= _circleUsedBlood;
    }

    public void resetUsedBlood()
    {
        _circleUsedBlood = 0;
    }

    public void setMaxBlood()
    {
        CurrentBlood = MaxBlood;
    }
}
