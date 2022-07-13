using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timer = 30;
    float maxTimer;
    TextMeshProUGUI text;
    Image dial;

    // Start is called before the first frame update
    void Start()
    {
        maxTimer = timer;
        text = GetComponentInChildren<TextMeshProUGUI>();
        dial = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (!FishNet.InstanceFinder.IsServer)
        {
            dial.fillAmount = timer / maxTimer;
            text.text = Mathf.CeilToInt(timer).ToString();
        }
    }
}
