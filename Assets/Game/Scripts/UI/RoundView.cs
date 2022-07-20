using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;

public class RoundView : View
{

    [SerializeField]
    private TextMeshProUGUI leaderboardText;

    [SerializeField]
    private TextMeshProUGUI playerList;


    public override void Initialize()
    {
        base.Initialize();

    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    // Update is called once per frame
    void Update()
    {
        string playerListText = "";


        for (var i = 0; i < GameManager.Instance.players.Count; i++)
        {
            Player currentPlayer = GameManager.Instance.players[i];

            playerListText += $"\r\n {currentPlayer.Username} (Score: {currentPlayer.Score})";
        }

        playerList.text = playerListText;
    }
}
