using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToNextLevel : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            other.gameObject.GetComponent<PlayerController>().SavePlayerStats();
            GameController.Instance.EnterDoor();
        }
    }
}
