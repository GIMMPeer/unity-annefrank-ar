using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Round5EliminationView : View
{
    [SerializeField]
    private TextMeshProUGUI dilemmaText;

    [SerializeField]
    private Button voteFor;

    [SerializeField]
    private Button voteAgainst;


    public override void Initialize()
    {
        base.Initialize();

        voteFor.onClick.AddListener(() => {
            Player.Instance.VoteStatus = 1;
            Player.Instance.HasVoted = true;
            GameManager.Instance.ReadyCheck();
        });
        voteAgainst.onClick.AddListener(() =>
        {
            Player.Instance.VoteStatus = -1;
            Player.Instance.HasVoted = true;
            GameManager.Instance.ReadyCheck();
        });
    }

    // Update is called once per frame
    void Update()
    {
        dilemmaText.text = $"The anonymous group decided enough was enough and proposes to remove group {GameManager.Instance.otherizedGroup} from the game all together. They state, “To protect the peace and fairness for all groups, we must remove group {GameManager.Instance.otherizedGroup} to restore it.” What do your group think about their claims? Discuss amongst yourselves.";
    }
}
