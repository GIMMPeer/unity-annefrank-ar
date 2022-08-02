using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }


    [SyncObject]
    public readonly SyncList<Player> players = new SyncList<Player>();

    private int[] groupVotes;


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

    [field: SyncVar]
    public bool discrimLaw { get; private set; } = false;

    [field: SyncVar]
    public bool speakUp1 {
        get; 
       
        private set; 
    }

    [field: SyncVar]
    public bool speakUp2 { get; private set; }

  

    private void Awake()
    {
        Instance = this;
    }

    
    void FixedUpdate() {
        if (!IsServer)
            return;
    }

    [Server]
    public void CreateAndAssignGroups() {
        int numPlayers = players.Count;
        int groupNumber = 0;
        // Create Groups based on number of players
        if (numPlayers > 0 && numPlayers <= 8) {
            numGroups = 2;
        } else if (numPlayers <= 16) {
            numGroups = 4;
        } else if (numPlayers <= 32) {
            numGroups = 8;
        } else {
            Debug.Log("Bad Error Message GameManager 179");
        }

        // Instantiate vote and score lists based off number of players
        groupVotes = new int[numPlayers];
        for (int i = 0; i < numGroups; i++) {
            groupScores.Add(0);
        }

        // Sort players into groups
        for (int i = 0; i < numPlayers; i++) {
            if (groupNumber == numGroups) {
                groupNumber = 0;
            }
            players[i].GroupNumber = groupNumber;
            groupNumber++;
        }
        viewNum = 1;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReadyCheck() {
        bool readyStartRound = players.All(player => player.IsReady);

        if (readyStartRound) {
            roundNum += 1;
            viewNum += 1;
            ResetAll();
        }
        bool ReadyEndRound = players.All(player => player.HasVoted);
        if (ReadyEndRound) {
            for (int i = 0; i < players.Count; i++) {
                CheckVotes(players[i]);
            }
            AssignScores();
            

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckVotes(Player player)
    {
        var playerGroupNum = player.GroupNumber;
        groupVotes[playerGroupNum] += player.VoteStatus;
    }

    [ServerRpc(RequireOwnership = false)]
    public void AssignScores()
    {
        Debug.Log("Round Number is " + roundNum);
        switch (roundNum) {
            case 1: //First Dilemma
                for (int i = 0; i < numGroups; i += 2) {
                    if (groupVotes[i] >= 0 && groupVotes[i + 1] >= 0) //Compete
                    {
                        groupScores[i] += 2;
                        groupScores[i + 1] += 2;

                    } else if (groupVotes[i] < 0 && groupVotes[i + 1] < 0) //Cooperate
                      {
                        groupScores[i] += 1;
                        groupScores[i + 1] += 1;
                    } else if (groupVotes[i] < 0 && groupVotes[i + 1] > 0)//higher team comp
                      {
                        groupScores[i] += 1;
                        groupScores[i + 1] += 4;
                    } else if (groupVotes[i] > 0 && groupVotes[i + 1] < 0)//lower team comp
                      {
                        groupScores[i] += 4;
                        groupScores[i + 1] += 1;
                    } else {
                        Debug.Log("Error");
                    }
                }
                CheckHighest();

                break;

            case 2:  //Attacked View

                for (int i = 0; i < numGroups; i++) {

                    if (i == highestGroup) {

                        if (groupVotes[i] > 0)
                        {
                            groupScores[i] -= 5;
                            speakUp1 = true;
                           
                        }
                        else if (groupVotes[i] <= 0){
                            
                            speakUp1 = false;
                       
                        }
                    } 
                        
                     
                }
                //Added because we have two rounds in round 2
                roundNum += 1;
                break;

            case 3: //Round 2 Dilemma
                for (int i = 0; i < numGroups; i++)
                {
                    if (i != highestGroup) 
                    {
                        if (groupVotes[i] >= 0)
                        { 
                            groupScores[i] += 10;
                        Debug.Log("Do nothing");
                        }
                        else if (groupVotes[i] < 0) {
                        Debug.Log("Got involved. If everyone gets involved the game should end.");

                        //Need end functionality here.
                        }
                    }
                }
                    break;
            case 4:
                int votesFor = 0;
                int votesAgainst = 0;
                for (int i = 0; i < numGroups; i++) {
                    if (groupVotes[i] >= 0) {
                        votesFor++;
                        if (groupVotes[i] != highestGroup)
                        {
                            groupScores[i] += 5;
                        }
                        Debug.Log("Votes for");
                    } else {
                        votesAgainst++;
                    }
                }
                if (votesFor >= votesAgainst) {
                    discrimLaw = true;
                    Debug.Log("Discrim law is" + discrimLaw);
                } else {
                    StopGame();
                    Debug.Log("Stop Game");
                }
                break;
            case 5:

                //If discrimLaw = true then highest group(otherized group) will multiply points by 0.5

               
                for (int i = 0; i < numGroups; i += 2)
                {

                    if (groupVotes[i] >= 0 && groupVotes[i + 1] >= 0) //Compete
                    {
                        groupScores[i] += 2;
                        groupScores[i + 1] += 2;
                        
                        
                    }
                    else if (groupVotes[i] < 0 && groupVotes[i + 1] < 0) //Cooperate
                    {
                        groupScores[i] += 1;
                        groupScores[i + 1] += 1;
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
                if(discrimLaw)
                    groupScores[highestGroup] -= 2;
                roundNum += 1;
                break;
                
            case 6: //Round 4 Violence & Attacked Views
                for (int i = 0; i < numGroups; i++)
                {

                    if (i == highestGroup)
                    {
                        if (groupVotes[i] > 0)
                        {
                            speakUp2 = true;
                            groupScores[i] -= 5;
                            //This should add some text that tells the other group that what they are hearing is not true.
                        }
                    }
                    else if (groupVotes[i] < 0)
                    {
                        groupScores[i] -= 10;
                        //Need end functionality here.
                    }
                }
                
                break;
            default:
                print("ERR: Outcast");
                break;
        }
        
        viewNum += 1;
        ResetAll();
    }
   

    public void CheckHighest()
    {
        int highestScore = -1;
        highestGroup = -1;

        List<int> highestGroupList = new List<int>();

        for (int i = 0; i < groupScores.Count; i++)
        {
            if (groupScores[i] > highestScore) {
                highestGroupList.Clear();
                highestGroupList.Add(i);
                highestScore = groupScores[i];
            } else if (groupScores[i] == highestScore) {
                highestGroupList.Add(i);
            }
        }
        highestGroup = highestGroupList[Random.Range(0, highestGroupList.Count)];

        
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

    private void Update()
    {

        
    }

}
