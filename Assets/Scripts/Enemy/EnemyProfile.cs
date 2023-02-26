using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profile/Enemy Profile")]
public class EnemyProfile : ScriptableObject
{
    [Header("Health Stats")]
    public int baseHealth;
    public int levelHealth;
    [Header("Speed Stats")]
    public float baseSpeed;
    public float levelSpeed;
    [Header("Attack Stats")]
    public int baseAttack;
    public int levelAttack;
    public int attackSpeed;
    [Header("Drop")]
    public int minExpDrop;
    public int maxExpDrop;
    public int minCoinDrop;
    public int maxCoinDrop;
}
