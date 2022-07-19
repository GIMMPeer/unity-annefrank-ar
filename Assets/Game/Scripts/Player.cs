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

    [field: SyncVar]
    public bool IsReady
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
    private string[] adjectives = { "Nefarious", "Curious", "Miniscule", "Humongous", "Cute", "Silly", "Majestic", "Indubitable", "Serendipitous", "Magnetic", "Sassy", "Brutal", "Mighty", "Suspicious", "Sneaky"};
    private string[] nouns = { "Red Panda", "Lemur", "Capybara", "Lion", "Robin", "Tortoise", "Hummingbird", "Snake", "Chipmunk", "Squirrel", "Ferret", "Owl", "Hornbill", "Mouse", "Hampster", "Human"};


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

        Debug.Log("Starting Game");
    }

    [Server]
    public void StopGame()
    { 
    
    }





    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
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
