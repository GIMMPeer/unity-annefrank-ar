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
    private Button toggleReadyButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    public override void Initialize()
    {
        base.Initialize();
        toggleReadyButton.onClick.AddListener(() => {

            Player.Instance.HasVoted = !Player.Instance.HasVoted;
            GameManager.Instance.ReadyCheck();
            
            });


    }

    private void Update()
    {
        readyButtonText.color = Player.Instance.HasVoted ? Color.green : Color.red;
    }

}
