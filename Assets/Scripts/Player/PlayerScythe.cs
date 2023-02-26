using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScythe : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject scytheObject;
    private Transform anchor;

    public void Setstats(int damage, float range, Transform anchor)
    {
        scytheObject.GetComponent<Scythe>().damage = damage;
        scytheObject.transform.localPosition = new Vector3(0, 0, range);
        this.anchor = anchor;
    }

    /*private void FixedUpdate()
    {
        if(anchor != null)
        {
            transform.position = anchor.position;
        }
    }*/
}
