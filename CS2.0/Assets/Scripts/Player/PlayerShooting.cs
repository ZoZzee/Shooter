using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Weapon[] weapons;
    public int currentWeapon;
    public KeyCode[] keysToSwitch;

    public GameObject particle;
    
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        SwithCheck();
        Shoot();
    }

    public void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, weapons[currentWeapon].distance)) 
            { 
                Transform newParticle = Instantiate(particle,hit.point, Quaternion.identity, null).transform;
                newParticle.LookAt(Camera.main.transform.position);
            }
                playerController.handAnimator.CrossFade("Shoot", 0.08f);
                
        }
    }

    private void SwithCheck()
    {
        int weaponBeforeCheck = currentWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentWeapon++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentWeapon--;
        }

        if (currentWeapon == weapons.Length)
        {
            currentWeapon = 0;
        }
        else if (currentWeapon == -1)
        {
            currentWeapon = weapons.Length - 1;
        }

        for (int i = 0; i < keysToSwitch.Length; i++)
        {
            if (Input.GetKeyDown(keysToSwitch[i]))
            {
                currentWeapon = i;
                break;
            }
        }


        if(currentWeapon != weaponBeforeCheck)
        {
            SwitchWeapon();
        }

    }

    private void SwitchWeapon()
    {
        for (int i = 0; i < weapons.Length ; i++ )
        {
            if ( i != currentWeapon)
            {
                weapons[i].gameObject.SetActive(false);
            }
            else
            {
                weapons[currentWeapon].gameObject.SetActive(true);
            }
        }
        
    }
}
