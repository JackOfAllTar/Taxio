using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private int boardWidth = 20;
    [SerializeField] private int boardLength = 30;
    [SerializeField] private float spawnOffset = 1;
    [Header("Stage")]
    [SerializeField] private GameObject[] obstacleObject;
    [Header("Player")]
    [SerializeField] private string playerTag = "Player";
    private GameObject player;
    [SerializeField] private string spawnTag = "Respawn";
    private Transform playerSpawn;
    [Header("Obstacle")]
    [SerializeField] private string obstacleTag = "Obstacle";
    [Header("Enemy")]
    [SerializeField] private int baseEnemyCount = 1;
    [SerializeField] private GameObject enemySpawnerPrefab;
    private List<Vector3> spawnBlock = new List<Vector3>();
    [SerializeField] private float enemySpawnDelay = 1.5f;
    private GameObject[] enemySpawner;
    [SerializeField] private GameObject bossPrefab;
    [Header("Exit")]
    [SerializeField] private string exitTag = "Finish";
    [SerializeField] private GameObject exitLight;
    private GameObject exitPrefab;
    [Header("Shop")]
    [SerializeField] private GameObject shopPrefab;

    public void SetUpStage(int stageType, int stageLevel)
    {
        int stageOnce = stageLevel % 10;
        if(stageType == 0)
        {
            if(stageOnce != 1 && stageOnce != 6)
            {
                int randomObstacle = Random.Range(0, obstacleObject.Length);
                Instantiate(obstacleObject[randomObstacle]);
            }
        }
    }

    #region Player
    public void SetupPlayer()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        playerSpawn = GameObject.FindGameObjectWithTag(spawnTag).transform;
        //Set player position
        playerSpawn.position = new Vector3(0, 0, ((float)boardLength / 4) - ((float)boardLength / 2));
        //Spawn player
        player.transform.position = playerSpawn.position;
    }

    public void NewPlayerStatus()
    {
        player.GetComponent<PlayerController>().StartStatus();
    }

    public void LoadPlayerStatus(int currentHealth)
    {
        player.GetComponent<PlayerController>().LoadStatus(currentHealth);
    }

    public void LoadPlayerStats(int level, int maxExp, int currentExp, int coins, UpgradeStats upgrade)
    {
        //Restore player stats
        player.GetComponent<PlayerController>().SetPlayerStats(level, maxExp, currentExp, coins, upgrade);
    }
    #endregion

    #region Scene
    public void SetupSceneFight(int level, int wave)
    {
        //calculate enemies number
        int enemyCount = baseEnemyCount + level + wave;
        spawnBlock.Clear();
        //Set enemies spawner
        Vector2 spawnWidth = new Vector2(-(boardWidth - spawnOffset) / 2, (boardWidth - spawnOffset) / 2);
        Vector2 spawnLength = new Vector2(-(boardLength - spawnOffset) / 4, (boardLength - spawnOffset) / 2);

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag(obstacleTag);
        foreach (GameObject obstacle in obstacles)
        {
            spawnBlock.Add(obstacle.transform.position);
        }

        enemySpawner = new GameObject[enemyCount];

        for (int i = 0; i < enemyCount; i++)
        {
            int randomWidth;
            int randomLength;
            Vector3 randomPos;
            do
            {
                randomWidth = Random.Range((int)spawnWidth.x, (int)spawnWidth.y);
                randomLength = Random.Range((int)spawnLength.x, (int)spawnLength.y);
                randomPos = new Vector3(randomWidth, 0, randomLength);
            } 
            while (spawnBlock.Contains(randomPos));
            spawnBlock.Add(randomPos);
            Vector3 spawnPoint = randomPos;

            enemySpawner[i] = Instantiate(enemySpawnerPrefab, spawnPoint, Quaternion.identity);
            enemySpawner[i].GetComponent<EnemySpawner>().StartSpawn(player, enemySpawnDelay);
        }

        //Set exit
        exitPrefab = GameObject.FindGameObjectWithTag(exitTag);
        exitPrefab.GetComponent<BoxCollider>().isTrigger = false;
        //Set UI
        UIController.Instance.PlayMode(player);
    }

    public void SetupSceneShop()
    {
        Vector3 shopPos = new Vector3(0, 0, (float)boardLength / 8);
        Instantiate(shopPrefab, shopPos, Quaternion.identity);
        exitPrefab = GameObject.FindGameObjectWithTag(exitTag);
        exitPrefab.GetComponent<BoxCollider>().isTrigger = true;
        UIController.Instance.PlayMode(player);
        UIController.Instance.RestMode();
    }

    public void SetupSceneBoss()
    {
        Vector3 bossPos = new Vector3(0, 1.5f, (float)boardLength / 4);

        GameObject boss = Instantiate(bossPrefab, bossPos, Quaternion.identity);
        boss.GetComponent<BossController>().SpawnBoss(player);

        exitPrefab = GameObject.FindGameObjectWithTag(exitTag);
        exitPrefab.GetComponent<BoxCollider>().isTrigger = false;
        UIController.Instance.PlayMode(player);
    }

    public void SetupSceneBlank()
    {
        exitPrefab.GetComponent<BoxCollider>().isTrigger = false;
    }

    public void EndWave()
    {
        //Set exit
        UIController.Instance.RestMode();
        exitPrefab.GetComponent<BoxCollider>().isTrigger = true;
        Instantiate(exitLight);
    }
    #endregion
}
