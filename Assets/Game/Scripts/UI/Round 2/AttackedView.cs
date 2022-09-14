using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;


public class AttackedView : View
{
    


    [SerializeField]
    private Button fightBack;

    [SerializeField]
    private Button stayDown;

    public override void Initialize()
    {
        base.Initialize();
        fightBack.onClick.AddListener(() => {

            Player.Instance.HasVoted = !Player.Instance.HasVoted;
            Player.Instance.VoteStatus = 1;

            Player.Instance.CallToReadyCheck();
            
            });

        stayDown.onClick.AddListener(() => {

            Player.Instance.HasVoted = !Player.Instance.HasVoted;
            Player.Instance.VoteStatus = -1;

            Player.Instance.CallToReadyCheck();

        });


    }

    private void Update()
    {
    }

}
