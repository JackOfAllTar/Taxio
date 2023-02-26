using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerData
{
    public int rankNumber;
    public string playerName;
    public int playerScore;

    public PlayerData(int rankNum, string name, int score)
    {
        rankNumber = rankNum;
        playerName = name;
        playerScore = score;
    }
}

public class RankData : MonoBehaviour
{
    public PlayerData playerData;
    [Space]
    [Header("UI")]
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerScoreText;

    public void UpdateData()
    {
        rankText.text = "#" + playerData.rankNumber.ToString();
        if(playerNameText != null)
        {
            playerNameText.text = playerData.playerName;
        }
        playerScoreText.text = playerData.playerScore.ToString();
    }
}
