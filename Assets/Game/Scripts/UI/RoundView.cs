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
    private TextMeshProUGUI[] groupScores;

    [SerializeField]
    private Button toggleReadyButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    [SerializeField]
    private Button continueButton;

    public override void Initialize()
    {
        base.Initialize();
        
    }

    public override void Show(object args = null)
    {
        base.Show(args);

       
        

    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsInitialized) return;


        toggleReadyButton.onClick.AddListener(() => Player.Instance.IsReady = !Player.Instance.IsReady);

        if (InstanceFinder.IsServer) {
            continueButton.onClick.AddListener(() => GameManager.Instance.ReadyCheck());
        } else {
            continueButton.gameObject.SetActive(false);
        }

        group1Score.text = $" Group 1 Score : {GameManager.Instance.groupScores[0]}";

        group2Score.text = $" Group 2 Score : {GameManager.Instance.groupScores[1]}";

        /*
        for(int i = 0; i < GameManager.Instance.numGroups; i++)
        {
            Debug.Log(GameManager.Instance.highestGroup);
            if(i == GameManager.Instance.highestGroup)
            {*/

        //} 

        //}
        readyButtonText.color = Player.Instance.IsReady ? Color.green : Color.red;

        //Debug.Log("Highest Group(RV): " + GameManager.Instance.highestGroup);

        switch (GameManager.Instance.highestGroup) {
            case 0:
                group1Prop.gameObject.SetActive(true);
                group1Prop.text = "You are bad!";
                group2Prop.gameObject.SetActive(false);

                break;
            case 1:
                group2Prop.gameObject.SetActive(true);
                group2Prop.text = "You are bad!";
                group1Prop.gameObject.SetActive(false);
                break;
            default:
                break;

        }


    }

    private void SetUpRoundEnd() {

        
    }
}
