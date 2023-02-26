using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStats
{
    public int healthTier;
    public int speedTier;
    public int cooldownTier;
    public int damageTier;
    public int paperTier;
    public int briefcaseTier;
    public int gasTier;
    public int scytheTier;
    public int brickTier;

    public UpgradeStats(int health, int speed, int cooldown, int damage, int paper, int briefcase, int gas, int scythe, int brick)
    {
        healthTier = health;
        speedTier = speed;
        cooldownTier = cooldown;
        damageTier = damage;
        paperTier = paper;
        briefcaseTier = briefcase;
        gasTier = gas;
        scytheTier = scythe;
        brickTier = brick;
    }
}
