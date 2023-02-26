using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerProfile profile;
    public UpgradeStats upgradeStats;
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private CharacterController controller;
    private Vector2 movement;
    private Vector2 turning;
    public Quaternion rotation = Quaternion.Euler(Vector3.zero);

    [Header("Action")]
    [SerializeField] private PlayerAction action;
    public bool attack = true;
    [Header("Health")]
    [SerializeField] private CapsuleCollider hitColl;
    public int maxHealth;
    public int currentHealth;
    public bool alive = true;
    [Header("Level")]
    public int playerLevel;
    public int maxExp;
    public int currentExp;
    [Header("Currency")]
    public int coins;
    [Header("Animation")]
    [SerializeField] private Animator anim;

    #region SaveLoad
    public void SetPlayerStats(int level, int maxExp, int currentExp, int coins, UpgradeStats upgradeStats)
    {
        playerLevel = level;
        this.maxExp = maxExp;
        this.currentExp = currentExp;
        this.coins = coins;
        this.upgradeStats = upgradeStats;

        maxHealth = profile.baseHealth + (upgradeStats.healthTier * profile.levelHealth);
        moveSpeed = profile.baseSpeed + (upgradeStats.speedTier * profile.levelSpeed);

        action.SetPlayerAction(upgradeStats);
    }

    public void StartStatus()
    {
        currentHealth = maxHealth;
    }

    public void LoadStatus(int loadHealth)
    {
        currentHealth = loadHealth;
    }

    public void SavePlayerStats()
    {
        GameController.Instance.playerLevel = playerLevel;
        GameController.Instance.maxExp = maxExp;
        GameController.Instance.currentExp = currentExp;
        GameController.Instance.coins = coins;
        GameController.Instance.upgrade = upgradeStats;
        GameController.Instance.currentHealth = currentHealth;
    }
    #endregion

    #region Control
    public void Move(InputAction.CallbackContext callback)
    {
        movement = callback.ReadValue<Vector2>().normalized;
    }

    public void Turn(InputAction.CallbackContext callback)
    {
        turning = callback.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        if(alive)
        {
            //RB.velocity = moveSpeed * new Vector3(movement.x, 0f, movement.y);
            controller.Move(Time.deltaTime * moveSpeed * new Vector3(movement.x, 0f, movement.y));

            float lookAngle;
            if (turning != Vector2.zero)
            {
                lookAngle = Mathf.Atan2(turning.y, turning.x) * -Mathf.Rad2Deg + 90f;
                rotation = Quaternion.Euler(0f, lookAngle, 0f);
                transform.rotation = rotation;
                //RB.rotation = rotation;
            }
            else if (movement != Vector2.zero)
            {
                lookAngle = Mathf.Atan2(movement.y, movement.x) * -Mathf.Rad2Deg + 90f;
                rotation = Quaternion.Euler(0f, lookAngle, 0f);
                transform.rotation = rotation;
                //RB.rotation = rotation;
            }

            anim.SetBool("IsWalking",movement != Vector2.zero);
        }
    }
    #endregion

    #region Pickup

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UIController.Instance.UISetPlayerHealth();
        if(currentHealth <= 0)
        {
            attack = false;
            alive = false;
            GameController.Instance.EndGame();
        }
        SoundController.Instance.Play(SoundController.SoundName.Damage);
    }

    public void GetExp(int expGain)
    {
        currentExp += expGain;
        UIController.Instance.UIChangeExp();
        CheckExp();
    }

    public void CheckExp()
    {
        if (currentExp >= maxExp)
            LevelUp();
    }

    public void GetCoin(int coinGain)
    {
        coins += coinGain;
        UIController.Instance.UIChangeCoins();
    }

    private void LevelUp()
    {
        playerLevel++;
        UIController.Instance.LevelUpgradeMode();
        currentExp -= maxExp;
        maxExp = playerLevel * GameController.Instance.expMultiply;
        UIController.Instance.UISetExp();
        UIController.Instance.UIChangeLevel();
    }
    #endregion

    #region Upgrade
    public void LevelUpgrade(PlayerUpgrade upgrade)
    {
        switch(upgrade)
        {
            case PlayerUpgrade.health:
                upgradeStats.healthTier++;
                maxHealth = profile.baseHealth + (upgradeStats.healthTier * profile.levelHealth);
                currentHealth += profile.levelHealth;
                UIController.Instance.UISetPlayerHealth();
                break;
            case PlayerUpgrade.speed:
                upgradeStats.speedTier++;
                moveSpeed = profile.baseSpeed + (upgradeStats.speedTier * profile.levelSpeed);
                break;
            case PlayerUpgrade.cooldown:
                upgradeStats.cooldownTier++;
                action.SetPlayerAction(upgradeStats);
                break;
            case PlayerUpgrade.paper:
                upgradeStats.paperTier++;
                action.SetPlayerAction(upgradeStats);
                break;
            case PlayerUpgrade.briefcase:
                upgradeStats.briefcaseTier++;
                action.SetPlayerAction(upgradeStats);
                break;
            case PlayerUpgrade.gas:
                upgradeStats.gasTier++;
                action.SetPlayerAction(upgradeStats);
                break;
            case PlayerUpgrade.scythe:
                upgradeStats.scytheTier++;
                action.SetPlayerAction(upgradeStats);
                break;
            case PlayerUpgrade.brick:
                upgradeStats.brickTier++;
                action.SetPlayerAction(upgradeStats);
                break;
        }
    }

    public void Healing(int amount)
    {
        if (currentHealth + amount <= maxHealth)
            currentHealth += amount;
        else
            currentHealth = maxHealth;
        UIController.Instance.UIChangePlayerHealth();
    }
    #endregion
}
