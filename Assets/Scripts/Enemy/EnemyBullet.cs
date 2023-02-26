using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string blockTag = "Block";
    private int damage = 0;

    public void SetBullet(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag(blockTag))
        {
            Destroy(gameObject);
        }
    }
}
