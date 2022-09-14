using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaitView : View
{
    [SerializeField]
    private TextMeshProUGUI waitText;

    [SerializeField]
    private Button toggleReadyButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    public override void Initialize()
    {
        base.Initialize();
        toggleReadyButton.onClick.AddListener(() => {
           // Player.Instance.HasVoted = !Player.Instance.HasVoted;
            Player.Instance.CallToSetStatus(0, true);
            Player.Instance.CallToReadyCheck();
            
        });



    }

    private void Update()
    {
        waitText.text = $"Something is happening. What's going on? ";

        //readyButtonText.color = Player.Instance.HasVoted ? Color.green : Color.red;

    }
}
