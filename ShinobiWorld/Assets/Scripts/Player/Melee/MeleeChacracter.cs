using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeChacracter : PlayerBase
{
    [SerializeField] float AttackRange;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            WeaponName = "Weapon_Sword";
            AccountWeapon_Entity = AccountWeapon_DAO.GetAccountWeaponByID(AccountEntity.ID, WeaponName);
            SkillOne_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, "Skill_MeleeOne");
            SkillTwo_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, "Skill_MeleeTwo");
            SkillThree_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, "Skill_MeleeThree");
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (photonView.IsMine)
        {
            SkillOne();
            SkillTwo();
            SkillThree();
        }

    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && AccountWeapon_Entity != null && photonView.IsMine)
        {
            CallSyncAnimation("Attack_Melee");
        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (SkillOne_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillOneCooldown_Current, SkillOne_Entity.Chakra))
            {
                CallSyncAnimation("Skill1_Melee");
            }
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (SkillTwo_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillTwoCooldown_Current, SkillTwo_Entity.Chakra))
            {
                CallSyncAnimation("Skill2_Melee");
            }
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (SkillThree_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillThreeCooldown_Current, SkillThree_Entity.Chakra))
            {
                CallSyncAnimation("Skill3_Melee");
            }
        }
    }

    public void SkillOne()
    {
        if (SkillOneCooldown_Current > 0)
        {
            SkillOneCooldown_Current -= Time.deltaTime;
        }
    }

    public void SkillTwo()
    {
        if (SkillTwoCooldown_Current > 0)
        {
            SkillTwoCooldown_Current -= Time.deltaTime;
        }
    }

    public void SkillThree()
    {
        if (SkillThreeCooldown_Current > 0)
        {
            SkillThreeCooldown_Current -= Time.deltaTime;
        }
    }
    public void DamageNormalAttack()
    {
        if (photonView.IsMine)
        {
            Collider2D[] HitEnemy = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, AttackableLayer);

            if (HitEnemy != null)
            {
                foreach (Collider2D Enemy in HitEnemy)
                {
                    if (Enemy.gameObject.CompareTag("Enemy"))
                    {
                        Enemy.GetComponent<Enemy>().TakeDamage(AccountEntity.ID, AccountWeapon_Entity.Damage);
                    }
                }
            }
        }
    }

    public void Animation_SkillOne()
    {
        StartCoroutine(RighteousSword());
    }

    public void Animation_SkillTwo()
    {
        GameObject skillTwo = playerPool.GetSkillTwoFromPool();

        if (skillTwo != null)
        {
            skillTwo.transform.position = AttackPoint.position;
            skillTwo.transform.rotation = AttackPoint.rotation;
            skillTwo.GetComponent<SwingSword>().SetUpSwingSword(AccountEntity.ID, AccountWeapon_Entity);
            skillTwo.GetComponent<SwingSword>().SetUpCenter(transform);
            skillTwo.SetActive(true);
        }
    }

    public IEnumerator RighteousSword()
    {
        int DamageBonus = 50;
        AccountWeapon_Entity.Damage += DamageBonus;
        yield return new WaitForSeconds(5f);
        AccountWeapon_Entity.Damage -= DamageBonus;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
