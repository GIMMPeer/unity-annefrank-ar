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


    public override void Initialize()
    {
        base.Initialize();

    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (!IsInitialized) return;

        group1Score.text = $" Group 1 Score : {GameManager.Instance.groupScores[0]}";
        
        group2Score.text = $" Group 2 Score : {GameManager.Instance.groupScores[1]}";
       
    }
}
