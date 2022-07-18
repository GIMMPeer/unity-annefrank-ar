using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

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



    //Username Generation
    private string[] adverbs = { "Big", "Small", "Sad", "Happy", "Cute" };
    private string[] nouns = {"Red Panda", "Lemur", "Capybara", "Lion"};

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        Instance = this;

        UIManager.Instance.Initialize();


        SetUsername();
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
}
