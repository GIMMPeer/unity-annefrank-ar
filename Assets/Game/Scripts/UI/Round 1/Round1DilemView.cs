using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;

public class Round1DilemView : View
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

            Player.Instance.CallToSetStatus(-1, true);
            //Player.Instance.VoteStatus = -1;
            //Player.Instance.HasVoted = true;
            Player.Instance.CallToReadyCheck();
        });
        compButton.onClick.AddListener(() =>
        {
            Player.Instance.CallToSetStatus(1, true);
            //Player.Instance.VoteStatus = 1;
            //Player.Instance.HasVoted = true;
            Player.Instance.CallToReadyCheck();
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
