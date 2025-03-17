using Unity.Netcode;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MultiplayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefabA; //add prefab in inspector
    [SerializeField] private GameObject playerPrefabB; //add prefab in inspector
    bool spawned = false;
    //[ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    public override void OnNetworkSpawn()
    {
        GameObject gameObject;
        GameObject gameObject2;

        /*if (IsHost)
        {
            gameObject = Instantiate(playerPrefabA);
            NetworkObject netObj = gameObject.GetComponent<NetworkObject>();
            gameObject.SetActive(true);
            //netObj.SpawnAsPlayerObject(clientId, true);
        }
        else
        {
            gameObject= Instantiate(playerPrefabB);
            NetworkObject netObj = gameObject.GetComponent<NetworkObject>();
            gameObject.SetActive(true);
            //netObj.SpawnAsPlayerObject(clientId, true);
        }*/

        // spawn both player prefabs

        if (!spawned)
        {
            gameObject = Instantiate(playerPrefabA);
            NetworkObject netObj = gameObject.GetComponent<NetworkObject>();
            gameObject.SetActive(true);
            gameObject2 = Instantiate(playerPrefabB);
            NetworkObject netObj2 = gameObject2.GetComponent<NetworkObject>();
            gameObject2.SetActive(true);
        }
    }
}