using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreUpdate : NetworkBehaviour
{
    [SerializeField]
    int player1score = 0;
    NetworkVariable<int> networkScore1 = new NetworkVariable<int>(0);
    NetworkVariable<int> networkScore2 = new NetworkVariable<int>(0);
    [SerializeField]
    int player2score = 0;
    [SerializeField]
    TextMeshProUGUI score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        score.SetText(networkScore1.Value + " - " + networkScore2.Value);
    }

    public void incrementP1score()
    {
        player1score += 1;
        networkScore1.Value += 1;
    }

    public void incrementP2score()
    {
        player2score += 1;
        networkScore2.Value += 1;
    }
}
