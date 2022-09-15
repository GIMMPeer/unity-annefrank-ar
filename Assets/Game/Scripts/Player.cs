using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using FishNet.Connection;
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

    [field: SerializeField]
    [field: SyncVar]
    public int GroupNumber
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }


    [field: SerializeField]
    [field: SyncVar]
    public int Score
    {
        get;

        [ServerRpc]
        private set;
    }

    [field: SyncVar]
    public bool ShowLobby
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }
    [field: SyncVar]
    public bool stopGame
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    } = false;

    [field: SyncVar]
    public bool HasVoted
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    } = false;

    [field: SyncVar]
    public bool ReadyForTeamCreation
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    } = false;


    [field: SyncVar]
    public bool IsReady
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    [field: SyncVar]
    public int VoteStatus
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

 


    /*
    [SyncObject]
    public readonly SyncDictionary<int, Player> instances = new SyncDictionary<int, Player>();
    */

    //Username Generation
    private string[] adjectives = { "Nefarious", "Curious", "Miniscule", "Humongous", "Cute", "Silly", "Majestic", "Indubitable", "Serendipitous", "Magnetic", "Sassy", "Brutal", "Mighty", "Suspicious", "Sneaky", "Hairy"};
    private string[] nouns = { "Red Panda", "Lemur", "Capybara", "Lion", "Robin", "Tortoise", "Hummingbird", "Snake", "Chipmunk", "Squirrel", "Ferret", "Owl", "Hornbill", "Mouse", "Hampster", "Dolphin", "Duck", "Fish"};


    public override void OnStartServer()
    {
        base.OnStartServer();

       
       GameManager.Instance.players.Add(this);
        
        
    }
    public override void OnStopServer()
    {
        base.OnStopServer();


       
       GameManager.Instance.players.Remove(this);
        
        
        
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        
        Instance = this;

        UIManager.Instance.Initialize();

       
       SetUsername();
      
        

        



        //instances.Add(LocalConnection.ClientId, this);
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        //instances.Remove(LocalConnection.ClientId, this);
    }

    [ServerRpc]
    public void CallToCheckToAssignGroups()
    {
        GameManager.Instance.CreateAndAssignGroups();
    }

    [ServerRpc]
    public void CallToReadyCheck()
    {
        GameManager.Instance.ReadyCheck();

    }
    [ServerRpc]
    public void CallToSetStatus(int status, bool hasVoted)
    {
        GameManager.Instance.SetPlayerStatus(status, hasVoted);
    }

    [Server]
    public void StartGame()
    {
        if (!IsOwner) return;
        
        Debug.Log("Starting Game" + this.Username);

        
    }

    [Server]
    public void StopGame()
    {
        stopGame = true;
    }





    // Update is called once per frame
    void Update()
    {
          if (!IsOwner) return;
        //Debug.Log(IsClient);
        Debug.Log("View Num" + GameManager.Instance.viewNum);
     
        Debug.Log("Group Number:" + GroupNumber);
        Debug.Log("Ready Status" + IsReady);
        Debug.Log("Has Voted Status" + HasVoted);
        if (stopGame)
        {
            switch (GameManager.Instance.endingNum)
            {
                case 1:
                    UIManager.Instance.Show<GoodEnding>();
                    break;
                case 2:
                    UIManager.Instance.Show<NeutralEnding>();
                    break;
                case 3:
                    UIManager.Instance.Show<BadEnding>();
                    break;
            }
        }

        if (!stopGame)
        {
            switch (GameManager.Instance.viewNum)
            {
                case 0:

                    break;
                case 1:
                    UIManager.Instance.Show<LobbyView>();

                    break;
                case 2:
                    UIManager.Instance.Show<Round1DilemView>();

                    break;
                case 3:
                    UIManager.Instance.Show<Round1EndView>();
                    break;
                case 4:
                   if (GroupNumber == GameManager.Instance.otherizedGroup)
                    {
                        UIManager.Instance.Show<AttackedView>();
                    }
                    else
                    {
                        UIManager.Instance.Show<WaitView>();
                    }
                    break;
                case 5:
                    if (GroupNumber == GameManager.Instance.otherizedGroup)
                    //if (GameManager.Instance.groups[GroupNumber].isOtherized)
                    {
                        UIManager.Instance.Show<WaitView>();
                    }
                    else
                    {

                        UIManager.Instance.Show<Round2DilemView>();
                    }
                    break;
                case 6:
                    UIManager.Instance.Show<Round2EndView>();
                    break;
                case 7:
                    UIManager.Instance.Show<Round3DilemView>();
                    break;
                case 8:
                    UIManager.Instance.Show<Round3EndView>();
                    break;
                case 9:
                    UIManager.Instance.Show<Round4DilemView>();
                    break;
                case 10:
                    if (GroupNumber != GameManager.Instance.otherizedGroup) { 
                   // if (!GameManager.Instance.groups[GroupNumber].isOtherized) 
                        UIManager.Instance.Show<WaitView>();
                    }
                    else
                    {
                        UIManager.Instance.Show<Round4AttackedView>();
                    }
                    break;
                case 11:
                    if (GroupNumber != GameManager.Instance.otherizedGroup)
                    //if (!GameManager.Instance.groups[GroupNumber].isOtherized) 
                    {
                        UIManager.Instance.Show<Round4ViolenceView>();
                    }
                    else
                    {
                        UIManager.Instance.Show<WaitView>();
                    }
                    break;
                case 12:
                    UIManager.Instance.Show<Round4EndView>();
                    break;
                case 13:
                    if (GroupNumber != GameManager.Instance.otherizedGroup)
                    //if (!GameManager.Instance.groups[GroupNumber].isOtherized) 
                    {
                        UIManager.Instance.Show<Round5EliminationView>();
                    }
                    else
                    {
                        UIManager.Instance.Show<WaitView>();
                    }
                    break;
            }
        }
        
        
       

       
    }

    public void LoadView<V>() where V : View
    {
        UIManager.Instance.Show<V>();
    }
    public void SetUsername()
    {
        int adjective = Random.Range(0, adjectives.Length);
        int noun = Random.Range(0, nouns.Length);

        Username = adjectives[adjective] + " " + nouns[noun];
    }

    [ServerRpc]
    public void ServerSetIsReady(bool value)
    {
        GameManager.Instance.SetPlayerReady(value);
    }
}
