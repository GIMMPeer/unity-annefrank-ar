using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodEnding : View
{
    [SerializeField]
    private Button finish;

    public override void Initialize()
    {
        base.Initialize();

        finish.onClick.AddListener(() => {
            Debug.Log("Stop Game");
        });
    }
}
