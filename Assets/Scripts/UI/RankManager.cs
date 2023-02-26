using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    public List<PlayerData> playerData;

    [SerializeField] private Firebase firebase;
    private Firebase.User user;
    public List<PlayerData> playerRank = new List<PlayerData>();

    private void Awake()
    {
        firebase.OnLoaded += LoadRankData;
        user = new Firebase.User();
    }

    private void LoadRankData(Firebase.User user)
    {
        ClearRankData();

        for (int i = 0; i < user.userData.Count; i++)
        {
            Firebase.User.UserData data = user.userData[i];
            playerRank.Add(new PlayerData(i + 1, data.Name, data.Points));
        }
    }

    public void AddRankData(string name, int savePoint)
    {
        bool insert = false;
        int i = 0;
        while(!insert && i < playerRank.Count)
        {
            if(savePoint > playerRank[i].playerScore)
            {
                playerRank.Insert(i, new PlayerData(i + 1, name, savePoint));
                insert = true;
            }
            else
            {
                i++;
            }
        }
        if(!insert)
        {
            playerRank.Add(new PlayerData(i + 1, name, savePoint));
        }
        while(i < playerRank.Count)
        {
            playerRank[i].rankNumber++;
            i++;
        }
    }

    public void ClearRankData()
    {
        playerRank.Clear();
    }

    public void SaveRankData()
    {
        user.userData = new List<Firebase.User.UserData>();
        for (int i = 0; i < playerRank.Count; i++)
        {
            Firebase.User.UserData ud = new Firebase.User.UserData("", 0, 0);
            ud.Name = playerRank[i].playerName;
            ud.Number = playerRank[i].rankNumber;
            ud.Points = playerRank[i].playerScore;
            user.userData.Add(ud);
        }
        firebase.Submit(user);
    }

    public void GetRankData()
    {
        firebase.GetData();
    }
}
