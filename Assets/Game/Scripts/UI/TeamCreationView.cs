using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet;
public class TeamCreationView : View
{

    [SerializeField]
    private TextMeshProUGUI playerList;
    [SerializeField]
    private Button createTeamButton;
    [SerializeField]
    private TextMeshProUGUI usernameText;
    // Start is called before the first frame update
    public override void Initialize()
    {
        // Update is called once per frame
        if (InstanceFinder.IsServer)
        {
            createTeamButton.onClick.AddListener(() => GameManager.Instance.CreateAndAssignGroups());
        }
        else 
        {
            createTeamButton.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {

        usernameText.text = $" My Username : {Player.Instance.Username}";

        string playerListText = "";


        for (var i = 0; i < GameManager.Instance.players.Count; i++)
        {
            Player currentPlayer = GameManager.Instance.players[i];

            playerListText += $"\r\n {currentPlayer.Username} (IsReady: {currentPlayer.IsReady}) (groupNumber: {currentPlayer.GroupNumber})";
        }

        playerList.text = playerListText;
    }
}
