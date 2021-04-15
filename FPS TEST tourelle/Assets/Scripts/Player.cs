using Mirror;
using UnityEngine;
using System.Collections;
[RequireComponent(typeof (PlayerSetup))]
public class Player : NetworkBehaviour
{
 private bool _isDead = false;
    public bool isDead
    {
        get{return _isDead;}
        protected set{ _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;
    public float GetHeathPct(){
        return (float) currentHealth/maxHealth;
    }

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;
    private bool[] wasEnableOnStart;

    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject spawnEffect;
    private bool firstSetup =true;

    public void Setup() 
    {
       CmdBroadcastPlayerSetup();
       if(isLocalPlayer){
        GameManager.instance.SetSceneCameraActive(false);
        GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
    }

    [Command(ignoreAuthority = true)]
    private void CmdBroadcastPlayerSetup(){
        RpcSetupPLayerOnAllClients();
    }
    [ClientRpc]
    private void RpcSetupPLayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnableOnStart = new bool[disableOnDeath.Length];
            for(int i = 0; i< disableOnDeath.Length; i++)
            {
                wasEnableOnStart[i] = disableOnDeath[i].enabled;
                
            }
            firstSetup =false;

        }
        

        SetDefault();
    }

    private void SetDefault()
    {
        currentHealth = maxHealth;
        isDead = false ; 
        for(int i = 0; i<disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnableOnStart[i];
        }
        for(int i=0; i<disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
            
        
            
        }
        Collider col =GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
        }
         GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(0.1f);

        Setup();
        
    }
    
    private void Update(){
        if(!isLocalPlayer){
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
    }
    [Client]
    public void RpcTakeDamage(float amount)
    {   
        if(isDead){
            return;
        }
        currentHealth-= amount;
        Debug.Log(transform.name + " a maintenant : " + currentHealth+ " PV");
        if(currentHealth <= 0)
        {
            Die();
        }
    }

     private void Die()
    {
        isDead = true;

        for(int i=0; i<disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
            
        }
        for(int i=0; i<disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
            
        }

        Collider col =GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = false;
            
        }
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + "est éliminé");
        StartCoroutine(Respawn());
    }
}
