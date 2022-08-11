using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;

public class Round2DilemView : View
{
    [SerializeField]
    private TextMeshProUGUI dilemmaText;

    [SerializeField]
    private Button helpButton;

    [SerializeField]
    private Button nothingButton;

    [SerializeField]
    private TextMeshProUGUI otherText;


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
        
        dilemmaText.text = $" Group {GameManager.Instance.otherizedGroup} is being accused of cheating. Another group is ganging up with the other groups to make sure they don't cheat again! They say that if you stay out of it, they will give you points." + 
            $"What do you do?";

       
        if (GameManager.Instance.speakUp1 == true)
        {
            otherText.gameObject.SetActive(true);
            otherText.text = $" Group {GameManager.Instance.otherizedGroup} says that they didn't do anything. This is all a lie that other groups are telling";
        }
        else
        {
            otherText.gameObject.SetActive(false);
        }
    }
}
