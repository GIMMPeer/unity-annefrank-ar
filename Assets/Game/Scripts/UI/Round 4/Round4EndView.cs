using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;
public class Round4EndView : View
{
    [SerializeField]
    private TextMeshProUGUI[] groupScoreText;
    [SerializeField]
    private TextMeshProUGUI[] groupPropText;

    [SerializeField]
    private Button toggleReadyButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    [SerializeField]
    private Button continueButton;

    void OnEnable()
    {
        toggleReadyButton.onClick.AddListener(() => Player.Instance.IsReady = !Player.Instance.IsReady);

        if (InstanceFinder.IsServer)
        {
            continueButton.onClick.AddListener(() => GameManager.Instance.ReadyCheck());
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }


        for (int i = 0; i < GameManager.Instance.numGroups; i++)
        {

            groupScoreText[i].text = $"Group {i + 1} Score: {GameManager.Instance.groupScores[i]}";
            if (i == GameManager.Instance.highestGroup)
            {
                groupPropText[i].gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        readyButtonText.color = Player.Instance.IsReady ? Color.green : Color.red;
    }
}
