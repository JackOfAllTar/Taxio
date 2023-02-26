using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyProfile profile;
    private int currentHealth;
    private bool isKnock = false;
    private float knockTimer = 0;
    [SerializeField] private float knockTime = 0.5f;
    [SerializeField] private float knockUp = 25f;
    [Header("Attack")]
    [SerializeField] private EnemyAttack attack;
    private int currentAttack;
    private float attackTimer;
    [Header("Fire")]
    private int fireCount = 0;
    private float fireTimer = 0;
    private int fireDamage = 0;
    [Header("Drop")]
    [SerializeField] private GameObject expPrefab;
    [SerializeField] private GameObject coinPrefab;
    [Header("Component")]
    [SerializeField] NavMeshAgent navigator;
    [SerializeField] Rigidbody rb;
    private GameObject player;
    private PlayerController playerController;

    public void SpawnEnemy(GameObject player)
    {
        int multiplier = GameController.Instance.currentStage / 10;
        currentHealth = profile.baseHealth + (profile.levelHealth * multiplier);
        currentAttack = profile.baseAttack + (profile.levelAttack * multiplier);
        navigator.speed = profile.baseSpeed + (profile.levelSpeed * multiplier);

        this.player = player;
        if (player != null)
        {
            playerController = this.player.GetComponent<PlayerController>();
        }
    }

    #region State
    private void Update()
    {
        if (player != null)
        {
            if (currentHealth > 0 && playerController.currentHealth > 0 && !isKnock)
            {
                navigator.SetDestination(player.transform.position);
            }
            else
            {
                navigator.enabled = false;
            }

            if(attack.target && attackTimer == 0)
            {
                playerController.TakeDamage(currentAttack);
                attackTimer += profile.attackSpeed;
            }
            if(fireCount > 0 && fireTimer == 0)
            {
                TakeDamage(fireDamage);
                fireTimer++;
            }
            if (knockTimer == 0 && isKnock)
            {
                isKnock = false;
                navigator.enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else if (attackTimer < 0)
            attackTimer = 0;

        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;
        else if (fireTimer < 0)
            fireTimer = 0;

        if (knockTimer > 0)
            knockTimer -= Time.deltaTime;
        else if (knockTimer < 0)
            knockTimer = 0;
    }
    #endregion

    #region Health
    public void FireDamageIn(int damage)
    {
        fireCount++;
        if(fireDamage < damage)
        {
            fireDamage = damage;
        }
    }

    public void FireDamageOut()
    {
        fireCount--;
        if(fireCount == 0)
        {
            fireDamage = 0;
        }
    }

    public void Knockback(float force, Vector3 knockbackPos, int damage)
    {
        isKnock = true;
        knockTimer += knockTime;
        //Vector3 floatKnock = new Vector3(0, knockUp, 0);
        rb.AddForce((knockbackPos * force));
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        SoundController.Instance.Play(SoundController.SoundName.Death);
        currentHealth -= damage;
        if (currentHealth <= 0)
            Death();
    }

    private void Death()
    {
        bool dropCoin = Random.Range(0, 2) == 0;
        Vector3 dropPos = transform.position + new Vector3(0, 1, 0);
        if (dropCoin)
        {
            int amount = Random.Range(profile.minCoinDrop, profile.maxCoinDrop + 1);
            if (amount > 0)
            {
                GameObject coinDrop = Instantiate(coinPrefab, dropPos, transform.rotation);
                coinDrop.GetComponent<Pickup>().SetValue(amount);
            }
        }
        else
        {
            int amount = Random.Range(profile.minExpDrop, profile.maxExpDrop + 1);
            if (amount > 0)
            {
                GameObject expDrop = Instantiate(expPrefab, dropPos, transform.rotation);
                expDrop.GetComponent<Pickup>().SetValue(amount);
            }
        }
        GameController.Instance.DownEnemy();
        Destroy(gameObject);
    }
    #endregion
}
