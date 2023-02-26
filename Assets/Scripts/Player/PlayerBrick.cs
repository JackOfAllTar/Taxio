using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrick : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string bossTag = "Boss";
    [SerializeField] private string blockTag = "Block";
    [SerializeField] private string floorTag = "Floor";
    [SerializeField] private Rigidbody rb;
    private int damage;
    private float knockback;
    private Vector3 velocityNormalize;
    private bool isGround = false;

    public void SetStats(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
        Vector3 knockbackPos = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        velocityNormalize = knockbackPos.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGround)
        {
            if (other.gameObject.CompareTag(enemyTag))
            {
                other.GetComponent<EnemyController>().Knockback(knockback, velocityNormalize, damage);
            }
            else if (other.gameObject.CompareTag(bossTag))
            {
                other.GetComponent<BossController>().TakeDamage(damage);
            }
            else if (other.gameObject.CompareTag(blockTag))
            {
                rb.velocity = Vector3.zero;
            }
            else if (other.gameObject.CompareTag(floorTag))
            {
                rb.velocity = Vector3.zero;
                Drop();
            }
        }
    }

    private void Drop()
    {
        rb.useGravity = false;
        isGround = true;
    }
}
