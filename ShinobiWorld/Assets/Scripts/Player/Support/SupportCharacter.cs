using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupportCharacter : PlayerBase
{
    [SerializeField] protected Vector2 DetectGroundVector;


    new void Start()
    {
        base.Start();
        WeaponName = "Weapon_Glove";
        AccountWeapon_Entity = AccountWeapon_DAO.GetAccountWeaponByID(AccountEntity.ID, WeaponName);
        SkillOne_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, "Skill_SupportOne");
        SkillTwo_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, "Skill_SupportTwo");
        SkillThree_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, "Skill_SupportThree");
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
        if (context.started && AccountWeapon_Entity != null && PV.IsMine)
        {
            CallSyncAnimation("Attack_Support");
        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (SkillOne_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillOneCooldown_Current, SkillOne_Entity.Chakra))
            {
                CallSyncAnimation("Skill1_Support");
            }
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (SkillTwo_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillTwoCooldown_Current, SkillTwo_Entity.Chakra))
            {
                CallSyncAnimation("Skill2_Support");
            }
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (SkillThree_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillThreeCooldown_Current, SkillThree_Entity.Chakra))
            {
                CallSyncAnimation("Skill3_Support");
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

    public void NormalAttackDamage()
    {
        RaycastHit2D[] HitEnemy = Physics2D.BoxCastAll(AttackPoint.position, DetectGroundVector, 0, -AttackPoint.up, 0, AttackableLayer);

        if (HitEnemy != null)
        {
            foreach (RaycastHit2D Enemy in HitEnemy)
            {
                if (Enemy.transform.CompareTag("Enemy"))
                {
                    Debug.Log(Enemy.transform.name);
                }
            }
        }
    }

    public void ExecuteSkillOne()
    {
        StartCoroutine(EnhanceDamage());
    }

    public void ExecuteSkillTwo()
    {
        StartCoroutine(EnhanceSpeedNHeal());

    }

    IEnumerator EnhanceSpeedNHeal()
    {
        int SpeedeBonus = 60;
        int HealAmount = 200;

        AccountEntity.Speed += SpeedeBonus;
        AccountEntity.CurrentHealth += HealAmount;

        yield return new WaitForSeconds(10f);


        AccountEntity.Speed -= SpeedeBonus;
    }

    IEnumerator EnhanceDamage()
    {
        int DamageBonus = 60;

        AccountEntity.Strength += DamageBonus;


        yield return new WaitForSeconds(10f);
        AccountEntity.Strength -= DamageBonus;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(AttackPoint.position - AttackPoint.up * 0, DetectGroundVector);
    }
}
