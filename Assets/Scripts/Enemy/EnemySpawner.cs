using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject player;
    private float delay;

    public void StartSpawn(GameObject player, float delay)
    {
        this.player = player;
        this.delay = delay;
        Invoke("EndSpawn", delay);
    }

    private void EndSpawn()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemy.GetComponent<EnemyController>().SpawnEnemy(player);
        GameController.Instance.AddEnemy();
        Destroy(gameObject, delay);
    }
}
