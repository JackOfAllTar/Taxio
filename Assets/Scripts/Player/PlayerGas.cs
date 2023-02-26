using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Profiling;

public class PlayerGas : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string bossTag = "Boss";
    [SerializeField] private string blockTag = "Block";
    [SerializeField] private string floorTag = "Floor";
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider coll;
    [SerializeField] private GameObject fireParticle;
    private int damage;
    private float radius;
    private float duration;
    private bool isFlame = false;

    public void SetStats(int damage, float radius, float duration)
    {
        this.damage = damage;
        this.radius = radius;
        this.duration = duration;
        Destroy(gameObject, 3 + duration);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(isFlame)
        {
            if (other.gameObject.CompareTag(enemyTag))
            {
                other.GetComponent<EnemyController>().FireDamageIn(damage);
            }
            else if (other.gameObject.CompareTag(bossTag))
            {
                other.GetComponent<BossController>().FireDamageIn(damage);
            }
        }
        else
        {
            if (other.gameObject.CompareTag(blockTag))
            {
                rb.velocity = Vector3.zero;
            }
            if (other.gameObject.CompareTag(floorTag))
            {
                rb.velocity = Vector3.zero;
                Flame();
            }
        }
    }

    private void Flame()
    {
        rb.useGravity = false;
        coll.radius = radius;
        fireParticle.SetActive(true);
        var particleRadius = fireParticle.GetComponent<ParticleSystem>().shape;
        particleRadius.radius = radius;
        isFlame = true;
        Destroy(gameObject, duration);
    }
}
