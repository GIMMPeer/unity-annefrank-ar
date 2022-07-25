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

    
    [field: SyncVar]
    public int highestGroup { get; private set; }

    public bool r1Active { get; private set; }

    [field: SyncVar]
    public int roundNum { get; private set; } = 0;

    [field: SyncVar]
    public int viewNum { get; private set; } = 0;

    [field: SyncVar]
    public int numGroups { get; private set; }

    //[field: SyncVar]
    //public int highestGroup { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckVotes(Player player)
    {

        var playerGroupNum = player.GroupNumber;
        groupVotes[playerGroupNum] += player.VoteStatus;
        Debug.Log("Group 1 Votes" + groupVotes[playerGroupNum]);

    }

    [ServerRpc(RequireOwnership = false)]
    public void AssignScores()
    {
        switch (roundNum)
        {
            case 1:
                for (int i = 0; i < numGroups; i += 2)
                {
                    if (groupVotes[i] >= 0 && groupVotes[i + 1] >= 0) //Compete
                    {
                        groupScores[i] += 2;
                        groupScores[i + 1] += 2;
                        Debug.Log(groupScores[i]);
                        Debug.Log(groupScores[i + 1]);
                    }
                    else if (groupVotes[i] < 0 && groupVotes[i + 1] < 0) //Cooperate
                    {
                        groupScores[i] += 1;
                        groupScores[i + 1] += 1;
                        Debug.Log(groupScores[i]);
                        Debug.Log(groupScores[i + 1]);
                    }
                    else if (groupVotes[i] < 0 && groupVotes[i + 1] > 0)//higher team comp
                    {
                        groupScores[i] += 1;
                        groupScores[i + 1] += 4;
                    }
                    else if (groupVotes[i] > 0 && groupVotes[i + 1] < 0)//lower team comp
                    {
                        groupScores[i] += 4;
                        groupScores[i + 1] += 1;
                    }
                    else
                    {
                        Debug.Log("Error");
                    }
                }

                break;

            case 2:
                if (groupVotes[0] >= 0 && groupVotes[1] >= 0) //Compete
                {
                    groupScores[0] += 2;
                    groupScores[1] += 2;
                    Debug.Log(groupScores[0]);
                    Debug.Log(groupScores[1]);
                }
                else if (groupVotes[0] < 0 && groupVotes[1] < 0) //Cooperate
                {
                    groupScores[0] += 1;
                    groupScores[1] += 1;
                    Debug.Log(groupScores[0]);
                    Debug.Log(groupScores[1]);
                }
                else if (groupVotes[0] < 0 && groupVotes[1] > 0)
                {
                    groupScores[0] += 1;
                    groupScores[1] += 4;
                }
                else if (groupVotes[0] > 0 && groupVotes[1] < 0)
                {
                    groupScores[0] += 4;
                    groupScores[1] += 1;
                }
                else
                {
                    Debug.Log("Error");
                }
                break;
            case 3:

                break;
            case 4:

                break;
            default:
                print("ERR: Outcast");
                break;
        }
    }
   

     public void CheckHighest()
    {
        int highestScore = -1;
        highestGroup = -1;
        Debug.Log("list count: " + groupScores.Count);

        for (int i = 0; i < groupScores.Count; i++)
        {
            if (groupScores[i] > highestScore)
            {
                highestScore = groupScores[i];
                highestGroup = i;
            }

        }
        print("Highest group: " + highestGroup);
    }
        



    //public readonly SyncList<Player> players = new SyncList<Player>();
    [Server]
    public void CreateAndAssignGroups()
    {
        Debug.Log("Inside ");

        int numPlayers = players.Count;
        int groupNumber = 0;
        /*
         * Create Groups based on number of players
         */
        if (numPlayers > 0 && numPlayers <= 8)
        {
            numGroups = 2;
        }
        else if (numPlayers <= 16)
        {
            numGroups = 4;
        }
        else if (numPlayers <= 32)
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
        for (int i = 0; i < numGroups; i++)
        {
            groupScores.Add(0);
        }
        for (int i = 0; i < numPlayers; i++)
        {
            if (groupNumber == numGroups)
            {
                groupNumber = 0;
            }

            players[i].GroupNumber = groupNumber;
            groupNumber++;
        }
        viewNum = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsServer) return;

    }



    [ServerRpc(RequireOwnership = false)]
    public void ReadyCheck()
    {
        bool readyStartRound = players.All(player => player.IsReady);
        
            if (readyStartRound)
            {
                roundNum += 1;
                Debug.Log("Round Num: " + roundNum);
                viewNum += 1;
                ResetAll();
            }
        bool ReadyEndRound = players.All(player => player.HasVoted);
        if (ReadyEndRound)
        {
            for (int i = 0; i < players.Count; i++)
            {
                CheckVotes(players[i]);
            }
        AssignScores();
        CheckHighest();
        viewNum += 1;
        ResetAll();

        }
    }



    public void ResetAll()
    {
        //Is Ready 
        for (int i = 0; i < players.Count; i++)
        {
            players[i].IsReady = false;
            players[i].HasVoted = false;
            players[i].VoteStatus = 0;
        }

        for (int i = 0; i < numGroups; i++)
        {
            groupVotes[i] = 0;
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
