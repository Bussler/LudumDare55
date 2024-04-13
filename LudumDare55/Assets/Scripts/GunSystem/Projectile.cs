using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _size;
    private float _speed;
    private int _damage;
    private float _range;
    private float _knockBack;
    private int  _health;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale.Set(_size, _size, _size);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
    }


    public void InitProjectile(int damage, float speed,float knockback, float range, float size, int health =1)
    {
        _damage = damage;
        _speed = speed;
        _range = range;
        _size = size;
        _health = health;
        _knockBack = knockback;
    }

    public void MoveForward()
    {
        this.transform.Translate(Vector3.forward*Time.fixedDeltaTime*_speed, Space.Self);
        _range -= Time.fixedDeltaTime;

        if( _range <= 0)
        {
            DestroyProjectile();
        }
    }

    public void TakeDamage(int damage)
    {
        _health-= damage;
        if (_health <= 0)
        {
            DestroyProjectile();
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            DestroyProjectile();
        }

        if (collision.gameObject.tag == "Projectile")
        {
            TakeDamage(1);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                // TODO Knockback on enemies can knock them off the navmesh
                //enemy.ApplyKnockback(_knockBack * transform.forward);
            }

            TakeDamage(1);
        } 
        else if (collision.gameObject.tag == "Player")
        {
            if (PlayerStatManager.Instance != null)
            {
                PlayerStatManager.Instance.TakeDamage(_damage);
                BasicMovement player_movement = collision.gameObject.GetComponent<BasicMovement>();
                if (player_movement != null)
                    player_movement.ForceToApply = _knockBack * transform.forward;
            }
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        // TODO Fall off?
        ObjectPoolManager.Instance.DespawnObject(this.gameObject);
    }
}
