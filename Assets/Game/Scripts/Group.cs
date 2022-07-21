using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using FishNet.Connection;
using System.Collections.Generic;

public class Group
{
    public static Group Instance { get; private set; }

    public bool GroupIsReady;

    public readonly List<Player> TeamPlayer = new List<Player>();

    public int GroupScore;

    public string Name;
}
