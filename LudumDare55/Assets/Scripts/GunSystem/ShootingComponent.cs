using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingComponent : MonoBehaviour
{

    private PlayerStatManager  _statManager;

    public List<Gun> gunTemplates;

    public Gun currentGun;

    public Transform ShootingStartPoint;
    private Vector3 _shootDir = Vector3.zero;

    public GameObject player; //TODO change this;

    private MainControls _input = null;

    private bool _canShoot = true;

    private bool _isShooting = false;

    private Gun.GunType lastGunType;

    private void Awake()
    {
        _input = new MainControls();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.shoot.shoot.started += OnShootingStarted;
        _input.shoot.shoot.canceled += OnShootingCanceled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.shoot.shoot.started -= OnShootingStarted;
        _input.shoot.shoot.canceled -= OnShootingCanceled;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Gun.GunEffect> list= new List<Gun.GunEffect>();
        Gun.GunEffect g = Gun.GunEffect.lifegive;
        list.Add(g);
        EquipGun(list);
        _statManager = PlayerStatManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ShootTargetPos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ShootTargetPos = hit.point;
            ShootTargetPos.y = 0;
        }

        _shootDir = ShootTargetPos - player.transform.position;
        _shootDir.y=0;
        
        this.transform.forward = _shootDir;
    }


    private void OnShootingStarted(InputAction.CallbackContext context)
    {
        _isShooting = true;
        Shoot();
    }

    private void OnShootingCanceled(InputAction.CallbackContext context)
    {
        _isShooting = false;
    }

    public void EquipGun(IEnumerable<Gun.GunEffect> effects = null)
    {
        GenerateNewGun(effects);
        Invoke("DequipGun", currentGun.TimeLimit);
    }

    public void DequipGun()
    {
        currentGun = null;
        List<Gun.GunEffect> list = new List<Gun.GunEffect>();
        Gun.GunEffect g = Gun.GunEffect.lifegive;
        list.Add(g);
        EquipGun(list); //TODO remove
    }

    private void GenerateNewGun(IEnumerable<Gun.GunEffect> effects= null)
    {
        currentGun = gunTemplates[Random.Range(0,gunTemplates.Count)].CopyThis();

        while(currentGun.type == lastGunType)
        {
            currentGun = gunTemplates[Random.Range(0, gunTemplates.Count)].CopyThis();
        }
        currentGun.ApplyEffects(effects);
        
        lastGunType = currentGun.type;
    }

    private void Shoot()
    {
        int bulletAmt = currentGun.BulletAmount;
        if(currentGun != null&&_isShooting&&_canShoot){
            for (int i = 0; i < bulletAmt; i++)
            {
                _canShoot = false;
                GameObject p = Instantiate(currentGun.Projectile, ShootingStartPoint.transform.position, Quaternion.identity);
                p.GetComponent<Projectile>().InitProjectile(currentGun.Damage, currentGun.BulletSpeed, currentGun.BulletKnockback, currentGun.Range, currentGun.BulletSize,currentGun.BulletHealth);
                p.transform.forward = _shootDir;
                float acc = (100 - currentGun.Accuracy) / 2;
                p.transform.Rotate(Vector3.up, Random.Range(-acc, acc));


                //Apply effects 
                foreach (Gun.GunEffect effect in currentGun.Effects) 
                { 
                    if(effect == Gun.GunEffect.lifesteal)
                    {
                        _statManager.Heal(GunEffectConfig.LifeStealPerEffect);
                    }
                    if (effect == Gun.GunEffect.lifegive)
                    {
                        _statManager.TakeDamage(GunEffectConfig.LifeGivePerEffect);
                    }

                }
                Invoke("SetCanShoot",1/currentGun.FireRate);
            }
            if (_isShooting)
            {
                Invoke("Shoot", 1 / currentGun.FireRate);
            }
        }
    }

    public void SetCanShoot()
    {
        _canShoot = true;
    }

}
