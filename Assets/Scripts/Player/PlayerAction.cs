using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Profile Controller")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerProfile profile;
    [Header("Action")]
    [SerializeField] private Transform firePoint;
    private float actionTimer = 0;
    private float actionCooldown;
    private float actionDamage;
    private enum AttackType
    {
        paper,
        briefcase,
        gas,
        scythe,
        brick
    }
    private List<AttackType> attackType = new List<AttackType>();
    [SerializeField] private Animator animator;
    [Header("Paper")]
    [SerializeField] private GameObject paperPrefab;
    private int paperCount;
    private bool paperActive;
    private int paperLeft;
    private float paperTimer = 0;
    private float paperCooldown;
    private int paperDamage;
    [Header("Briefcase")]
    [SerializeField] private GameObject briefcasePrefab;
    private int wallBounce;
    private int briefcaseDamage;
    [Header("Gas")]
    [SerializeField] private GameObject gasPrefab;
    private float gasRadius;
    private int gasDamage;
    [Header("Scythe")]
    [SerializeField] private GameObject scythePrefab;
    [SerializeField] private GameObject scytheAnchorPrefab;
    private Transform scytheAnchor;
    private float scytheSpeed;
    private int scytheDamage;
    [Header("Brick")]
    [SerializeField] private GameObject brickPrefab;
    private float brickKnockback;
    private int brickDamage;

    public void SetPlayerAction(UpgradeStats upgrade)
    {
        actionCooldown = 1 / (profile.baseActionCooldown + (upgrade.cooldownTier * profile.levelActionCooldown));
        actionDamage = profile.baseActionDamage + (upgrade.damageTier * profile.levelActionDamage);
        paperCount = profile.basePaperCount + (upgrade.paperTier * profile.levelPaperCount);
        paperCooldown = actionCooldown / paperCount;
        wallBounce = profile.baseWallBounce + (upgrade.briefcaseTier * profile.levelWallBounce);
        gasRadius = profile.baseGasRange + (upgrade.gasTier * profile.levelGasRange);
        scytheSpeed = profile.baseScytheSpeed + (upgrade.scytheTier * profile.levelScytheSpeed);
        brickKnockback = profile.baseBrickKnockback + (upgrade.brickTier * profile.levelBrickKnockback);

        paperDamage = (int)(profile.paperDamage * actionDamage);
        briefcaseDamage = (int)(profile.briefDamage * actionDamage);
        gasDamage = (int)(profile.gasDamage * actionDamage);
        scytheDamage = (int)(profile.scytheDamage * actionDamage);
        brickDamage = (int)(profile.brickDamage * actionDamage);

        GameObject anchor = Instantiate(scytheAnchorPrefab, transform.position, Quaternion.identity);
        scytheAnchor = anchor.transform;

        SetAttackType(upgrade.paperTier, upgrade.briefcaseTier, upgrade.gasTier, upgrade.scytheTier, upgrade.brickTier);
    }

    private void SetAttackType(int paper, int briefcase, int gas, int scythe, int brick)
    {
        attackType.Clear();
        for (int i = 0; i < paper; i++)
        {
            attackType.Add(AttackType.paper);
        }
        for (int i = 0; i < briefcase; i++)
        {
            attackType.Add(AttackType.briefcase);
        }
        for (int i = 0; i < gas; i++)
        {
            attackType.Add(AttackType.gas);
        }
        for (int i = 0; i < scythe; i++)
        {
            attackType.Add(AttackType.scythe);
        }
        for (int i = 0; i < brick; i++)
        {
            attackType.Add(AttackType.brick);
        }
    }

    private void FixedUpdate()
    {
        if(controller.attack)
        {
            if (paperActive)
            {
                paperTimer += Time.deltaTime;
                if (paperTimer >= paperCooldown)
                {
                    ShootPaper();
                }
            }
            else
            {
                actionTimer += Time.deltaTime;
                if (actionTimer >= actionCooldown)
                {
                    Attack();
                }
            }
        }
        scytheAnchor.position = gameObject.transform.position;
    }

    private void Attack()
    {
        int attackRandom = Random.Range(0, attackType.Count);
        switch(attackType[attackRandom])
        {
            case AttackType.paper:
                SetPaper();
                break;
            case AttackType.briefcase:
                ShootBriefcase();
                break;
            case AttackType.gas:
                ShootGas();
                break;
            case AttackType.scythe:
                ShootScythe();
                break;
            case AttackType.brick:
                ShootBrick();
                break;
            default:
                break;
        }
        actionTimer = 0;

    }

    private void SetPaper()
    {
        paperLeft = paperCount;
        paperActive = true;
        paperTimer = paperCooldown;
    }

    private void ShootPaper()
    {
        paperTimer = 0;
        paperLeft--;
        GameObject paper = Instantiate(paperPrefab, firePoint.position, controller.rotation);
        paper.GetComponent<PlayerPaper>().SetStats(paperDamage);
        paper.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * profile.paperBulletSpeed, ForceMode.Impulse);
        Destroy(paper, 3f);
        if (paperLeft <= 0)
        {
            paperActive = false;
        }
        animator.SetTrigger("Attack");
    }

    private void ShootBriefcase()
    {
        GameObject briefcase = Instantiate(briefcasePrefab, firePoint.position, controller.rotation);
        briefcase.GetComponent<PlayerBriefcase>().SetStats(briefcaseDamage, wallBounce, profile.paperBulletSpeed);
        briefcase.GetComponent<PlayerBriefcase>().BriefcaseLaunch();
        Destroy(briefcase, 3f);
        animator.SetTrigger("Attack");
    }

    private void ShootGas()
    {
        GameObject gas = Instantiate(gasPrefab, firePoint.position, controller.rotation);
        gas.GetComponent<PlayerGas>().SetStats(gasDamage, gasRadius, profile.gasTime);
        gas.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * profile.gasSpeed);
        animator.SetTrigger("Attack");
    }

    private void ShootScythe()
    {
        GameObject scythe = Instantiate(scythePrefab, scytheAnchor);
        scythe.GetComponent<PlayerScythe>().Setstats(scytheDamage, profile.scytheRange, scytheAnchor);
        scythe.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * scytheSpeed, ForceMode.Impulse);
        Destroy(scythe, profile.scytheDuration);
        animator.SetTrigger("Attack");
    }

    private void ShootBrick()
    {
        GameObject brick = Instantiate(brickPrefab, firePoint.position, controller.rotation);
        brick.GetComponent<PlayerBrick>().SetStats(brickDamage, brickKnockback);
        brick.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * profile.brickForce);
        Destroy(brick, 3f);
        animator.SetTrigger("Attack");
    }
}
