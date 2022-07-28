using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;

public class DilemmaView : View
{

    [SerializeField]
    private TextMeshProUGUI dilemmaText;

    [SerializeField]
    private Button coopButton;

    [SerializeField]
    private Button compButton;


    public override void Initialize()
    {
        base.Initialize();

        coopButton.onClick.AddListener(() => {
            Player.Instance.VoteStatus = -1;
            Player.Instance.HasVoted = true;
            GameManager.Instance.ReadyCheck();
        });
        compButton.onClick.AddListener(() =>
        {
            Player.Instance.VoteStatus = 1;
            Player.Instance.HasVoted = true;
            GameManager.Instance.ReadyCheck();
        });
    }

    /*public override void Show(object args = null)
    {
        base.Show(args);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
