
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   

    private const string playersIdPrefix = "Player ";

    public MatchSettings matchSettings;
    public static GameManager instance ;
    
    [SerializeField]
    private GameObject sceneCamera;
    [SerializeField]
    

    private void Awake(){
        if(instance == null)
        {
            instance = this;
            return;
        }
        Debug.LogError("+ d'une instance de GameManager dans la scene");
    }

    public void SetSceneCameraActive( bool isActive)
    {
        if (sceneCamera == null)
        {
            return;
        }

        sceneCamera.SetActive(isActive);

    }

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public  static void RegisterPlayer(string netId , Player player)
    {
        string playerId = playersIdPrefix + netId;
        players.Add(playerId,player);
        player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }

    

    



   
}
