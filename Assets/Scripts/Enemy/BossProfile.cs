using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profile/Boss Profile")]
public class BossProfile : ScriptableObject
{
    [Header("Health Stats")]
    public int baseHealth;
    public int levelHealth;
    [Header("Speed Stats")]
    public float baseSpeed;
    public float levelSpeed;
    [Header("Action")]
    public float actionCooldown;
    [Header("Firing Stats")]
    public int baseFireCount;
    public int levelFireCount;
    public float baseFireSpeed;
    public float levelFireSpeed;
    public int baseFireDamage;
    public int levelFireDamage;
    public float baseFireBulletSpeed;
    public float levelFireBulletSpeed;
    public float fireRecoil;
    [Header("Dashing Stats")]
    public int stunTime;
    public float baseDashSpeed;
    public float levelDashSpeed;
    public int baseDashDamage;
    public int levelDashDamage;
    [Header("Drop")]
    public int minExpDrop;
    public int maxExpDrop;
    public int minCoinDrop;
    public int maxCoinDrop;
}
