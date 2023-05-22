using Assets.Scripts.Database.Entity;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class RangeCharacter : PlayerBase
{
    [SerializeField] GameObject NormalAttackPrefabs;

    [SerializeField] float AttackRange;
    [SerializeField] float EndAngle = 25f;
    bool IsDetectEnemy;


    new void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            WeaponName = "Weapon_Dart";
            SkillOneName = "Skill_RangeOne";
            SkillTwoName = "Skill_RangeTwo";
            SkillThreeName = "Skill_RangeThree";
            AccountWeapon_Entity = AccountWeapon_DAO.GetAccountWeaponByID(AccountEntity.ID, WeaponName);
            LoadAccountSkill();

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
            CallSyncAnimation("Attack_Range");

        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (SkillOne_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillOneCooldown_Current, SkillOne_Entity.Chakra))
            {
                CallSyncAnimation("Skill1_Range");
            }
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (SkillTwo_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillTwoCooldown_Current, SkillTwo_Entity.Chakra))
            {
                CallSyncAnimation("Skill2_Range");
            }
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (SkillThree_Entity != null)
        {
            if (context.started && CanExecuteSkill(SkillThreeCooldown_Current, SkillThree_Entity.Chakra))
            {
                CallSyncAnimation("Skill3_Range");
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

    public void Animation_NormalAttack()
    {
        GameObject normalAttack = playerPool.GetNormalAttackFromPool();

        photonView.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

        if (Enemy != null)
        {
            FlipToEnemy();
            Vector2 direction = (Vector2)Enemy.transform.position - (Vector2)AttackPoint.position;
            direction.Normalize();

            if (normalAttack != null)
            {
                normalAttack.transform.position = AttackPoint.position;
                normalAttack.transform.rotation = AttackPoint.rotation;
                normalAttack.GetComponent<Dart>().SetUpDart(AccountEntity.ID, AccountWeapon_Entity);
                normalAttack.SetActive(true);
                normalAttack.GetComponent<Rigidbody2D>().velocity = direction * 10;
            }
        }
        else
        {
            if (normalAttack != null)
            {
                normalAttack.transform.position = AttackPoint.position;
                normalAttack.transform.rotation = AttackPoint.rotation;
                normalAttack.GetComponent<Dart>().SetUpDart(AccountEntity.ID, AccountWeapon_Entity);
                normalAttack.SetActive(true);
                normalAttack.GetComponent<Rigidbody2D>().velocity = 10 * new Vector2(transform.localScale.x, 0);
            }
        }
    }

    public void Animation_SkillOne()
    {
        GameObject skillOne = playerPool.GetSkillOneFromPool();

        photonView.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

        if (Enemy != null)
        {
            FlipToEnemy();
            Vector2 direction = (Vector2)Enemy.transform.position - (Vector2)AttackPoint.position;
            direction.Normalize();

            if (skillOne != null)
            {
                skillOne.transform.position = AttackPoint.position;
                skillOne.transform.rotation = AttackPoint.rotation;
                skillOne.SetActive(true);
                skillOne.GetComponent<Rigidbody2D>().velocity = (direction * 10);
            }
        }
        else
        {
            if (skillOne != null)
            {
                skillOne.transform.position = AttackPoint.position;
                skillOne.transform.rotation = AttackPoint.rotation;
                skillOne.SetActive(true);
                skillOne.GetComponent<Rigidbody2D>().velocity = (10 * new Vector2(transform.localScale.x, 0));
            }
        }
    }

    public void Animation_SkillTwo()
    {
        photonView.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

        if (Enemy != null)
        {
            FlipToEnemy();
            Vector2 direction = (Vector2)Enemy.transform.position - (Vector2)AttackPoint.position;
            direction.Normalize();

            ExecuteThreeDarts(10 * direction);

        }
        else
        {
            ExecuteThreeDarts(10 * new Vector2(transform.localScale.x, 0));
        }

    }

    public void ExecuteThreeDarts(Vector2 direction)
    {
        GameObject centerDarts = playerPool.GetSkillTwoFromPool();
        if (centerDarts != null)
        {
            centerDarts.transform.position = transform.position;
            centerDarts.transform.rotation = Quaternion.identity;
            centerDarts.SetActive(true);
            centerDarts.GetComponent<Rigidbody2D>().velocity = direction;
        }

        GameObject leftDarts = playerPool.GetSkillTwoFromPool();
        if (leftDarts != null)
        {
            leftDarts.transform.position = transform.position;
            leftDarts.SetActive(true);
            leftDarts.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(-EndAngle, Vector3.forward) * centerDarts.GetComponent<Rigidbody2D>().velocity;
        }

        GameObject rightDarts = playerPool.GetSkillTwoFromPool();
        if (rightDarts != null)
        {
            rightDarts.transform.position = transform.position;
            rightDarts.SetActive(true);
            rightDarts.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(EndAngle, Vector3.forward) * centerDarts.GetComponent<Rigidbody2D>().velocity;
        }
    }

    public void Animation_SkillThree()
    {
        StartCoroutine(EnhanceDamageNSpeed());
    }

    IEnumerator EnhanceDamageNSpeed()
    {
        int DamageBonus = 70;
        int SpeedBonus = 5;

        AccountEntity.Strength += DamageBonus;
        AccountEntity.Speed += SpeedBonus;


        yield return new WaitForSeconds(10f);
        AccountEntity.Strength -= DamageBonus;
        AccountEntity.Speed -= SpeedBonus;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
