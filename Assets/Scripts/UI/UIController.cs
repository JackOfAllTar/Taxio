using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    #region Attribute
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject levelUpgrade;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject pause;
    [Header("Player")]
    private GameObject playerPrefab;
    private PlayerController playerCon;
    [SerializeField] private GameObject playerHealth;
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI stageNumber;
    [SerializeField] private Slider waveBar;
    [SerializeField] private TextMeshProUGUI levelNumber;
    [SerializeField] private Slider levelBar;
    [SerializeField] private TextMeshProUGUI UIcoins;
    //private Camera mainCamera;
    [SerializeField] private List<PlayerUpgrade> newUpgrade = new List<PlayerUpgrade>();
    [Header("Pause")]
    [SerializeField] private UpgradeProfile upgradeProfile;
    private List<PlayerUpgrade> ownUpgrade = new List<PlayerUpgrade>();
    [Header("Button")]
    [SerializeField] private GameObject[] upgradeButton;
    [SerializeField] private GameObject[] shopButton;
    [SerializeField] private Image[] pauseUpgradeType;
    [SerializeField] private TextMeshProUGUI[] pauseUpgradeTier;
    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI endHeader;
    [SerializeField] private TextMeshProUGUI endStages;
    [SerializeField] private TextMeshProUGUI endStagesPoint;
    [SerializeField] private TextMeshProUGUI endLevels;
    [SerializeField] private TextMeshProUGUI endLevelsPoint;
    [SerializeField] private TextMeshProUGUI endCoins;
    [SerializeField] private TextMeshProUGUI endCoinsPoint;
    [SerializeField] private TextMeshProUGUI endPoints;
    [SerializeField] private TextMeshProUGUI winBonusPoint;
    private int score;
    [Header("Ranking")]
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankParent;
    private RankManager rankManager;
    #endregion

    #region Mode
    public void PlayMode(GameObject player)
    {
        Time.timeScale = 1;
        //mainCamera = Camera.main;
        controller.SetActive(true);
        levelUpgrade.SetActive(false);
        shop.SetActive(false);
        gameOver.SetActive(false);

        playerPrefab = player;
        playerCon = playerPrefab.GetComponent<PlayerController>();

        Instance.UIChangeLevel();
        Instance.UISetExp();
        Instance.UIChangeCoins();
        Instance.UISetPlayerHealth();

        playerCon.attack = true;
    }

    public void PauseMode()
    {
        Time.timeScale = 0;
        pause.SetActive(true);
        controller.SetActive(false);
        if(ownUpgrade.Count <= GameController.Instance.maxUpgradeCount)
        {
            for (int i = 0; i < ownUpgrade.Count; i++)
            {
                switch (ownUpgrade[i])
                {
                    case PlayerUpgrade.health:
                        pauseUpgradeType[i].sprite = upgradeProfile.healthUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.healthTier.ToString();
                        break;
                    case PlayerUpgrade.speed:
                        pauseUpgradeType[i].sprite = upgradeProfile.speedUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.speedTier.ToString();
                        break;
                    case PlayerUpgrade.cooldown:
                        pauseUpgradeType[i].sprite = upgradeProfile.cooldownUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.cooldownTier.ToString();
                        break;
                    case PlayerUpgrade.damage:
                        pauseUpgradeType[i].sprite = upgradeProfile.damageUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.damageTier.ToString();
                        break;
                    case PlayerUpgrade.paper:
                        pauseUpgradeType[i].sprite = upgradeProfile.paperUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.paperTier.ToString();
                        break;
                    case PlayerUpgrade.briefcase:
                        pauseUpgradeType[i].sprite = upgradeProfile.briefcaseUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.briefcaseTier.ToString();
                        break;
                    case PlayerUpgrade.gas:
                        pauseUpgradeType[i].sprite = upgradeProfile.gasUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.gasTier.ToString();
                        break;
                    case PlayerUpgrade.scythe:
                        pauseUpgradeType[i].sprite = upgradeProfile.scytheUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.scytheTier.ToString();
                        break;
                    case PlayerUpgrade.brick:
                        pauseUpgradeType[i].sprite = upgradeProfile.brickUpgrade.sprite;
                        pauseUpgradeTier[i].text = playerCon.upgradeStats.brickTier.ToString();
                        break;
                }
            }
            int blankUpgrade = GameController.Instance.maxUpgradeCount - ownUpgrade.Count;
            for (int i = 0; i < blankUpgrade; i++)
            {
                pauseUpgradeTier[i + ownUpgrade.Count].text = "";
                pauseUpgradeType[i + ownUpgrade.Count].sprite = upgradeProfile.blankUpgrade.sprite;
            }
        }
    }

    public void ResumeMode()
    {
        Time.timeScale = 1;
        pause.SetActive(false);
        controller.SetActive(true);
    }
    
    public void RestMode()
    {
        playerCon.attack = false;
    }

    public void LevelUpgradeMode()
    {
        Time.timeScale = 0;
        levelUpgrade.SetActive(true);
        controller.SetActive(false);
        UpgradeButton[] button = new UpgradeButton[3];

        if (ownUpgrade.Count < GameController.Instance.maxUpgradeCount)
        {
            for (int i = 0; i < 3; i++)
            {
                button[i] = upgradeButton[i].GetComponent<UpgradeButton>();
            }
            int j, j1, j2;
            j = Random.Range(0, newUpgrade.Count);
            button[0].SetUpgrade(0, newUpgrade[j]);
            j1 = j;
            while (j1 == j)
            {
                j = Random.Range(0, newUpgrade.Count);
            }
            button[1].SetUpgrade(0, newUpgrade[j]);
            j2 = j;
            while (j1 == j || j2 == j)
            {
                j = Random.Range(0, newUpgrade.Count);
            }
            button[2].SetUpgrade(0, newUpgrade[j]);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                button[i] = upgradeButton[i].GetComponent<UpgradeButton>();
            }
            int j, j1, j2;
            j = Random.Range(0, ownUpgrade.Count);
            button[0].SetUpgrade(0, ownUpgrade[j]);
            j1 = j;
            while (j1 == j)
            {
                j = Random.Range(0, ownUpgrade.Count);
            }
            button[1].SetUpgrade(0, ownUpgrade[j]);
            j2 = j;
            while (j1 == j || j2 == j)
            {
                j = Random.Range(0, ownUpgrade.Count);
            }
            button[2].SetUpgrade(0, ownUpgrade[j]);
        }
    }

    public void ShopUpgradeMode(int minHealAmount, int maxHealAmount, int minCost, int maxCost)
    {
        Time.timeScale = 0;
        controller.SetActive(false);
        shop.SetActive(true);

        UpgradeButton[] button = new UpgradeButton[3];
        for (int i = 0; i < 3; i++)
        {
            button[i] = shopButton[i].GetComponent<UpgradeButton>();
            button[i].SetCoin(playerCon.coins);
        }

        int healCost = Random.Range(minCost, maxCost);
        int heal = Random.Range(minHealAmount, maxHealAmount);
        button[0].SetHeal(healCost, heal);

        int upgradeCost = Random.Range(minCost, maxCost);
        if (ownUpgrade.Count < GameController.Instance.maxUpgradeCount)
        {
            int upgrade = Random.Range(0, newUpgrade.Count);
            button[1].SetUpgrade(upgradeCost, newUpgrade[upgrade]);
        }
        else
        {
            int upgrade = Random.Range(0, ownUpgrade.Count);
            button[1].SetUpgrade(upgradeCost, ownUpgrade[upgrade]);
        }
        button[2].SetQuit();
    }

    public void GameOverMode()
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
        controller.SetActive(false);

        endHeader.text = "Stage Failed";

        int stage = (GameController.Instance.currentStage - 1) * GameController.Instance.stageMultiply;
        endStages.text = "Stages:   " + GameController.Instance.currentStage.ToString();
        endStagesPoint.text = stage.ToString();

        int level = playerCon.playerLevel * GameController.Instance.levelMultiply;
        endLevels.text = "Levels:   " + playerCon.playerLevel.ToString();
        endLevelsPoint.text = level.ToString();

        int coin = playerCon.coins * GameController.Instance.coinMultiply;
        endCoins.text = "Coins:   " + playerCon.coins.ToString();
        endCoinsPoint.text = coin.ToString();

        winBonusPoint.text = "0";

        score = stage + level + coin;
        endPoints.text = "Points:   " + score.ToString();
        SoundController.Instance.Play(SoundController.SoundName.GameOver);
    }

    public void CompleteMode(int winBonus)
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
        controller.SetActive(false);

        endHeader.text = "Stage Complete";

        int stage = (GameController.Instance.currentStage - 1) * GameController.Instance.stageMultiply;
        endStages.text = "Stages:   " + (GameController.Instance.currentStage - 1).ToString();
        endStagesPoint.text = stage.ToString();

        int level = playerCon.playerLevel * GameController.Instance.levelMultiply;
        endLevels.text = "Levels:   " + (playerCon.playerLevel - 1).ToString();
        endLevelsPoint.text = level.ToString();

        int coin = (playerCon.coins - 1) * GameController.Instance.coinMultiply;
        endCoins.text = "Coins:   " + playerCon.coins.ToString();
        endCoinsPoint.text = coin.ToString();

        winBonusPoint.text = winBonus.ToString();

        score = stage + level + coin + winBonus;
        endPoints.text = "Points:   " + score.ToString();
    }

    public void ShowLeaderboard()
    {
        rankManager = GameObject.FindGameObjectWithTag("System").GetComponent<RankManager>();
        rankManager.AddRankData(SystemController.Instance.playerName, score);
        rankManager.SaveRankData();
        for (int i = 0; i < rankManager.playerRank.Count; i++)
        {
            GameObject rank = Instantiate(rankPrefab, rankParent);
            RankData data = rank.GetComponent<RankData>();
            data.playerData = new PlayerData(i + 1, rankManager.playerRank[i].playerName, rankManager.playerRank[i].playerScore);
            data.UpdateData();
        }
    }
    #endregion

    #region Upgrade
    public void StartUpgrade(PlayerUpgrade upgrade)
    {
        ownUpgrade.Add(upgrade);
    }

    public void SelectUpgrade(PlayerUpgrade upgrade)
    {
        playerCon.LevelUpgrade(upgrade);
        if(!ownUpgrade.Contains(upgrade))
        {
            Debug.Log("New Upgrade");
            ownUpgrade.Add(upgrade);
        }
        else
        {
            Debug.Log("Same Upgrade");
        }
        Time.timeScale = 1;
        controller.SetActive(true);
        levelUpgrade.SetActive(false);
        playerCon.CheckExp();
    }

    public void BuyHeal(int cost, int heal)
    {
        playerCon.GetCoin(-cost);
        playerCon.Healing(heal);
        Time.timeScale = 1;
        controller.SetActive(true);
        shop.SetActive(false);
    }

    public void BuyUpgrade(int cost, PlayerUpgrade upgrade)
    {
        playerCon.GetCoin(-cost);
        playerCon.LevelUpgrade(upgrade);
        if (!ownUpgrade.Contains(upgrade))
        {
            ownUpgrade.Add(upgrade);
        }
        Time.timeScale = 1;
        controller.SetActive(true);
        shop.SetActive(false);
    }

    public void FreeUpgrade(PlayerUpgrade upgrade)
    {
        playerCon.LevelUpgrade(upgrade);
        if (!ownUpgrade.Contains(upgrade))
        {
            ownUpgrade.Add(upgrade);
        }
    }

    public void QuitShop()
    {
        Time.timeScale = 1;
        controller.SetActive(true);
        shop.SetActive(false);
    }
    #endregion

    #region Interface
    public void UIStageNumber(int stageNumber)
    {
        this.stageNumber.text = "Stage : " + stageNumber;
    }

    public void UIStageBar(int currentWave, int maxWave)
    {
        waveBar.minValue = 1;
        waveBar.maxValue = maxWave;
        waveBar.value = currentWave;
    }

    public void UISetExp()
    {
        levelBar.minValue = 0;
        levelBar.maxValue = playerCon.maxExp;
        UIChangeExp();
    }

    public void UIChangeExp()
    {
        levelBar.value = playerCon.currentExp;
    }

    public void UIChangeLevel()
    {
        levelNumber.text = "Level : " + playerCon.playerLevel;
    }

    public void UIChangeCoins()
    {
        UIcoins.text = playerCon.coins.ToString();
    }

    public void UISetPlayerHealth()
    {
        playerHealth.GetComponent<Slider>().minValue = 0;
        playerHealth.GetComponent<Slider>().maxValue = playerCon.maxHealth;
        UIChangePlayerHealth();
    }

    public void UIChangePlayerHealth()
    {
        playerHealth.GetComponent<Slider>().value = playerCon.currentHealth;
    }
    #endregion

    #region Camera
    public void TakeCamera()
    {
        canvas.worldCamera = Camera.main;
    }
    public void FinishCamera()
    {

    }
    #endregion

    public void BackToMenu()
    {
        print("Test Back");
        Debug.Log("Test DBack");
        Destroy(Instance.gameObject);
        Destroy(GameController.Instance.gameObject);
        SceneManager.LoadScene(0);
        SoundController.Instance.Play(SoundController.SoundName.MenuBGM);
    }
}
