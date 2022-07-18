using FishNet;
using TMPro;
using UnityEngine;

public class MainView : View
{
    [SerializeField]
    private TextMeshProUGUI infoText;

    [SerializeField]
    private TextMeshProUGUI usernameText;

    [SerializeField]
    private TextMeshProUGUI playerCountText;

    private void LateUpdate()
    {
        if (!IsInitialized) return;

        infoText.text = $"Is Server = {InstanceFinder.IsServer}, Is Client = {InstanceFinder.IsClient}, Is host = {InstanceFinder.IsHost}";

        usernameText.text = $" Username : {Player.Instance.Username}";


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
