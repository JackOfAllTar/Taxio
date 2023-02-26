using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaper : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string bossTag = "Boss";
    [SerializeField] private string blockTag = "Block";
    private int damage;

    public void SetStats(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag(bossTag))
        {
            other.GetComponent<BossController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag(blockTag))
        {
            Destroy(gameObject);
        }
    }
}
