using Assets.Scripts.Database.Entity;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class RangeCharacter : PlayerBase
{
    [SerializeField] GameObject NormalAttackPrefabs;

    [SerializeField] float AttackRange;
    [SerializeField] float EndAngle = 25f;

    //Skill One
    float Hunter_Time = 10f;
    private Coroutine Hunter;
    int Hunter_DamageBonus = 70;
    int Hunter_SpeedBonus = 5;


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
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && CanExecuteNormalAttack(AttackCooldown_Current))
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

    public void Animation_NormalAttack()
    {
        GameObject normalAttack = playerPool.GetNormalAttackFromPool();
        FlipToMouse();

        SkillDirection = (Vector2)targetPosition - (Vector2)AttackPoint.position;
        SkillDirection.Normalize();

        if (normalAttack != null)
        {
            normalAttack.transform.position = AttackPoint.position;
            normalAttack.transform.rotation = AttackPoint.rotation;
            normalAttack.GetComponent<Dart>().SetUp(AccountEntity.ID, AccountWeapon_Entity.Damage + DamageBonus);
            normalAttack.SetActive(true);
            normalAttack.GetComponent<Rigidbody2D>().velocity = SkillDirection * 10;
        }

    }

    public void Animation_SkillOne()
    {
        GameObject skillOne = playerPool.GetSkillOneFromPool();

        FlipToMouse();

        SkillDirection = (Vector2)targetPosition - (Vector2)AttackPoint.position;
        SkillDirection.Normalize();

        if (skillOne != null)
        {
            skillOne.transform.position = AttackPoint.position;
            skillOne.transform.rotation = AttackPoint.rotation;
            skillOne.GetComponent<SuperDart>().SetUp(AccountEntity.ID, SkillOne_Entity.Damage + DamageBonus);
            skillOne.SetActive(true);
            skillOne.GetComponent<Rigidbody2D>().velocity = (SkillDirection * 10);
        }
    }

    public void Animation_SkillTwo()
    {
        GameObject centerDarts = playerPool.GetSkillTwoFromPool();

        FlipToMouse();

        SkillDirection = (Vector2)targetPosition - (Vector2)AttackPoint.position;
        SkillDirection.Normalize();


        if (centerDarts != null)
        {
            centerDarts.transform.position = AttackPoint.position;
            centerDarts.GetComponent<RedDart>().SetUp(AccountEntity.ID, SkillTwo_Entity.Damage + DamageBonus);
            centerDarts.SetActive(true);
            centerDarts.GetComponent<Rigidbody2D>().velocity = SkillDirection * 10;
        }

        GameObject leftDarts = playerPool.GetSkillTwoFromPool();
        if (leftDarts != null)
        {
            leftDarts.transform.position = AttackPoint.position;
            leftDarts.GetComponent<RedDart>().SetUp(AccountEntity.ID, SkillTwo_Entity.Damage + DamageBonus);
            leftDarts.SetActive(true);
            leftDarts.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(-EndAngle, Vector3.forward) * centerDarts.GetComponent<Rigidbody2D>().velocity;
        }

        GameObject rightDarts = playerPool.GetSkillTwoFromPool();
        if (rightDarts != null)
        {
            rightDarts.transform.position = AttackPoint.position;
            rightDarts.GetComponent<RedDart>().SetUp(AccountEntity.ID, SkillTwo_Entity.Damage + DamageBonus);
            rightDarts.SetActive(true);
            rightDarts.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(EndAngle, Vector3.forward) * centerDarts.GetComponent<Rigidbody2D>().velocity;
        }

    }

    public void Animation_SkillThree()
    {

        if (Hunter != null)
        {
            StopCoroutine(Hunter);
            SetUpHunter(-Hunter_DamageBonus, -Hunter_SpeedBonus);
            Hunter = null;
        }

        Hunter = StartCoroutine(IE_Hunter());
    }


    IEnumerator IE_Hunter()
    {
        SetUpHunter(Hunter_DamageBonus, Hunter_SpeedBonus);

        yield return new WaitForSeconds(Hunter_Time);

        SetUpHunter(-Hunter_DamageBonus, -Hunter_SpeedBonus);

        Hunter = null;
    }

    public void SetUpHunter(int Damage, int Speed)
    {
        DamageBonus += Damage;
        SpeedBonus += Speed;
        Debug.Log(DamageBonus);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
