using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;

public class RoundView : View
{

    [SerializeField]
    private TextMeshProUGUI group1Score;

    [SerializeField]
    private TextMeshProUGUI group2Score;

    [SerializeField]
    private TextMeshProUGUI group1Prop;

    [SerializeField]
    private TextMeshProUGUI group2Prop;

    [SerializeField]
    private TextMeshProUGUI[] groupPropText;
    [SerializeField]
    private TextMeshProUGUI[] groupScoreText;

    [SerializeField]
    private Button toggleReadyButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    [SerializeField]
    private Button continueButton;

    void OnEnable() {
        toggleReadyButton.onClick.AddListener(() => Player.Instance.IsReady = !Player.Instance.IsReady);

        if (InstanceFinder.IsServer) {
            continueButton.onClick.AddListener(() => GameManager.Instance.ReadyCheck());
        } else {
            continueButton.gameObject.SetActive(false);
        }

        Debug.Log("Highest Group End: " + GameManager.Instance.highestGroup);
        for (int i = 0; i < GameManager.Instance.numGroups; i++) {
            groupScoreText[i].text = $"Group {i+1} Score: {GameManager.Instance.groupScores[i]}";
            if (i == GameManager.Instance.highestGroup) {
                groupPropText[i].gameObject.SetActive(true);
            }
        }
        readyButtonText.color = Player.Instance.IsReady ? Color.green : Color.red;
    }
}
