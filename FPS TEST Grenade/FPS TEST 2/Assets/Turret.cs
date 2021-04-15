using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Turret : NetworkBehaviour
{
    [SerializeField]
    private float HP;
    private float currentHp;
    [SerializeField]
    GameObject dieParticul;
    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;
    [SerializeField]
    float range  =5f;
    float damage = 5;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Camera cam;
    private Transform target;
    [SerializeField]
    private Transform tourrel;
    private float speed= 5 ;

    float fireRate = 12f;
    float fireCountdown =1f;
    


    private void Start() {
        currentHp = HP;
        InvokeRepeating("UpdateTarget", 0f , 0.5f);
    }

    private void TakeDamage(float _damage)
    {
        currentHp =- _damage;
        if(currentHp <=0)
        {
            Die();
        }
    }

    private void Die(){
        Collider col = GetComponent<Collider>();
        Debug.Log(col);
        col.enabled = false;
        GameObject _gfxIns = Instantiate(dieParticul, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        for(int i=0; i<disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
            Debug.Log(disableGameObjectOnDeath[i]);
        }


    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Y))
            {
                TakeDamage(999);
            }
        if (Input.GetKeyDown(KeyCode.A))
            {
                Shoot();
            }
        if (target == null)
        {
            return;
        }
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation =Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(tourrel.rotation, lookRotation, Time.deltaTime* speed ).eulerAngles;
        tourrel.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown =1/fireRate;
        }
        fireCountdown -= Time.deltaTime;

    }

    void Shoot()
    {
       RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask))
        {
            
            if(hit.collider.tag == "Player")
            {
                CmdTurretShot(hit.collider.name);
            }
            

            CmdOnHit(hit.point , hit.normal);

        } 
    }
    

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position , range);
    }

    void UpdateTarget()
    {
        GameObject[] ennemies = GameObject.FindGameObjectsWithTag("Player");
        float nearestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in ennemies)
        {
            float distanceToPlayer= Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToPlayer < nearestDistance)
            {
                nearestDistance = distanceToPlayer;
                nearestEnemy = enemy;
            }
        }
        
        if (nearestEnemy != null && nearestDistance < range)
        {
                target = nearestEnemy.transform;
        }
        else
        {
            target =null;
        }
    
    }

    private void CmdTurretShot(string playerId )
    {
        Debug.Log( playerId + "a été touché");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);

    }
    [Command]
    void CmdOnHit(Vector3 pos,Vector3 normal)
    {
        
    }
    

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffects();
    }

    
    void RpcDoShootEffects()
    {
        
    }
    
        
    


}
