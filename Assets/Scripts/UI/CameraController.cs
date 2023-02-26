using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = offset + player.position;
    }
}
