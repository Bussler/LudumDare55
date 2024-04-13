using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public int BulletHealth;


    public GameObject Projectile;




    public enum GunEffect
    {
        piercing,
        lifesteal,
        playerSlowness, //TODO
        enemySlowness,  //TODO
        lifegive,
        extraDamage,
        extraFireRate,
        extraRange,
        bulletSlowness,
        extraKnockBack
    }

    public List<GunEffect> Effects;

    public void ApplyEffects(IEnumerable<GunEffect> effects = null)
    {

        this.Effects = effects.ToList();

        foreach (GunEffect effect in effects)
        {
            switch (effect)
            {
                case GunEffect.piercing:
                    BulletHealth++;
                    break;
                case GunEffect.bulletSlowness:
                    BulletSpeed -= GunEffectConfig.BulletSlownessperEffect;
                    break;
                case GunEffect.extraDamage:
                    Damage += GunEffectConfig.ExtraDamagePerEffect;
                    break;
                case GunEffect.extraFireRate:
                    Damage += GunEffectConfig.ExtraFireRatePerEffect;
                    break;
                case GunEffect.extraRange:
                    Damage += GunEffectConfig.ExtraRangePerEffect;
                    break;
                case GunEffect.extraKnockBack:
                    Damage += GunEffectConfig.ExtraKnockBackPerEffect;
                    break;

            }
        }
    }

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
