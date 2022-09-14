using FishNet;
using UnityEngine;
using UnityEngine.UI;

public sealed class ConnectToServer : View
{
    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button connectButton;

    public bool isServer;

   
    private void Start()
    {
        if (isServer)
        {
            InstanceFinder.ServerManager.StartConnection();
            //InstanceFinder.ClientManager.StartConnection();
        }
        else
        {
            hostButton.gameObject.SetActive(false);
        }
        connectButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());
    }

    public override void Show(object args = null)
    {
        if (args is string message)
        {
            Debug.Log(message);
        }
        base.Show(args);
    }
}