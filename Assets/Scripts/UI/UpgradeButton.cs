using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    private int currentCoins;
    [Header("Upgrade")]
    [SerializeField] private PlayerUpgrade upgrade;
    [SerializeField] private UpgradeProfile profile;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonCost;
    private int upgradeCost;
    [Header("Heal")]
    [SerializeField] private TextMeshProUGUI buttonHeal;
    [SerializeField] private Sprite healSprite;
    private int healNum;
    [Header("Quit")]
    [SerializeField] private Sprite quitSprite;

    public void SetCoin(int coins)
    {
        currentCoins = coins;
    }

    public void SetUpgrade(int cost, PlayerUpgrade upgrade)
    {
        upgradeCost = cost;
        this.upgrade = upgrade;
        if(upgradeCost > 0)
        {
            buttonCost.enabled = true;
            buttonCost.text = upgradeCost.ToString();
        }
        else
            buttonCost.enabled = false;
        buttonImage.sprite = FindImage(upgrade);
        buttonHeal.enabled = false;
    }

    private Sprite FindImage(PlayerUpgrade upgrade)
    {
        Sprite sprite = upgrade switch
        {
            PlayerUpgrade.health => profile.healthUpgrade.sprite,
            PlayerUpgrade.speed => profile.speedUpgrade.sprite,
            PlayerUpgrade.cooldown => profile.cooldownUpgrade.sprite,
            PlayerUpgrade.damage => profile.damageUpgrade.sprite,
            PlayerUpgrade.paper => profile.paperUpgrade.sprite,
            PlayerUpgrade.briefcase => profile.briefcaseUpgrade.sprite,
            PlayerUpgrade.gas => profile.gasUpgrade.sprite,
            PlayerUpgrade.scythe => profile.scytheUpgrade.sprite,
            PlayerUpgrade.brick => profile.brickUpgrade.sprite,
            PlayerUpgrade.blank => profile.blankUpgrade.sprite,
            _ => profile.blankUpgrade.sprite,
        };
        return sprite;
    }

    public void SetHeal(int cost, int heal)
    {
        upgradeCost = cost;
        healNum = heal;
        if (upgradeCost > 0)
        {
            buttonCost.enabled = true;
            buttonCost.text = upgradeCost.ToString();
        }
        else
            buttonCost.enabled = false;
        buttonImage.sprite = healSprite;
        buttonHeal.enabled = true;
        buttonHeal.text = healNum.ToString();
    }

    public void SetQuit()
    {
        buttonImage.sprite = quitSprite;
        buttonCost.enabled = false;
        buttonHeal.enabled = false;
    }

    public void UpgradeSelected()
    {
        UIController.Instance.SelectUpgrade(upgrade);
        SoundController.Instance.Play(SoundController.SoundName.Upgrade);
    }

    public void FreeUpgrade()
    {
        UIController.Instance.FreeUpgrade(upgrade);
        SoundController.Instance.Play(SoundController.SoundName.Upgrade);
    }

    public void BuyHealSelected()
    {
        if (upgradeCost <= currentCoins)
        {
            UIController.Instance.BuyHeal(upgradeCost, healNum);
            SoundController.Instance.Play(SoundController.SoundName.Buy);
        }
    }

    public void BuyUpgradeSelected()
    {
        if (upgradeCost <= currentCoins)
        {
            UIController.Instance.BuyUpgrade(upgradeCost, upgrade);
            SoundController.Instance.Play(SoundController.SoundName.Buy);
        }
    }

    public void QuitShopSelected()
    {
        UIController.Instance.QuitShop();
    }
}
