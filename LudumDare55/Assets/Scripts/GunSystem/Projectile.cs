using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _size;
    private float _speed;
    private int _damage;
    private float _range;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
    }


    public void InitProjectile(int damage, float speed, float range, float size)
    {
        _damage = damage;
        _speed = speed;
        _range = range;
        _size = size;
    }

    public void MoveForward()
    {
        this.transform.Translate(Vector3.forward*Time.fixedDeltaTime, Space.Self);
        _range -= Time.fixedDeltaTime;

        if( _range < 0)
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        //TODO
        //Destroy?
        //Fall off?
    }

}
