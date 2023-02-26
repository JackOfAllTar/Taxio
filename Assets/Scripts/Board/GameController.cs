using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField] private string sceneName = "PlayScene";
    [SerializeField] private Scene playScene;
    [SerializeField] private BoardController boardController;

    [Header("Stage")]
    public int currentStage = 1;
    private int maxWave;
    private int currentWave = 1;
    private int stageType;
    [SerializeField] private int maxStage = 5;
    private int currentEnemies = 0;
    private bool firstStart = false;

    [Header("Random")]
    [SerializeField] private int shopMax = 8;
    private int shopChance = 0;

    [Header("PlayerStat")]
    public int expMultiply = 5;
    public int playerLevel = 1;
    public int maxExp;
    public int currentExp = 0;
    public UpgradeStats upgrade = new UpgradeStats(0, 0, 0, 0, 0, 0, 0, 0, 0);
    public enum AttackType
    {
        paper,
        briefcase,
        gas,
        scythe,
        brick
    }
    private AttackType starterAttack = AttackType.paper;
    public int coins = 0;
    public int currentHealth;
    public int maxUpgradeCount = 4;
    //public int maxUpgradeTier = 5;

    [Header("EndGameStat")]
    public int stageMultiply = 50;
    public int levelMultiply = 10;
    public int coinMultiply = 1;
    public int winBonus = 100;

    #region Start
    private void Start()
    {
        SceneManager.sceneLoaded += NextLevel;
        starterAttack = SystemController.Instance.attackType;
        switch (starterAttack)
        {
            case AttackType.paper:
                upgrade.paperTier++;
                UIController.Instance.StartUpgrade(PlayerUpgrade.paper);
                break;
            case AttackType.briefcase:
                upgrade.briefcaseTier++;
                UIController.Instance.StartUpgrade(PlayerUpgrade.briefcase);
                break;
            case AttackType.gas:
                upgrade.gasTier++;
                UIController.Instance.StartUpgrade(PlayerUpgrade.gas);
                break;
            case AttackType.scythe:
                upgrade.scytheTier++;
                UIController.Instance.StartUpgrade(PlayerUpgrade.scythe);
                break;
            case AttackType.brick:
                upgrade.brickTier++;
                UIController.Instance.StartUpgrade(PlayerUpgrade.brick);
                break;
        }
        StartLevel(delegate { firstStart = true; print($"Start First Level"); });
        boardController.NewPlayerStatus();
        UIController.Instance.UISetPlayerHealth();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= NextLevel;
    }

    private void StartLevel(Action callback = null)
    {
        UIController.Instance.UIStageNumber(currentStage);

        int subStage = currentStage % 10;
        if (subStage == 0)
        {
            SoundController.Instance.Play(SoundController.SoundName.BossBGM);
            stageType = 2;
            maxWave = 2;
            shopChance++;
        }
        else
        {
            int shopRange = UnityEngine.Random.Range(1, shopMax);
            if (shopRange <= shopChance)
            {
                SoundController.Instance.Play(SoundController.SoundName.PlayBGM);
                stageType = 1;
                maxWave = 1;
                shopChance -= shopMax;
            }
            else
            {
                SoundController.Instance.Play(SoundController.SoundName.PlayBGM);
                stageType = 0;
                if (subStage > 6)
                    maxWave = 4;
                else if (subStage > 3)
                    maxWave = 3;
                else
                    maxWave = 2;
                shopChance++;
            }
        }

        maxExp = playerLevel * expMultiply;
        boardController.SetUpStage(stageType, currentStage);
        boardController.SetupPlayer();
        boardController.LoadPlayerStats(Instance.playerLevel, Instance.maxExp, Instance.currentExp, Instance.coins, Instance.upgrade);
        boardController.LoadPlayerStatus(currentHealth);
        CheckWave();
        callback?.Invoke();
    }

    private void CheckWave()
    {
        currentEnemies = 0;
        switch (stageType)
        {
            case 0:
                if (currentWave == maxWave)
                    boardController.EndWave();
                else
                    boardController.SetupSceneFight(Instance.currentStage, currentWave);
                break;
            case 1:
                boardController.SetupSceneShop();
                break;
            case 2:
                if (currentWave == maxWave)
                    boardController.EndWave();
                else
                {
                    boardController.SetupSceneBoss();
                }
                break;
            default:
                boardController.SetupSceneBlank();
                break;
        }
        UIController.Instance.UIStageBar(currentWave, maxWave);
    }

    public void AddEnemy()
    {
        currentEnemies++;
    }
    #endregion

    #region InGame
    public void DownEnemy()
    {
        currentEnemies--;
        if (currentEnemies <= 0)
        {
            NextWave();
        }
    }

    public void BossDown()
    {
        CheckWave();
    }

    private void NextWave()
    {
        currentWave++;
        CheckWave();
    }
    #endregion

    #region EndGame
    public void SavePlayerStats(int level, int maxExp, int currentExp)
    {
        playerLevel = level;
        this.maxExp = maxExp;
        this.currentExp = currentExp;
    }

    public void EnterDoor()
    {
        if (currentStage == maxStage)
            CompleteGame();
        else
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void NextLevel(Scene arg0, LoadSceneMode arg1)
    {
        if (firstStart == false) return;
        currentStage++;
        currentWave = 1;
        StartLevel(delegate { print($"Start Level({currentStage})"); });
    }

    public void EndGame()
    {
        UIController.Instance.GameOverMode();
    }

    private void CompleteGame()
    {
        UIController.Instance.CompleteMode(winBonus);
    }
    #endregion
}
