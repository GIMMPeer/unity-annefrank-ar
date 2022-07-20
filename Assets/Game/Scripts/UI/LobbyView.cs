using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;

using UnityEngine.UI;
public class LobbyView : View
{

    [SerializeField]
    private TextMeshProUGUI usernameText;

    [SerializeField]
    private Button toggleReadyButton;
    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    [SerializeField]
    private TextMeshProUGUI playerList;


    public override void Initialize()
    {
        base.Initialize();

        toggleReadyButton.onClick.AddListener(() => Player.Instance.IsReady = !Player.Instance.IsReady);
        //Player.Instance.SetUsername();

        if (InstanceFinder.IsServer)
        { 
            startGameButton.onClick.AddListener(() => GameManager.Instance.StartGame());
        }
        else 
        {
            startGameButton.gameObject.SetActive(false);
        }
    }

    
    private void LateUpdate()
    {
        if (!IsInitialized) return;

      

        usernameText.text = $" Username : {Player.Instance.Username}";


        string playerListText = "";


        for (var i = 0; i < GameManager.Instance.players.Count; i++)
        {
            Player currentPlayer = GameManager.Instance.players[i];

            playerListText += $"\r\n {currentPlayer.Username} (Is Ready: {currentPlayer.IsReady})" ;
        }

        playerList.text = playerListText;

        readyButtonText.color = Player.Instance.IsReady ? Color.green : Color.red;

        if (InstanceFinder.IsHost)
        {
        }
        else
        {

        }
    }
}
