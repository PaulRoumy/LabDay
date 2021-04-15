using Mirror;
using UnityEngine;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private string weaponLayerName ="Weapon";

    [SerializeField]
    private Transform weaponOlder ;

    public bool isReloading = false;

    void Start()
    {
       EquipWeapon(primaryWeapon); 
    }

    public PlayerWeapon getCurrentWeapon()
    {
        return currentWeapon;
    }

public WeaponGraphics getCurrentGraphics()
    {
        return currentGraphics;
    }

   void EquipWeapon(PlayerWeapon _weapon)
   {
        currentWeapon = _weapon;
        GameObject weaponints= Instantiate(_weapon.graphics , weaponOlder.position, weaponOlder.rotation);
        weaponints.transform.SetParent(weaponOlder);

        currentGraphics = weaponints.GetComponent<WeaponGraphics>();
        if(currentGraphics == null){
            Debug.LogError("Pas de WeaponGraphics sur l'arme : " + weaponints.name);
        }

        if (isLocalPlayer)
        {
            
            Util.SetLayerRecursively(weaponints,LayerMask.NameToLayer(weaponLayerName));
        }
   }

   public void Reload()
   {
       if(isReloading)
       {
           return;   
        }
        StartCoroutine(Reload_Coroutine());
   }

   public IEnumerator Reload_Coroutine(){
   
       isReloading = true;
       currentWeapon.ammo = 0;



       CmdOnReload();
        Debug.Log("Reloading ...");
        yield return new WaitForSeconds(2f);
        currentWeapon.ammo = currentWeapon.maxAmmos;



       isReloading = false;

   }

   [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload(){
        Animator anim= currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }


   
}
