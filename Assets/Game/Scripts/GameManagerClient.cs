using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System;

public class GameManagerClient : NetworkBehaviour
{
    [FishNet.Object.Synchronizing.SyncVar] bool? player2choice = null;
    [FishNet.Object.Synchronizing.SyncVar] bool? player1choice = null;
    [FishNet.Object.Synchronizing.SyncVar] int player1points = 0;
    [FishNet.Object.Synchronizing.SyncVar] int player2points = 0;
    public UnityEngine.UI.Button cooperate;
    public UnityEngine.UI.Button compete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player1choice != null && player2choice != null)
        {
            if (player1choice == true && player2choice == true)
            {
                player1points += 3;
                player2points += 3;
            } else if (player1choice == true && player2choice == false)
            {
                player1points += 1;
                player2points += 4;
            }
            else if (player1choice == false && player2choice == true)
            {
                player1points += 4;
                player2points += 1;
            }
            else if (player1choice == false && player2choice == false)
            {
                player1points += 2;
                player2points += 2;
            }
            print("player 1 points = " + player1points.ToString() + " player 2 points = " + player2points.ToString());
            cooperate.interactable = true;
            compete.interactable = true;
            player1choice = null;
            player2choice = null;
        }
    }

    public void CooperateButton()
    {
        if (player1choice == null)
        {
            player1choice = true;
        } else if (player2choice == null)
        {
            player2choice = true;
        } else
        {
            print("eror");
        }
        cooperate.interactable = false;
        compete.interactable = false;
    }

    public void CompeteButton()
    {
        if (player1choice == null)
        {
            player1choice = false;
        }
        else if (player2choice == null)
        {
            player2choice = false;
        } else
        {
            print("eror");
        }
        cooperate.interactable = false;
        compete.interactable = false;
    }
}
