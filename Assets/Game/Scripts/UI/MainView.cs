using FishNet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MainView : View
{
    [SerializeField]
    private TextMeshProUGUI infoText;

    [SerializeField]
    private TextMeshProUGUI usernameText;

    [SerializeField]
    private TextMeshProUGUI playerCountText;

    [SerializeField]
    private TextMeshProUGUI playerList;


    public override void Initialize()
    {
        base.Initialize();

        //Player.Instance.SetUsername();
    }

    private void LateUpdate()
    {
        if (!IsInitialized) return;

        infoText.text = $"Is Server = {InstanceFinder.IsServer}, Is Client = {InstanceFinder.IsClient}, Is host = {InstanceFinder.IsHost}";

        usernameText.text = $" Username : {Player.Instance.Username}";

        playerList.text = $" Players : ";

        for (var i = 0; i <= InstanceFinder.ServerManager.Clients.Count; i++)
        {
            var currentPlayer = InstanceFinder.ServerManager.Clients[i];
            //Trying to get the players usernames? 
        }
       

        if (InstanceFinder.IsHost)
        {
            playerCountText.text = $"Players = {InstanceFinder.ServerManager.Clients.Count}";


            playerCountText.gameObject.SetActive(true);
        }
        else 
        { 
            playerCountText.gameObject.SetActive(false);
        }

        
        
        
    }
}
