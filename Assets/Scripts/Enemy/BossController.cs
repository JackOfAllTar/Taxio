using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossProfile profile;
    [Header("Health")]
    private int currentHealth;
    private float stunTimer;
    [Header("Attack")]
    [SerializeField] private EnemyAttack attack;
    private float actionTimer;
    private enum ActionState {walk, shoot, dash , stun}
    private ActionState actionState = ActionState.walk;
    private Vector3 playerTarget;
    private float currentDashSpeed;
    private int currentDashDamage;
    [Header("Shoot")]
    [SerializeField] private GameObject shootBulletPrefab;
    [SerializeField] private Transform shootPoint;
    private float currentShootSpeed;
    private int currentShootCount;
    private int currentShootDamage;
    private float currentBulletSpeed;
    private int shootLeft = 0;
    private float shootTimer = 0;
    [Header("Fire")]
    private int fireCount = 0;
    private float fireTimer = 0;
    private int fireDamage = 0;
    [Header("Drop")]
    [SerializeField] private GameObject exp;
    [SerializeField] private GameObject coin;
    [Header("Component")]
    [SerializeField] NavMeshAgent navigator;
    [SerializeField] Rigidbody rb;
    private GameObject player;
    private PlayerController playerController;

    public void SpawnBoss(GameObject player)
    {
        int multiplier = GameController.Instance.currentStage / 10;
        currentHealth = profile.baseHealth + (profile.levelHealth * multiplier);
        navigator.speed = profile.baseSpeed + (profile.levelSpeed * multiplier);
        currentDashSpeed = profile.baseDashSpeed + (profile.levelDashSpeed * multiplier);
        currentDashDamage = profile.baseDashDamage + (profile.levelDashDamage * multiplier);
        currentShootSpeed = profile.baseFireSpeed + (profile.levelFireSpeed * multiplier);
        currentShootCount = profile.baseFireCount + (profile.levelFireCount * multiplier);
        currentShootDamage = profile.baseFireDamage + (profile.levelFireDamage * multiplier);
        currentBulletSpeed = profile.baseFireBulletSpeed + (profile.levelFireBulletSpeed * multiplier);

        this.player = player;
        if(player != null)
        {
            playerController = this.player.GetComponent<PlayerController>();
        }
    }

    #region State
    private void Update()
    {
        if(player != null)
        {
            switch(actionState)
            {
                case ActionState.walk:
                    {
                        if (currentHealth > 0 && playerController.currentHealth > 0)
                        {
                            navigator.SetDestination(player.transform.position);
                            rb.isKinematic = true;
                        }
                        else
                            navigator.enabled = false;

                        if (actionTimer >= profile.actionCooldown)
                        {
                            if (attack.target)
                            {
                                actionState = ActionState.dash;
                                Dash();
                            }
                            else
                            {
                                actionState = ActionState.shoot;
                                ShootState();
                            }
                        }
                    }
                    break;
                case ActionState.stun:
                    rb.isKinematic = true;
                    break;
                default:
                    navigator.enabled = false;
                    break;
            }
            if (fireCount > 0 && fireTimer == 0)
            {
                TakeDamage(fireDamage);
                fireTimer++;
            }
        }
    }
    
    private void FixedUpdate()
    {
        switch(actionState)
        {
            case ActionState.walk:
                actionTimer += Time.deltaTime;
                break;
            case ActionState.stun:
                stunTimer += Time.deltaTime;
                if(stunTimer > profile.stunTime)
                {
                    navigator.enabled = true;
                    actionState = ActionState.walk;
                }
                break;
            case ActionState.shoot:
                if(shootLeft > 0)
                {
                    shootTimer += Time.deltaTime;
                    if (shootTimer >= currentShootSpeed)
                        Fire();
                }
                else
                {
                    navigator.enabled = true;
                    actionState = ActionState.walk;
                }
                break;
            case ActionState.dash:
                transform.localPosition += currentDashSpeed * Time.deltaTime * transform.forward;
                rb.velocity = playerTarget * currentDashSpeed;
                break;
            default:
                break;
        }
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;
        else if (fireTimer < 0)
            fireTimer = 0;
    }

    #endregion

    #region Health
    public void FireDamageIn(int damage)
    {
        fireCount++;
        if (fireDamage < damage)
        {
            fireDamage = damage;
        }
    }

    public void FireDamageOut()
    {
        fireCount--;
        if (fireCount == 0)
        {
            fireDamage = 0;
        }
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
        Vector3 dropTransform = transform.position + new Vector3(0, 1.5f, 0);

        int coinAmount = Random.Range(profile.minCoinDrop, profile.maxCoinDrop + 1);
        if (coinAmount > 0)
        {
            GameObject coinDrop = Instantiate(coin, dropTransform, transform.rotation);
            coinDrop.GetComponent<Pickup>().SetValue(coinAmount);
        }
        int expAmount = Random.Range(profile.minExpDrop, profile.maxExpDrop + 1);
        if (expAmount > 0)
        {
            GameObject expDrop = Instantiate(exp, dropTransform, transform.rotation);
            expDrop.GetComponent<Pickup>().SetValue(expAmount);
        }
        GameController.Instance.DownEnemy();
        Destroy(gameObject);
    }
    #endregion

    #region Action

    private void Dash()
    {
        rb.isKinematic = false;

        actionTimer = 0;

        HeadToTarget();

        actionState = ActionState.dash;
    }

    private void HeadToTarget()
    {
        playerTarget = player.transform.position - transform.position;
        playerTarget.y = 3;
        playerTarget.Normalize();

        float targetAngle = Mathf.Atan2(playerTarget.z, playerTarget.x) * -Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (actionState == ActionState.dash)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerController.TakeDamage(currentDashDamage);
            }
            else if(collision.gameObject.CompareTag("Block"))
            {
                actionState = ActionState.stun;
                stunTimer = 0;
            }
        }
    }

    private void ShootState()
    {
        actionTimer = 0;
        actionState = ActionState.shoot;
        shootLeft += currentShootCount;
    }

    private void Fire()
    {
        shootLeft--;
        shootTimer = 0;

        HeadToTarget();

        float attackRecoil = Random.Range(- profile.fireRecoil / 2, profile.fireRecoil / 2);
        float targetAngle = Mathf.Atan2(playerTarget.z, playerTarget.x) * -Mathf.Rad2Deg + 90f;
        Quaternion rotation = Quaternion.Euler(0, targetAngle + attackRecoil, 0);
        transform.rotation = rotation;

        GameObject bullet = Instantiate(shootBulletPrefab, shootPoint.position, rotation);
        bullet.GetComponent<EnemyBullet>().SetBullet(currentShootDamage);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * currentBulletSpeed, ForceMode.Impulse);
        Destroy(bullet, 3f);
    }
    #endregion
}
