using UnityEngine;

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
