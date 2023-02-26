using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemController : Singleton<SystemController>
{
    public string playerName = "Kum";
    [Header("Customize")]
    [SerializeField] private ItemProfile profile;
    private TextMeshProUGUI itemHeader;
    private TextMeshProUGUI itemDescription;
    public GameController.AttackType attackType = GameController.AttackType.paper;
    private StarterButton[] starterButton;
    private StarterButton currentStarter;

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void CustomizeSetup(TextMeshProUGUI itemHeader, TextMeshProUGUI itemDescription, StarterButton[] starterButton)
    {
        this.itemHeader = itemHeader;
        this.itemDescription = itemDescription;
        this.starterButton = starterButton;
        StarterAttack(0);
    }

    public void StarterAttack(int typeNum)
    {
        switch (typeNum)
        {
            case 0:
                attackType = GameController.AttackType.paper;
                itemHeader.text = profile.paper.name;
                itemDescription.text = profile.paper.description;
                break;
            case 1:
                attackType = GameController.AttackType.briefcase;
                itemHeader.text = profile.briefcase.name;
                itemDescription.text = profile.briefcase.description;
                break;
            case 2:
                attackType = GameController.AttackType.gas;
                itemHeader.text = profile.gas.name;
                itemDescription.text = profile.gas.description; 
                break;
            case 3:
                attackType = GameController.AttackType.scythe;
                itemHeader.text = profile.scythe.name;
                itemDescription.text = profile.scythe.description;
                break;
            case 4:
                attackType = GameController.AttackType.brick;
                itemHeader.text = profile.brick.name;
                itemDescription.text = profile.brick.description;
                break;
        }
        starterButton[typeNum].SelectedStarter();
        if (currentStarter != null)
            currentStarter.UnselectedStarter();
        currentStarter = starterButton[typeNum];
    }
}
