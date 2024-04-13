using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().ApplyKnockback(_knockBack * transform.forward);

            _health--;
            if (_health <= 0)
            {
                DestroyProjectile();
            }
        }
    }

    private void DestroyProjectile()
    {
        //TODO
        //Destroy?
        //Fall off?
        Destroy(gameObject);
    }

}
