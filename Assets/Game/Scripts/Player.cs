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

    [SyncVar]
    public bool isReady;

    /*
    [SyncObject]
    public readonly SyncDictionary<int, Player> instances = new SyncDictionary<int, Player>();
    */

    //Username Generation
    private string[] adverbs = { "Big", "Small", "Sad", "Happy", "Cute" };
    private string[] nouns = {"Red Panda", "Lemur", "Capybara", "Lion"};


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

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
    }

    public void SetUsername()
    {
        int adverb = Random.Range(0, adverbs.Length);
        int noun = Random.Range(0, nouns.Length);

        Username = adverbs[adverb] + " " + nouns[noun];
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerSetIsReady(bool value)
    {
        isReady = value;
    }
}
