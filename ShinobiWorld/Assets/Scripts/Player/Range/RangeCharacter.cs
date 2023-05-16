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
        WeaponName = "Weapon_Dart";
        Weapon_Entity = Weapon_DAO.GetWeaponByID(WeaponName);
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
            CallSyncAnimation("Attack_Range");
        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.started && SkillOneCooldown_Current <= 0f && PV.IsMine)
        {
            SkillOneCooldown_Current = SkillOneCooldown_Total;
            CallSyncAnimation("Skill1_Range");
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.started && SkillTwoCooldown_Current <= 0f && PV.IsMine)
        {
            SkillTwoCooldown_Current = SkillTwoCooldown_Total;
            CallSyncAnimation("Skill2_Range");
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.started && SkillThreeCooldown_Current <= 0f && PV.IsMine)
        {
            SkillThreeCooldown_Current = SkillThreeCooldown_Total;
            CallSyncAnimation("Skill3_Range");
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

    public void Spawn_Darts()
    {
        GameObject normalAttack = playerPool.GetNormalAttackFromPool();

        PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

        if (Enemy != null)
        {
            FlipToEnemy();
            Vector2 direction = (Vector2)Enemy.transform.position - (Vector2)AttackPoint.position;
            direction.Normalize();

            if (normalAttack != null)
            {
                normalAttack.transform.position = AttackPoint.position;
                normalAttack.transform.rotation = AttackPoint.rotation;
                normalAttack.GetComponent<Dart>().SetUpDart(this, Weapon_Entity);
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
                normalAttack.GetComponent<Dart>().SetUpDart(this, Weapon_Entity);
                normalAttack.SetActive(true);
                normalAttack.GetComponent<Rigidbody2D>().velocity = 10 * new Vector2(transform.localScale.x, 0);
            }
        }
    }

    public void SpawnBigDarts()
    {
        GameObject skillOne = playerPool.GetSkillOneFromPool();

        PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

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

    public void SpawnThreeDarts()
    {
        PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

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

    public void Ultimate()
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
