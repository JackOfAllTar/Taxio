using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private CapsuleCollider shopColl;
    [Header("Healing")]
    [SerializeField] private int minHealAmount = 2;
    [SerializeField] private int maxHealAmount = 5;
    [Header("Cost")]
    [SerializeField] private int minCost = 5;
    [SerializeField] private int maxCost = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            shopColl.enabled = false;
            UIController.Instance.ShopUpgradeMode(minHealAmount, maxHealAmount, minCost, maxCost);
        }
    }
}
