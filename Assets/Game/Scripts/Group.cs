using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using FishNet.Connection;
public class Group
{
    public static Group Instance { get; private set; }

    public int groupNum { get; set; }
    public int score { get; set; }
    public int votes { get; set; }
    public bool isOtherized { get; set; }

    public Group(int groupNum, int score) {
        this.groupNum = groupNum;
        this.score = score;
    }

    public Group() {
        this.groupNum = 0;
        this.score = 0;
    }
}
