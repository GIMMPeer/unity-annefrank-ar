using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public sealed class Player : NetworkBehaviour
{
    public static Player Instance { get; private set; }


    [field: SerializeField]
    [field: SyncVar]
    public string Username
    {
        get;

        [ServerRpc]
        private set;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        Instance = this;

        UIManager.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Username = "Player_000001";
        }
    }
}
