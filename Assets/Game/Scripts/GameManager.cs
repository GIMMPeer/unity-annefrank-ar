using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine;
using FishNet.Connection;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private struct playerContainer
    {
        public string PlayerName;
    }

    [SyncObject]
    public readonly SyncList<Player> players = new();

    
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        

    }
}
