using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine;
using FishNet.Connection;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }


    [SyncObject]
    public readonly SyncList<Player> players = new SyncList<Player>();

    /*
    [SyncObject]
    public readonly SyncList<Group> groups = new SyncList<Group>();
    */

    private int[] groupVotes = new int[4];


    [SyncObject]
    public readonly SyncList<int> groupScores = new SyncList<int>();


    [field: SerializeField]
    [field: SyncVar]
    public bool CanStart { get; private set; }

    public bool ReadyNextRound { get; private set; }
    // Start is called before the first frame update

    public bool r1Active { get; private set; }

    private int roundNum = 0;

    private void Awake()
    {
        Instance = this;
        groupScores.Add(0);
        groupScores.Add(0);
        groupScores.Add(0);
        groupScores.Add(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsServer) return;

        CanStart = players.All(player => player.IsReady);
        
    
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReadyCheck()
    {
        ReadyNextRound = players.All(player => player.HasVoted);
        if (ReadyNextRound)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].HasVoted = false;
            }

            CheckVotesAndAssignScore(1);
            NextRound();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void CheckVotesAndAssignScore(int roundNum)
    {
        

        print("Check Score");
        for (var i = 0; i < players.Count; i++)
        {

            var playerGroupNum = players[i].GroupNumber;

            switch (playerGroupNum)
            {
                case 0:
                    groupVotes[0] += players[i].VoteStatus;
                    Debug.Log("Group 1 Votes" + groupVotes[0]);
                    break;
                case 1:
                    groupVotes[1] += players[i].VoteStatus;
                    Debug.Log("Group 2 Votes" + groupVotes[1]);
                    break;
                case 2:
                    groupVotes[2] += players[i].VoteStatus;
                    break;
                case 3:
                    groupVotes[3] += players[i].VoteStatus;
                    break;
                default:
                    print("ERR: Outcast");
                    break;
            }
        }

        switch (roundNum)
        {
            case 1:

                
                if (groupVotes[0] >= 0 && groupVotes[1] >=  0) //Compete
                {
                    groupScores[0] += 2;
                    groupScores[1] += 2;
                    Debug.Log(groupScores[0]);
                    Debug.Log(groupScores[1]);
                }
                else if(groupVotes[0] < 0 && groupVotes[1] < 0) //Cooperate
                {
                    groupScores[0] += 1;
                    groupScores[1] += 1;
                    Debug.Log(groupScores[0]);
                    Debug.Log(groupScores[1]);
                }


                break;
            case 2:
               
                break;
            case 3:
                
                break;
            case 4:
               
                break;
            default:
                print("ERR: Outcast");
                break;
        }

        for (int i = 0; i < players.Count; i++)
        {
            players[i].VoteStatus = 0;
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
            Debug.Log("Bad Error Message GameManager 179");
        }
        /*
        for(int i = 0; i < numGroups; i++)
        {
            groups.Add(new Group());
        }
        */
        for(int i = 0; i < numPlayers; i++)
        {
            if (groupNumber == numGroups)
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
                r1Active = true;
                roundNum = 1;
            }
        }
    }

    [Server]
    public void NextRound()
    {
        for (int i = 0; i < players.Count; i++)
        {

            players[i].R1End = true;
            //roundNum += 1;
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
