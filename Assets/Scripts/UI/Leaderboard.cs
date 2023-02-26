using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankParent;
    private RankManager rankManager;
    private GameObject[] rankObject;

    public void ShowLeaderboard()
    {
        rankManager = GameObject.FindGameObjectWithTag("System").GetComponent<RankManager>();
        rankObject = new GameObject[rankManager.playerRank.Count];
        for (int i = 0; i < rankManager.playerRank.Count; i++)
        {
            rankObject[i] = Instantiate(rankPrefab, rankParent);
            RankData data = rankObject[i].GetComponent<RankData>();
            data.playerData = new PlayerData(i + 1, rankManager.playerRank[i].playerName, rankManager.playerRank[i].playerScore);
            data.UpdateData();
        }
    }

    public void HideLeaderboard()
    {
        for (int i = 0; i < rankManager.playerRank.Count; i++)
        {
            Destroy(rankObject[i]);
        }
    }
}
