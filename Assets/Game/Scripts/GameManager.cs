using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;

public sealed class GameManager : NetworkBehaviour {
    public static GameManager Instance { get; private set; }

    [field: SerializeField]
    [SyncObject]
    public readonly SyncList<Player> players = new SyncList<Player>();
    [field: SerializeField]
    [SyncObject]
    public readonly SyncList<int> groupScores = new SyncList<int>();

    private int[] groupVotes;

    [field: SerializeField]
    [SyncObject]
    public readonly SyncList<Group> orderedGroups = new SyncList<Group>();
    private bool stopGame;


    [field: SyncVar]
    public bool CanStart { get; private set; }

    [field: SyncVar]
    public int otherizedGroup { get; private set; }

    //public bool r1Active { get; private set; }

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

    [field: SyncVar]
    public int endingNum { get; private set; }

    private void Awake() {
        Instance = this;
        endingNum = 0;
    }


    [Server]
    public void CheckToAssignGroups()
    {
        bool readyAssignGroups = players.All(player => player.ReadyForTeamCreation);

        if (readyAssignGroups)
        {
            CreateAndAssignGroups();
        }
    }

    [Server]
    public void CreateAndAssignGroups() {
        Debug.Log("Player Count " + players.Count);
        int numPlayers = players.Count;

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
        for (int i = 0; i < numGroups; i++) {
            groupScores.Add(0);
        }

        // Sort players into groups
        AssignGroups();
        viewNum = 1;
    }

    [ObserversRpc]
    public void AssignGroups()
    {
        int groupNumber = 0;
        int numPlayers = players.Count;
        for (int i = 0; i < numPlayers; i++)
        {
            if (groupNumber == numGroups)
            {
                groupNumber = 0;
            }
            players[i].GroupNumber = groupNumber;
            groupNumber++;
        }
    }
    [Server]
    public void ReadyCheck() {
        bool readyStartRound = players.All(player => player.IsReady);
        Debug.Log(readyStartRound);

        if (readyStartRound) {
            Debug.Log("Started Round");
            IncreaseViewNum();
            roundNum += 1;
            viewNum += 1;
            ResetAll();
            GroupVotesReset();
        }

        bool ReadyEndRound = players.All(player => player.HasVoted);
        
        if (ReadyEndRound) {
            Debug.Log("All players have voted");
            for (int i = 0; i < players.Count; i++) {
                CheckVotes(players[i]);
            }
            AssignScores();
            
        }
    }

    [ObserversRpc]
    public void IncreaseViewNum()
    {
        roundNum += 1;
        viewNum += 1;

    }

    [ObserversRpc]
    public void IncreaseViewNumnoRound()
    { 
    
    }


    [ObserversRpc]
    public void SetPlayerStatus(int status, bool hasvoted)
    {
        Player.Instance.VoteStatus = status;
        Player.Instance.HasVoted = hasvoted;
    }

    [ObserversRpc]
    public void SetPlayerReady(bool isready)
    {
        Player.Instance.IsReady = isready;
    }

    [Server]
    public void CheckVotes(Player player) {
        var playerGroupNum = player.GroupNumber;
        groupVotes[playerGroupNum] += player.VoteStatus;
    }

    [ObserversRpc]
    public void AssignScores() {
        Debug.Log("Round Number is " + roundNum);
        switch (roundNum) {
            case 1: //First Dilemma
                for (int i = 0; i < numGroups; i += 2) {
                    if (groupVotes[i] >= 0 && groupVotes[i + 1] >= 0)  //Compete
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
                    } else {
                        Debug.Log("Error");
                    }
                }
                RankScores();
                CheckHighest();

                break;

            case 2:  //Attacked View

                
                        for (int i = 0; i < numGroups; i++) {

                    if (i == otherizedGroup)
                    {

                        if (groupVotes[i] > 0)
                        {
                            groupScores[i] -= 5;
                        }
                       else {
                            speakUp1 = false;
                        }
                    }


                }
                //Added because we have two rounds in round 2
                roundNum += 1;
                break;

            case 3: //Round 2 Dilemma
                int end = 0;

                for (int i = 0; i < numGroups; i++)
                {
                    if (i != otherizedGroup)
                    {
                        if (groupVotes[i] >= 0)
                        {
                            groupScores[i] += 10;
                            Debug.Log("Do nothing");
                        }
                        else if (groupVotes[i] < 0)
                        {
                            Debug.Log("Got involved. If everyone gets involved the game should end.");
                        }
                        if (end >= numGroups - 1)
                        {
                            endingNum = 1;
                            StopGame();
                        }
                          
                            //Need end functionality here. Good Ending
                        
                    }
                }
                break;
            case 4:
                int votesFor = 0;
                int votesAgainst = 0;
                for (int i = 0; i < numGroups; i++) {
                    if (groupVotes[i] >= 0)
                    {
                        votesFor++;
                        if (groupVotes[i] != otherizedGroup)
                        {
                            groupScores[i] += 5;
                        }
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
                  
                }
                if (discrimLaw)
                    groupScores[otherizedGroup] -= 2;
                roundNum += 1;
                break;

            case 6: //Round 4 Violence & Attacked Views

                int end1 = 0;
                for (int i = 0; i < numGroups; i++) {

                        if (i == otherizedGroup)
                        {
                            if (groupVotes[i] > 0)
                            {
                                speakUp2 = true;
                                groupScores[i] -= 5;
                            }

                        }
                        else if (groupVotes[i] < 0)
                        {
                            groupScores[i] -= 10;
                        
                            end1++;
                            if (end1 >= numGroups - 1)
                            {
                            endingNum = 1;
                            StopGame();
                            }
                        } 
                        //Need end functionality here.
                        //Good Ending
                    }
                

                break;
            case 7: //Round 5 Elimination View
                int end2 = 0;
                int end3 = 0;
                for (int i = 0; i < numGroups; i++)
                {
                        if (i != otherizedGroup)
                        {
                            if (groupVotes[i] >= 0)
                            {
                                end2++;

                                if (end2 >= numGroups / 2)
                                {

                                    endingNum = 3;
                                    StopGame();
                                }

                                else if (groupVotes[i] < 0)
                                {
                                    end3++;

                                    if (end3 > numGroups / 2)
                                    {

                                        endingNum = 2;
                                        StopGame();
                                    }

                                }
                            
                        }
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

    [Server]
    public void RankScores()
    {
        int highestScore = -1;
        otherizedGroup = -1;

        List<int> highestGroupList = new List<int>();

        for (int i = 0; i < groupScores.Count; i++)
        {
            if (groupScores[i] > highestScore)
            {
                highestGroupList.Clear();
                highestGroupList.Add(i);
                highestScore = groupScores[i];
            }
            else if (groupScores[i] == highestScore)
            {
                highestGroupList.Add(i);
            }
        }
        otherizedGroup = highestGroupList[Random.Range(0, highestGroupList.Count)];
    }
    [ObserversRpc]
    public void CheckHighest()
    {
        otherizedGroup = orderedGroups[0].groupNum;
    }

    [ObserversRpc]
    public void ResetAll()
    {
        Debug.Log("Reset all ready/votes");
        for (int i = 0; i < players.Count; i++)
        {
            players[i].IsReady = false;
            players[i].HasVoted = false;
            players[i].VoteStatus = 0;
        }
    }

    [ServerRpc]
    public void GroupVotesReset()
    {
        for (int i = 0; i < numGroups; i++)
        {
            groupVotes[i] = 0;
        }
    }

    [Server]
    public void StopGame()
    {
        Instance.ServerManager.StopConnection(true);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].StopGame();
        }
    }

}
