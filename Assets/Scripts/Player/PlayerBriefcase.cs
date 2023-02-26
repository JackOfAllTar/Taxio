using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerBriefcase : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string bossTag = "Boss";
    [SerializeField] private string blockTag = "Block";
    private int damage;
    private int bounce;
    private float speed;
    [Header("Bounce")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float direction;
    
    public void SetStats(int damage, int bounce, float speed)
    {
        this.damage = damage;
        this.bounce = bounce;
        this.speed = speed;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag(enemyTag))
        {
            coll.GetComponent<EnemyController>().TakeDamage(damage);
        }
        else if(coll.gameObject.CompareTag(bossTag))
        {
            coll.GetComponent<BossController>().TakeDamage(damage);
        }
        else if(coll.gameObject.CompareTag(blockTag))
        {
            if (bounce > 0)
            {
                bounce--;
                Bounce(coll.transform.position);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Bounce(Vector3 colliderPos)
    {
        Vector3 triggerPos = transform.position;
        Vector3 collNetPos = colliderPos - triggerPos;

        if(-1 > collNetPos.x)
            collNetPos.x = 0;
        else if(collNetPos.x > 1)
            collNetPos.x = 0;
        if (-1 > collNetPos.y)
            collNetPos.y = 0;
        else if (collNetPos.y > 1)
            collNetPos.y = 0;
        if (-1 > collNetPos.z)
            collNetPos.z = 0;
        else if (collNetPos.z > 1)
            collNetPos.z = 0;

        collNetPos.Normalize();

        Vector3 dirNormalize = rb.velocity.normalized;
        float squareRoot = Mathf.Sqrt(2) / 2;

        dirNormalize.x *= collNetPos.x > -squareRoot && collNetPos.x < squareRoot ? 1 : -1;
        dirNormalize.z *= collNetPos.z > -squareRoot && collNetPos.z < squareRoot ? 1 : -1;

        direction = Mathf.Atan2(dirNormalize.z, dirNormalize.x) * -Mathf.Rad2Deg + 90f;
        rb.rotation = Quaternion.Euler(0, direction, 0);

        BriefcaseLaunch();
    }

    public void BriefcaseLaunch()
    {
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }
}
