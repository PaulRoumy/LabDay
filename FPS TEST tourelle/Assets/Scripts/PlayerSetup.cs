using UnityEngine;
using Mirror;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    [SerializeField]
    private string dontDrawName = "dontDraw";

    [SerializeField]
    private GameObject playerGraphic; 

    [SerializeField]
    private GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;

    

    private void Start() 
    {
        if (!isLocalPlayer)
        {
            
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            

            Util.SetLayerRecursively(playerGraphic, LayerMask.NameToLayer(dontDrawName));

            playerUIInstance = Instantiate(playerUIPrefab);

            PlayerUI ui= playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {

            }
            else
            {
                ui.SetPlayer(GetComponent<Player>());
            }
             GetComponent<Player>().Setup();
        }

       
        
    }

    

   public override void OnStartClient()
   {
       base.OnStartClient();

       string netId = GetComponent<NetworkIdentity>().netId.ToString();
       Player player = GetComponent<Player>();
       GameManager.RegisterPlayer(netId ,player);
   }

    private void DisableComponents()
    {
        for (int i = 0; i< componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable() 
    {

        Destroy(playerUIInstance);
        if (isLocalPlayer)
        {
        GameManager.instance.SetSceneCameraActive(true);
        
        GameManager.UnregisterPlayer(transform.name);
        }
        
    }
}
