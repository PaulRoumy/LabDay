using UnityEngine;

    


[System.Serializable]

public class PlayerWeapon 
{
    public string name = "Submachinegun";
    public float damage = 10f;
    public float range = 100f;
    public float fireRate =0f;
    public int maxAmmos = 30;
    public int ammo;

    public GameObject graphics;

    public float reloadTime = 2f;

    public PlayerWeapon()
    {
        ammo = maxAmmos;
    }

}
