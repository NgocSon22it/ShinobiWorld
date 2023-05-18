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
        WeaponEntity = Weapon_DAO.GetWeaponByID("Weapon_Sword");
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        SkillOne();
        SkillTwo();
        SkillThree();

    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && PV.IsMine)
        {
            CallSyncAnimation("Attack_Melee");
        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.started && SkillOneCooldown_Current <= 0f && PV.IsMine)
        {
            SkillOneCooldown_Current = SkillOneCooldown_Total;
            CallSyncAnimation("Skill1_Melee");
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.started && SkillTwoCooldown_Current <= 0f && PV.IsMine)
        {
            SkillTwoCooldown_Current = SkillTwoCooldown_Total;
            CallSyncAnimation("Skill2_Melee");

        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.started && SkillThreeCooldown_Current <= 0f && PV.IsMine)
        {
            SkillThreeCooldown_Current = SkillThreeCooldown_Total;
            CallSyncAnimation("Skill3_Melee");
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
        Collider2D[] HitEnemy = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, AttackableLayer);

        if (HitEnemy != null)
        {
            foreach (Collider2D Enemy in HitEnemy)
            {
                if (Enemy.gameObject.CompareTag("Enemy"))
                {
                    Enemy.GetComponent<Enemy>().TakeDamage(this, WeaponEntity.Damage);
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
            skillTwo.GetComponent<SwingSword>().SetUpSwingSword(this, WeaponEntity);
            skillTwo.GetComponent<SwingSword>().SetUpCenter(transform);
            skillTwo.SetActive(true);
        }
    }




    public IEnumerator RighteousSword()
    {
        int DamageBonus = 50;
        WeaponEntity.Damage += DamageBonus;
        yield return new WaitForSeconds(5f);
        WeaponEntity.Damage -= DamageBonus;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
