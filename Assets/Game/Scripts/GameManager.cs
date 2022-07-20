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

    [field: SerializeField]
    [field: SyncVar]
    public bool CanStart { get; private set; }


    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        CanStart = players.All(player => player.IsReady);
    }

    [Server]
    public void StartGame()
    {
        if (CanStart)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].StartGame();
            }
        }
    }

    [Server]
    public void StopGame()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].StopGame();
        }
    }

}
