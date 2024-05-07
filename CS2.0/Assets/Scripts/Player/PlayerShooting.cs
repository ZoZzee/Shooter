using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public Weapon[] weapons;
    [HideInInspector] public int currentWeapon;
    public KeyCode[] keysToSwitch;

    [SerializeField] private GameObject particle;
    
    private PlayerController playerController;

    [SerializeField] private TMP_Text bulletsText;
    [SerializeField] private TMP_Text bulletsAllText;

    [SerializeField] private Image reloadingBar;

    [SerializeField] private Animator handAnimator;

    public bool isReloading;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        RefreshUI();
        
    }
    private void Update()
    {
        SwithCheck();
        Shoot();
    }

    private void FixedUpdate()
    {
        if(isReloading)
        {
            Reloading();
        }
    }
    private void LateUpdate()
    {
        handAnimator.SetBool("IsReloading", isReloading);
    }

    public void Shoot()
    {
        if(Input.GetMouseButtonDown(0) && weapons[currentWeapon].bullets > 0 && isReloading == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, weapons[currentWeapon].distance)) 
            { 
                Transform newParticle = Instantiate(particle,hit.point, Quaternion.identity, null).transform;
                newParticle.LookAt(Camera.main.transform.position);
            }
            playerController.handAnimator.CrossFade("Shoot", 0.08f);

            weapons[currentWeapon].bullets--;

            if (weapons[currentWeapon].bullets == 0 && weapons[currentWeapon].bulletAll > 0)
            {
                isReloading = true;
                Reloading();
            }

            RefreshUI();
        }
    }

    private void Reloading()
    {

        weapons[currentWeapon].reloading++;
        if(weapons[currentWeapon].reloading >= weapons[currentWeapon].reloadingMax)
        {
            ReloadingFinish();
            weapons[currentWeapon].reloading = 0;
        }
        reloadingBar.fillAmount = weapons[currentWeapon].reloading / weapons[currentWeapon].reloadingMax;
    }

    private void ReloadingFinish()
    {
        if (weapons[currentWeapon].bulletAll >= weapons[currentWeapon].bulletMax)
        {
            weapons[currentWeapon].bullets = weapons[currentWeapon].bulletMax;
            weapons[currentWeapon].bulletAll -= weapons[currentWeapon].bulletMax;
        }
        else
        {
            weapons[currentWeapon].bullets = weapons[currentWeapon].bulletAll;
            weapons[currentWeapon].bulletAll = 0;
        }
        isReloading = false;
        RefreshUI();
    }
    private void RefreshUI()
    {
        bulletsText.text = weapons[currentWeapon].bullets + "/" + weapons[currentWeapon].bulletMax;

        bulletsAllText.text = weapons[currentWeapon].bulletAll.ToString();

        if(isReloading == false)
        {
            float bulletsFloat = weapons[currentWeapon].bullets;
            float bulletsMaxFloat = weapons[currentWeapon].bulletMax;
            reloadingBar.fillAmount = bulletsFloat / bulletsMaxFloat;
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
        RefreshUI();

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
