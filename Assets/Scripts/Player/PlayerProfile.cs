using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profile/Player Profile")]
public class PlayerProfile : ScriptableObject
{
    [Header("Health Stats")]
    public int baseHealth;
    public int levelHealth;
    [Header("Speed Stats")]
    public float baseSpeed;
    public float levelSpeed;
    [Header("Action")]
    public float baseActionCooldown;
    public float levelActionCooldown;
    public float baseActionDamage;
    public float levelActionDamage;
    [Header("Paper")]
    public int basePaperCount;
    public int levelPaperCount;
    public float paperBulletSpeed;
    public int paperDamage;
    [Header("Briefcase")]
    public int baseWallBounce;
    public int levelWallBounce;
    public float briefSpeed;
    public int briefDamage;
    [Header("Gas")]
    public float baseGasRange;
    public float levelGasRange;
    public float gasSpeed;
    public float gasTime;
    public int gasDamage;
    [Header("Scythe")]
    public float baseScytheSpeed;
    public float levelScytheSpeed;
    public float scytheRange;
    public int scytheDamage;
    public float scytheDuration;
    [Header("Brick")]
    public float baseBrickKnockback;
    public float levelBrickKnockback;
    public float brickForce;
    public int brickDamage;
}
