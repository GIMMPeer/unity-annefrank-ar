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
    private TextMeshProUGUI teamOneList;

    [SerializeField]
    private TextMeshProUGUI teamTwoList;

    [SerializeField]
    private TextMeshProUGUI teamThreeList;

    [SerializeField]
    private TextMeshProUGUI teamFourList;

    [SerializeField]
    private TextMeshProUGUI[] teamTitles;


    public override void Initialize()
    {
        base.Initialize();

        toggleReadyButton.onClick.AddListener(() => Player.Instance.ServerSetIsReady(true));
        
        startGameButton.onClick.AddListener(() => Player.Instance.CallToReadyCheck());
       
    }

    
    private void LateUpdate()
    {
        if (!IsInitialized) return;

      

        usernameText.text = $" Username : {Player.Instance.Username}";


        string teamOne = "";
        string teamTwo = "";
        string teamThree = "";
        string teamFour = "";

        for (var i = 0; i < GameManager.Instance.players.Count; i++) {
            Player currentPlayer = GameManager.Instance.players[i];

            if (currentPlayer.GroupNumber == 0) {
                teamOne += $"\r\n {currentPlayer.Username}";
            }
            if (currentPlayer.GroupNumber == 1) {
                teamTwo += $"\r\n {currentPlayer.Username}";
            }
            if (currentPlayer.GroupNumber == 2) {
                teamThree += $"\r\n {currentPlayer.Username}";
            }
            if (currentPlayer.GroupNumber == 3) {
                teamFour += $"\r\n {currentPlayer.Username}";
            }
        }

        for (int i = 0; i < GameManager.Instance.numGroups; i++)
        {
            teamTitles[i].gameObject.SetActive(true);
        }
        teamOneList.text = teamOne;
        teamTwoList.text = teamTwo;
        teamThreeList.text = teamThree;
        teamFourList.text = teamFour;

        readyButtonText.color = Player.Instance.IsReady ? Color.green : Color.red;

       
    }
}
