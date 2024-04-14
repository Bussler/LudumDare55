using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSystem", menuName = "GunSystem/Gun", order = 1)]
public class Gun : ScriptableObject
{
    public enum GunType
    {
        basic,
        pistol,
        assaultRifle,
        sniper,
        shotgun,
        gatlingGun,
        rocketLauncher,
        flameThrower,
        lazerGun
    }

    public GunType type;

    public int Damage;

    public float FireRate;

    [Range(50f, 100f)]
    public float Accuracy;

    public float TimeLimit;

    public float Range;

    public int BulletAmount;

    public float BulletSpeed;

    public float BulletSize;

    public float BulletKnockback;

    public int BulletHealth;

    public int LifeSteal;


    public GameObject Projectile;


    public List<ObjectEffect> Effects;

   

    public Gun CopyThis()
    {
        Gun newGun = new Gun();
        newGun.type= type;
        newGun.Damage= Damage;
        newGun.FireRate = FireRate;  
        newGun.Accuracy = Accuracy;
        newGun.TimeLimit = TimeLimit;
        newGun.Range = Range;
        newGun.BulletAmount = BulletAmount;
        newGun.BulletSpeed = BulletSpeed;
        newGun.BulletSize = BulletSize;
        newGun.BulletKnockback = BulletKnockback;
        newGun.BulletHealth = BulletHealth;
        newGun.Projectile = Projectile;
        newGun.Effects = Effects;

        return newGun;

    }




}
