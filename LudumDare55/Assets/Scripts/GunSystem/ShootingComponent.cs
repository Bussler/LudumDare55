using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingComponent : MonoBehaviour
{

    private PlayerStatManager  _statManager;

    public Gun basicGun;
    public List<Gun> gunTemplates;

    public Gun currentGun;

    public Transform ShootingStartPoint;
    private Vector3 _shootDir = Vector3.zero;

    public GameObject player; //TODO change this;

    private MainControls _input = null;

    private bool _canShoot = true;

    private bool _isShooting = false;

    private Gun.GunType lastGunType;

    public GameObject GunObject;
    private MeshRenderer gunMesRenderer;
    private MeshFilter gunMeshFilterer;

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
        currentGun = basicGun;
        _statManager = PlayerStatManager.Instance;
        ShootingStartPoint = GunObject.transform;
        gunMeshFilterer= GunObject.GetComponent<MeshFilter>();
        gunMesRenderer= GunObject.GetComponent<MeshRenderer>();
        SetGunMesh();
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

    public Gun EquipGun(IEnumerable<ObjectEffect> effects = null)
    {
        Gun g= GenerateNewGun(effects);

        foreach (ObjectEffect effect in effects)
        {
            if (effect.GetType().IsSubclassOf(typeof(GunEffect)))
            {
                effect.ApplyEffect(g);
            }

            if (effect.GetType().IsSubclassOf(typeof(PlayerEffects)))
            {
                effect.ApplyEffect(_statManager);
            }

            //TODO add enemyEffects
        }
        Invoke("DequipGun", currentGun.TimeLimit);

        SetGunMesh();
        return g;
    }

    public void DequipGun()
    {
        foreach (ObjectEffect effect in currentGun.Effects)
        {
            if (effect.GetType().IsSubclassOf(typeof(GunEffect)))
            {
                effect.RemoveEffect(currentGun);
            }

            if (effect.GetType().IsSubclassOf(typeof(PlayerEffects)))
            {
                effect.RemoveEffect(_statManager);
            }

            //TODO add enemyEffects
        }

        currentGun = basicGun;
        SetGunMesh();
    }

    public void SetGunMesh()
    {
        for(int i=0; i <GunObject.transform.childCount; i++)
        {
            Destroy(GunObject.transform.GetChild(i).gameObject);
        }

        GunObject.transform.localScale = currentGun.gunSize;
        gunMesRenderer.material = currentGun.material;
        gunMeshFilterer.mesh = currentGun.mesh;
        foreach (ObjectEffect effect in currentGun.Effects)
        {
            Instantiate(effect.ParticleSystem,GunObject.transform.position,GunObject.transform.rotation,GunObject.transform);
        }
    }

    private Gun GenerateNewGun(IEnumerable<ObjectEffect> effects= null)
    {
        currentGun = gunTemplates[Random.Range(0,gunTemplates.Count)].CopyThis();

        while(currentGun.type == lastGunType)
        {
            currentGun = gunTemplates[Random.Range(0, gunTemplates.Count)].CopyThis();
        }
        currentGun.Effects = effects.ToList();
        
        lastGunType = currentGun.type;
        return currentGun;
    }

    private void Shoot()
    {
        if(currentGun != null&&_isShooting&&_canShoot){
            int bulletAmt = currentGun.BulletAmount;
            for (int i = 0; i < bulletAmt; i++)
            {
                _canShoot = false;

                GameObject p = ObjectPoolManager.Instance.SpawnObject(currentGun.Projectile, ShootingStartPoint.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Bullet);
                p.GetComponent<Projectile>().InitProjectile(currentGun.Damage, currentGun.BulletSpeed, currentGun.BulletKnockback, currentGun.Range, currentGun.BulletSize,currentGun.BulletHealth);
                p.layer = LayerMask.NameToLayer("PlayerProjectile");
                p.transform.forward = _shootDir;

                float acc = (100 - currentGun.Accuracy) / 2;
                p.transform.Rotate(Vector3.up, Random.Range(-acc, acc));

                if (currentGun.LifeSteal > 0)
                {
                    _statManager.Heal(currentGun.LifeSteal);
                }
                else if(currentGun.LifeSteal < 0)
                {
                    _statManager.TakeDamage(currentGun.LifeSteal);
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
