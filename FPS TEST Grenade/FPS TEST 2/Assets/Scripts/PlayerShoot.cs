using Mirror ;
using UnityEngine;
[RequireComponent(typeof( WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    private PlayerWeapon currentweapon;
    private WeaponManager weaponManager;

   [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
    void Start()
    {
        if( cam == null)
        {
            Debug.LogError("Pas de camera renseignée sur le systeme de tir.");
            this.enabled = false;

        }

        weaponManager = GetComponent<WeaponManager>();
    }

     private void Update()   
     
    {
        currentweapon = weaponManager.getCurrentWeapon();

        if (PauseMenu.isOn)
        {
            return;
        }
        if(currentweapon.ammo < currentweapon.maxAmmos){
            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponManager.Reload();
                return;
            }
        }

        if (currentweapon.fireRate <=0f)
        {
            
        
            if(Input.GetButtonDown("Fire1"))
            {
            
                Shoot();
            }
        }else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot",0f,1f/currentweapon.fireRate);
            }else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }

            
        }
    }

    [Command]
    void CmdOnHit(Vector3 pos,Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }
    

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffects();
    }

    [ClientRpc]
    void RpcDoShootEffects()
    {
        weaponManager.getCurrentGraphics().muzzelFlash.Play();
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos , Vector3 normal)
    {
       GameObject hitEffect = Instantiate(weaponManager.getCurrentGraphics().hitEffectPrefab, pos , Quaternion.LookRotation(normal));
        Destroy(hitEffect, 1f);
    }

    [Client]
    private void Shoot()
    {
        if(!isLocalPlayer && weaponManager.isReloading)
        {
            return;
        }

        if (currentweapon.ammo <=0)
        {
            weaponManager.Reload();
            return;
        }

        currentweapon.ammo -= 1;

        Debug.Log("ammo left :" + currentweapon.ammo);

        CmdOnShoot();

        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentweapon.range, mask))
        {
            
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name);
            }
            else
            {
                if (hit.collider.tag == "Tourelle")
                {
                    CmdPlayerShotTurret(hit.collider.name);
                }
            }

            CmdOnHit(hit.point , hit.normal);

        }
    }
    [Command]
    private void CmdPlayerShot(string playerId )
    {
        Debug.Log( playerId + "a été touché");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(currentweapon.damage);
    }
    [Command]
    private void CmdPlayerShotTurret(string turretId)
    {
        Debug.Log("toucher tourelle : " + turretId);
        //turretId.SendMessage(TakeDamage, currentweapon.damage);
       
       

    }
   
}
