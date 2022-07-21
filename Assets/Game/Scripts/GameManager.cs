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
    public readonly SyncList<Player> players = new SyncList<Player>();

    [SyncObject]
    public readonly SyncList<Group> groups = new SyncList<Group>();

    [field: SerializeField]
    [field: SyncVar]
    public bool CanStart { get; private set; }

    public bool ReadyNextRound { get; private set; }
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
        ReadyNextRound = players.All(player => player.HasVoted);
        if (ReadyNextRound)
        {
            NextRound();

            for (int i = 0; i < players.Count; i++)
            {
                players[i].HasVoted = false;
            }
        }
    }




    //public readonly SyncList<Player> players = new SyncList<Player>();
    [Server]
    public void CreateAndAssignGroups()
    {
        Debug.Log("Inside ");
        int numGroups = 0;
        int numPlayers = players.Count;
        int groupNumber = 0;
        /*
         * Create Groups based on number of players
         */
        if (numPlayers > 0 && numPlayers <= 8)
        {
            numGroups = 2;
        }
        else if(numPlayers <= 16)
        {
            numGroups = 4;
        }
        else if(numPlayers <= 32)
        {
            numGroups = 8;
        }
        else
        {
            Debug.Log("Bad Error Message GameManager 76");
        }
        /*
        for(int i = 0; i < numGroups; i++)
        {
            groups.Add(new Group());
        }
        */
        for(int i = 0; i < numPlayers; i++)
        {
            if (groupNumber == numGroups + 1)
            {
                groupNumber = 0;
            }

            players[i].GroupNumber = groupNumber;
            groupNumber++;
            
        }
        ShowLobbyView();
    }

    [Server]
    public void ShowLobbyView()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].ShowLobby = true;
        }
    }

    [Server]
    public void StartGame()
    {
        
        if (CanStart)
        {
            for (int i = 0; i < players.Count; i++)
            {
                //players[i].StartGame();
                players[i].R1Start = true;
            }
        }
    }
    [Server]
    public void NextRound()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].HasVoted = false;
            players[i].R1End = true;
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
