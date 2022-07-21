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
    public bool IsReady
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    [field: SyncVar]
    public bool HasVoted
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    [field: SyncVar]
    public bool Coop
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }
    [field: SyncVar]
    public bool Comp
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    [field: SyncVar]
    public bool R1Start
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    [field: SyncVar]
    public bool R1End
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
    private string[] nouns = { "Red Panda", "Lemur", "Capybara", "Lion", "Robin", "Tortoise", "Hummingbird", "Snake", "Chipmunk", "Squirrel", "Ferret", "Owl", "Hornbill", "Mouse", "Hampster", "Human", "Dolphin"};


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
    
    }





    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;


        if (ShowLobby == true)
        {
            UIManager.Instance.Show<LobbyView>();
        }
        if (R1Start == true)
        {
            UIManager.Instance.Show<DilemmaView>();

            if (Coop == true)
            {
                HasVoted = true;
                Score += 1;
            }
            else if (Comp == true)
            {
                HasVoted = true;
                Score += 5;
            }
        }

        if (R1End)
        {
            R1Start = false;
            UIManager.Instance.Show<RoundView>();
        }
    }

    public void SetUsername()
    {
        int adjective = Random.Range(0, adjectives.Length);
        int noun = Random.Range(0, nouns.Length);

        Username = adjectives[adjective] + " " + nouns[noun];
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerSetIsReady(bool value)
    {
        IsReady = value;
    }
}
