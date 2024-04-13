using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingComponent : MonoBehaviour
{

    public List<Gun> gunTemplates;

    public Gun equippedGun;

    public Transform ShootingStartPoint;
    private Projectile _projectile;
    private bool _canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewGun();
        _projectile = equippedGun.Projectile.GetComponent<Projectile>();
        _projectile.SetCollisionTag("Player");
    }

    private void GenerateNewGun(IEnumerable<Gun.GunEffect> effects = null)
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
                GameObject p = Instantiate(equippedGun.Projectile, ShootingStartPoint.transform.position, Quaternion.identity);
                p.GetComponent<Projectile>().InitProjectile(equippedGun.Damage, equippedGun.BulletSpeed, equippedGun.BulletKnockback, equippedGun.Range, equippedGun.BulletSize, equippedGun.BulletHealth);
                p.transform.forward = transform.forward;
                float acc = (100 - equippedGun.Accuracy) / 2;
                p.transform.Rotate(Vector3.up, Random.Range(-acc, acc));

                Invoke("SetCanShoot", 1 / equippedGun.FireRate);
            }
        }
    }
    private void SetCanShoot()
    {
        _canShoot = true;
    }
}
