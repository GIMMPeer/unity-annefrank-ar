using FishNet;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using UnityEngine.UI;

public class Round1EndView : View
{
    //[SerializeField]
    //private TextMeshProUGUI[] groupPropText;
    [SerializeField]
    private TextMeshProUGUI[] groupScoreText;

    [SerializeField]
    private TextMeshProUGUI roundEndText;

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

        for (int i = 0; i < GameManager.Instance.numGroups; i++) {
            groupScoreText[i].text = $"{i+1}: Group {GameManager.Instance.orderedGroups[i].groupNum}";
            // Dev Option, comment above line and uncomment below to see group scores alongside rankings
            // groupScoreText[i].text = $"{i+1}: Group {GameManager.Instance.orderedGroups[i].groupNum}; Score: {GameManager.Instance.orderedGroups[i].score}";
        }
    }

    void Update() {
        readyButtonText.color  = Player.Instance.IsReady ? Color.green : Color.red;
    }
}
