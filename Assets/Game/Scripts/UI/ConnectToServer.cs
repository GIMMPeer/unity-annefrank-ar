using FishNet;
using UnityEngine;
using UnityEngine.UI;

public sealed class ConnectToServer : View
{
    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button connectButton;

    private void Start()
    {
        InstanceFinder.ServerManager.StartConnection();

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