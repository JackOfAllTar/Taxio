using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profile/Item Profile")]
public class ItemProfile : ScriptableObject
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public string description;
    }

    public Item paper;
    public Item briefcase;
    public Item gas;
    public Item scythe;
    public Item brick;
}
