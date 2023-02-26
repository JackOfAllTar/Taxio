using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profile/Upgrade Profile")]
public class UpgradeProfile : ScriptableObject
{
    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public PlayerUpgrade upgrade;
        public Sprite sprite;
    }

    public Upgrade blankUpgrade;
    public Upgrade healthUpgrade;
    public Upgrade speedUpgrade;
    public Upgrade cooldownUpgrade;
    public Upgrade damageUpgrade;
    public Upgrade paperUpgrade;
    public Upgrade briefcaseUpgrade;
    public Upgrade gasUpgrade;
    public Upgrade scytheUpgrade;
    public Upgrade brickUpgrade;
}
