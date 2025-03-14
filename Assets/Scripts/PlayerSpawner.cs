/*using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject player1;

    [SerializeField]
    GameObject player2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(player1);
        Instantiate(player2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/

using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{

    [SerializeField]
    GameObject player1;

    [SerializeField]
    GameObject player2;

    [SerializeField]
    GameObject newp2;

    public NetworkVariable<Vector3> Position;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            if (!(GameObject.FindGameObjectWithTag("Player1"))) {
                var instance = Instantiate(player1);
                var instanceNetworkObject = instance.GetComponent<NetworkObject>();
                instanceNetworkObject.Spawn();

                var instance2 = Instantiate(newp2);
                var instanceNetworkObject2 = instance2.GetComponent<NetworkObject>();
                instanceNetworkObject2.Spawn();
            }
            else {
                
            }

            /*var instance2 = Instantiate(player2);
            var instanceNetworkObject2 = instance2.GetComponent<NetworkObject>();
            instanceNetworkObject2.Spawn();*/
        }

    }
}