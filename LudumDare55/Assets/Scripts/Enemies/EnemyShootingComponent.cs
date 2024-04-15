using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingComponent : MonoBehaviour
{

    public List<Gun> gunTemplates;

    public Gun equippedGun;

    public Transform ShootingStartPoint;
    private Projectile _projectile; // TODO do we need this gloabally?
    private bool _canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewGun();
        //_projectile = equippedGun.Projectile.GetComponent<Projectile>();
        //if (_projectile != null)
        //    _projectile.SetCollisionTag("Player");
    }

    private void GenerateNewGun(IEnumerable<ObjectEffect> effects = null)
    {
        equippedGun = gunTemplates[Random.Range(0, gunTemplates.Count)].CopyThis();
    }

    public void Shoot(Vector3 shootingDirection)
    {
        int bulletAmt = equippedGun.BulletAmount;
        if (equippedGun != null && _canShoot)
        {
            for (int i = 0; i < bulletAmt; i++)
            {
                _canShoot = false;

                // calculate quaternion to look at direction
                float angle = Mathf.Atan2(shootingDirection.z, -shootingDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.up);


                GameObject p = ObjectPoolManager.Instance.SpawnObject(equippedGun.Projectile, ShootingStartPoint.transform.position, rotation, ObjectPoolManager.PoolType.EnemyBullet);
                p.GetComponent<Projectile>().InitProjectile(equippedGun.Damage, equippedGun.BulletSpeed, equippedGun.BulletKnockback, equippedGun.Range, equippedGun.BulletSize, equippedGun.BulletHealth);
                p.layer = LayerMask.NameToLayer("EnemyProjectile");
                //p.transform.forward = transform.forward;
                float acc = (100 - equippedGun.Accuracy) / 2;
                p.transform.Rotate(Vector3.up, Random.Range(-acc, acc));

             
            }
            Invoke("SetCanShoot", 1 / equippedGun.FireRate);
        }
    }

    public void ShootMultipleTimes(Vector3[] shootingDirections)
    {
        Debug.Log("TEST canshoot " + _canShoot);
        if (_canShoot)
        {
            _canShoot = false;
            for (int l = 0; l < shootingDirections.Length; l++)
            {
               
                if (equippedGun != null )
                {
                    int bulletAmt = equippedGun.BulletAmount;
                    for (int i = 0; i < bulletAmt; i++)
                    {
                        // calculate quaternion to look at direction
                        float angle = Mathf.Atan2(shootingDirections[l].z, -shootingDirections[l].x) * Mathf.Rad2Deg;
                        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.up);

                        GameObject p = ObjectPoolManager.Instance.SpawnObject(equippedGun.Projectile, ShootingStartPoint.transform.position, rotation, ObjectPoolManager.PoolType.EnemyBullet);
                        p.GetComponent<Projectile>().InitProjectile(equippedGun.Damage, equippedGun.BulletSpeed, equippedGun.BulletKnockback, equippedGun.Range, equippedGun.BulletSize, equippedGun.BulletHealth);
                        p.layer = LayerMask.NameToLayer("EnemyProjectile");
                        //p.transform.forward = transform.forward;
                        float acc = (100 - equippedGun.Accuracy) / 2;
                        p.transform.Rotate(Vector3.up, Random.Range(-acc, acc));
                    }
                }
            }
            Invoke("SetCanShoot", 1 / equippedGun.FireRate);
        }
       
       
    }


    public void SetCanShoot()
    {
        _canShoot = true;
    }
}
