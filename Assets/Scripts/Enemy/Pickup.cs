using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int point = 1;
    [SerializeField] private string playerTag = "Player";
    private enum PickupType{exp, coin };
    [SerializeField] private PickupType type;

    public void SetValue(int amount)
    {
        point = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            if(type == PickupType.exp)
                other.GetComponent<PlayerController>().GetExp(point);
            if (type == PickupType.coin)
                other.GetComponent<PlayerController>().GetCoin(point);
            SoundController.Instance.Play(SoundController.SoundName.Pickup);
            Destroy(gameObject);
        }
    }
}
