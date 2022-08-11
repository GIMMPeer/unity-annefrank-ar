using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using FishNet.Connection;
public class GroupOld : NetworkBehaviour
{
    public static GroupOld Instance { get; private set; }

    private int groupNum;
    private int score;

    [field: SyncVar]
    public bool GroupIsReady
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    [SyncObject]
    public readonly SyncList<Player> TeamPlayer = new SyncList<Player>();

    [field: SyncVar]
    public int GroupScore
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    

    [field: SyncVar]
    public string Name
    {
        get;

        [ServerRpc(RequireOwnership = false)]
        set;
    }

    /*public Group(int groupNum, int score) {
        this.groupNum = groupNum;
        this.score = score;
    }*/

    public void init(int groupNum, int score) {
        this.groupNum = groupNum;
        this.score = score;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();


        //GameManager.Instance.players.Add(this);

    }
    public override void OnStopServer()
    {
        base.OnStopServer();


       // GameManager.Instance.players.Remove(this);

    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        Instance = this;

        //UIManager.Instance.Initialize();


        //SetUsername();



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
        //Debug.Log("Starting Game" + this.Username);


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

    [ServerRpc(RequireOwnership = false)]
    public void ServerSetIsReady(bool value)
    {
        GroupIsReady = value;
    }
}
