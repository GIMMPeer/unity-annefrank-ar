using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

public class TeamManager : NetworkBehaviour
{
    [Serializable]
    public class Team : ISerializable
    {
        public ObservableCollection<Player> _members = new ObservableCollection<Player>();

        public int maxLength = 4;

        public Team(List<Player> mem, int max)
        {
            _members = new ObservableCollection<Player>(mem);
            maxLength = max;
            _members.CollectionChanged += members_OnChange;
        }

        public Team(List<Player> mem)
        {
            _members = new ObservableCollection<Player>(mem);
            _members.CollectionChanged += members_OnChange;
        }

        public Team(int max)
        {
            maxLength = max;
            _members.CollectionChanged += members_OnChange;
        }

        public Team()
        {
            _members.CollectionChanged += members_OnChange;
        }

        private void members_OnChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_members.Count > maxLength)
            {
                _members.Remove(_members.Last());
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("json", JsonConvert.SerializeObject(this));
        }

        public Team(SerializationInfo info, StreamingContext context)
        {
            Team t = (Team)JsonConvert.DeserializeObject(info.GetString("json"));
            _members = t._members;
            _members.CollectionChanged += members_OnChange;
            maxLength = t.maxLength;
        }
    }

    [SyncObject]
    public readonly SyncList<Team> teams = new SyncList<Team>();
    int index = 0;
    int numTeams = 2;//start game w/ two teams

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc]
    public void AddPlayer(Player p)
    {
        if (teams.Count < numTeams)
        {
            teams.Add(new Team(new List<Player> { p }));
            print($"added player {p.Username} to team {teams.IndexOf(teams.Last())}");
        }
        else
        {
            teams[index]._members.Add(p);
            if (!teams[index]._members.Contains(p))
            {
                teams.Add(new Team(new List<Player> { p }));
                print($"added player {p.Username} to team {teams.IndexOf(teams.Last())}");
                numTeams += numTeams;
                RebalanceTeams();
            } else
            {
                print($"added player {p.Username} to team {index}");
            }
            index++;
            index = index % (numTeams - 1);
        }
    }

    [ServerRpc]
    public void RebalanceTeams()
    {
        print("rebalance");
        List<Player> players = new List<Player>();
        foreach (Team t in teams)
        {
            foreach (Player x in t._members)
            {
                players.Add(x);
            }
        }
        for (int i = 0; i < teams.Count; i++)
        {
            teams[i]._members.Clear();
        }
        index = 0;
        foreach (Player p in players)
        {
            AddPlayer(p);
        }
    }
}
