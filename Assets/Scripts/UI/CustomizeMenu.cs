using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomizeMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemHeader;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private StarterButton[] starterButtons;

    private void Start()
    {
        SystemController.Instance.CustomizeSetup(itemHeader, itemDescription, starterButtons);
    }
}
