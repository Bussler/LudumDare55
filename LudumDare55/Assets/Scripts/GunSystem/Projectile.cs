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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
    }


    public void InitProjectile(int damage, float speed,float knockback, float range, float size)
    {
        _damage = damage;
        _speed = speed;
        _range = range;
        _size = size;
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


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().ApplyKnockback(_knockBack * transform.TransformDirection(transform.forward));
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
