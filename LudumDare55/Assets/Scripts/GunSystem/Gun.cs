using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunType", menuName = "GunType/Gun", order = 1)]
public class Gun : ScriptableObject
{

    public int Damage;

    public float FireRate;

    [Range(50f, 100f)]
    public float Accuracy;

    public float TimeLimit;

    public float Range;

    public int BulletAmount;

    public float BulletSpeed;

    public float BulletSize;


    public GameObject Projectile;


    public enum GunEffects
    {
        piercing,
        lifesteal,
        slowness,
        lifegive,
        extraDamage,
        extraFireRate
    }

    public List<GunEffects> Effects;

    private void GenerateGun(List<GunEffects> Effects)
    {

    }


    

}
