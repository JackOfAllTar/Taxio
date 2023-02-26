using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string bossTag = "Boss";
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
        else if (other.gameObject.CompareTag(bossTag))
        {
            other.GetComponent<BossController>().TakeDamage(damage);
        }
    }
}
