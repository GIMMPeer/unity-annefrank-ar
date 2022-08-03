using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;
public class Round4ViolenceView : View
{
    [SerializeField]
    private TextMeshProUGUI dilemmaText;

    [SerializeField]
    private Button stopButton;

    [SerializeField]
    private Button nothingButton;

    [SerializeField]
    private TextMeshProUGUI otherText;

    public override void Initialize()
    {
        base.Initialize();

        stopButton.onClick.AddListener(() => {
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

        dilemmaText.text = $" Things have been escalating and the anonymous group is planning to steal away group {GameManager.Instance.highestGroup}'s points! Your group has an option to either stop them, risking several of your own points, or stand back which will keep you safe. Discuss with your group to decide the right course of action.";


        if (GameManager.Instance.speakUp2 == true)
        {
            otherText.gameObject.SetActive(true);
            otherText.text = $" Group {GameManager.Instance.highestGroup} says that they didn't do anything. This is all a lie that other groups are telling!";
        }
        else
        {
            otherText.gameObject.SetActive(false);
        }
    }
}
