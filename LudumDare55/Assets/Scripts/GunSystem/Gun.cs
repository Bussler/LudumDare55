using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunType", menuName = "GunType/Gun", order = 1)]
public class Gun : ScriptableObject
{
    public enum GunType
    {
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


    public GameObject Projectile;




    public enum GunEffect
    {
        piercing,
        lifesteal,
        slowness,
        lifegive,
        extraDamage,
        extraFireRate,
        extraRange,

    }

    public List<GunEffect> Effects;

    public void ApplyEffects(IEnumerable<GunEffect> effects=null)
    {



    }


    

}
