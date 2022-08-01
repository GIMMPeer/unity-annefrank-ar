using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;
public class Round3DilemView : View
{
    [SerializeField]
    private TextMeshProUGUI dilemmaText;

    [SerializeField]
    private Button helpButton;

    [SerializeField]
    private Button nothingButton;


    public override void Initialize()
    {
        base.Initialize();

        helpButton.onClick.AddListener(() => {
            Player.Instance.VoteStatus = -1;
            Player.Instance.HasVoted = true;
            GameManager.Instance.ReadyCheck();
        });
        nothingButton.onClick.AddListener(() =>
        {
            Player.Instance.VoteStatus = 1;
            Player.Instance.HasVoted = true;
            GameManager.Instance.ReadyCheck();
        });
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    // Update is called once per frame
    void Update()
    {

        dilemmaText.text = $" Since all of the evidence points towards {GameManager.Instance.highestGroup} cheating to gain all of their points. Should we pass a law to limit the points they can get? If you choose to vote yay, you will gain 5 points. If you vote nay, you won't recieve any points." ;


    }
}
