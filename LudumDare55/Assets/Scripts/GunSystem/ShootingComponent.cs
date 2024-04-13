using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingComponent : MonoBehaviour
{

    private PlayerStatManager statManager = PlayerStatManager.instance;

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
        EquipGun();
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
        Vector3 lookDir = _shootDir;
        lookDir.y = 0;
        this.transform.forward = lookDir;
    }


    private void OnShootingStarted(InputAction.CallbackContext context)
    {
        _isShooting = true;
        Shoot();
    }

    private void OnShootingCanceled(InputAction.CallbackContext context)
    {
        _isShooting = false;
      //  CancelInvoke();
    }

    public void EquipGun(IEnumerator<Gun.GunEffect> effects = null)
    {
        GenerateNewGun(effects);
        Invoke("DequipGun", currentGun.TimeLimit);
    }

    public void DequipGun()
    {
        currentGun = null;
        EquipGun(); //TODO remove
    }

    private void GenerateNewGun(IEnumerator<Gun.GunEffect> effects= null)
    {
        currentGun = gunTemplates[Random.Range(0,gunTemplates.Count)];

        while(currentGun.type == lastGunType)
        {
            currentGun = gunTemplates[Random.Range(0, gunTemplates.Count)];
        }

        lastGunType = currentGun.type;
    }

    private void Shoot()
    {
        int bulletAmt = currentGun.BulletAmount;
        if(currentGun != null&&_isShooting){
            for (int i = 0; i < bulletAmt; i++)
            {
                GameObject p = Instantiate(currentGun.Projectile, ShootingStartPoint.transform.position, Quaternion.identity);
                p.GetComponent<Projectile>().InitProjectile(currentGun.Damage, currentGun.BulletSpeed, currentGun.BulletKnockback, currentGun.Range, currentGun.BulletSize);
                p.transform.forward = _shootDir;
                float acc = (100 - currentGun.Accuracy) / 2;
                p.transform.Rotate(Vector3.up, Random.Range(-acc, acc));
            }
            if (_isShooting)
            {
                Invoke("Shoot", 1 / currentGun.FireRate);
            }
        }
    }

}
