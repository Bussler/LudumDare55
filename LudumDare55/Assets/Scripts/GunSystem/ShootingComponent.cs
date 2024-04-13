using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingComponent : MonoBehaviour
{

    private PlayerStatManager statManager= PlayerStatManager.instance;

    public Gun currentGun;

    public Transform ShootingStartPoint;
    private Vector3 _shootDir = Vector3.zero;

    public GameObject player; //TODO change this;

    private MainControls _input = null;

    private bool _canShoot= true;

    private void Awake()
    {
        _input = new MainControls();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.shoot.shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        _input.Disable();
       // _input.shoot.shoot.performed -= OnShoot;
    }

    // Start is called before the first frame update
    void Start()
    {
       
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

        _shootDir = ShootTargetPos-player.transform.position;
    }


    private void OnShoot(InputAction.CallbackContext context)
    {
       
            Debug.Log("TEST SHOOT");
            if (_canShoot)
            {
                int bulletAmt = currentGun.BulletAmount;
                _canShoot = false;
                for (int i = 0; i < bulletAmt; i++)
                {
                    GameObject p = Instantiate(currentGun.Projectile, ShootingStartPoint.transform.position, Quaternion.identity);
                    p.GetComponent<Projectile>().InitProjectile(currentGun.Damage, currentGun.BulletSpeed, currentGun.Range, currentGun.BulletSize);
                    p.transform.forward = _shootDir;
                    p.transform.RotateAround(Vector3.up, Random.Range(-(100 - currentGun.Accuracy), 100 - currentGun.Accuracy));
                }
                Invoke("SetCanShoot", 1 / currentGun.FireRate);
            }
       
    }


    public void ClickHoldRelease(InputAction.CallbackContext context)
    {
        System.Type vector2Type = Vector2.zero.GetType();

        string buttonControlPath = "/Mouse/leftButton";
        //string deltaControlPath = "/Mouse/delta";

        Debug.Log(context.control.path);
        //Debug.Log(context.valueType);

        if (context.started)
        {
            
                Debug.Log("Button Pressed Down Event - called once when button pressed");
            
        }
        else if (context.performed)
        {
           
                Debug.Log("Button Hold Down - called continously till the button is pressed");
            
        }
        else if (context.canceled)
        {
            
                Debug.Log("Button released");
            
        }
    }

    private void SetCanShoot()
    {
        Debug.Log("TEST canShoot");
        _canShoot = true;
    }

}
