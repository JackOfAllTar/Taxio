using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterButton : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private int attackType;

    public void OnClick()
    {
        SystemController.Instance.StarterAttack(attackType);
    }

    public void SelectedStarter()
    {
        image.sprite = selectedSprite;
        button.enabled = false;
    }

    public void UnselectedStarter()
    {
        image.sprite = unselectedSprite;
        button.enabled = true;
    }
}
