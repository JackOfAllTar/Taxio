using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FillName : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void EditName()
    {
        SystemController.Instance.SetPlayerName(inputField.text);
    }

    public void EnterSetting()
    {
        inputField.text = SystemController.Instance.playerName;
    }
}
