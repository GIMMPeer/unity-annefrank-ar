using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;


public class Round4AttackedView : View
{



    [SerializeField]
    private Button Speakup;

    [SerializeField]
    private Button nothing;

    public override void Initialize()
    {
        base.Initialize();
        Speakup.onClick.AddListener(() => {

            Player.Instance.HasVoted = !Player.Instance.HasVoted;
            Player.Instance.VoteStatus = -1;

            GameManager.Instance.ReadyCheck();

        });

        nothing.onClick.AddListener(() => {

            Player.Instance.HasVoted = !Player.Instance.HasVoted;
            Player.Instance.VoteStatus = 1;

            GameManager.Instance.ReadyCheck();

        });


    }

   

}
