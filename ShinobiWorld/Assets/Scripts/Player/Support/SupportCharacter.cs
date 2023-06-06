using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupportCharacter : PlayerBase
{
    [SerializeField] protected Vector2 DetectGroundVector;

    //Skill One
    float SteelFist_Time = 5f;
    private Coroutine SteelFist;
    int SteelFist_DamageBonus = 60;

    //Skill Two
    float Blessing_Time = 7f;
    private Coroutine Blessing;
    int Blessing_SpeedBonus = 5;
    int Blessing_HealthBonus = 200;

    new void Start()
    {
        base.Start();

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

    public void NormalAttackDamage()
    {
        RaycastHit2D[] HitEnemy = Physics2D.BoxCastAll(AttackPoint.position, DetectGroundVector, 0, -AttackPoint.up, 0, AttackableLayer);

        if (HitEnemy != null)
        {
            foreach (RaycastHit2D Enemy in HitEnemy)
            {
                if (Enemy.transform.CompareTag("Enemy"))
                {
                    //Enemy.transform.GetComponent<Enemy>().take
                }
            }
        }
    }

    public void Animation_SkillOne()
    {
        if (SteelFist != null)
        {
            StopCoroutine(SteelFist);
            SetUpSteelFist(-SteelFist_DamageBonus);
            SteelFist = null;
        }

        SteelFist = StartCoroutine(IE_SteelFist());
    }

    public void Animation_SkillTwo()
    {
        if (Blessing != null)
        {
            // If a color change coroutine is already running, stop it
            StopCoroutine(Blessing);
            SetUpBlessing(Blessing_SpeedBonus, 0);
            Blessing = null;
        }

        Blessing = StartCoroutine(IE_Blessing());
    }

    public void Animation_SkillThree()
    {
        GameObject skillThree = playerPool.GetSkillThreeFromPool();

        FlipToMouse();

        SkillDirection = (Vector2)targetPosition - (Vector2)AttackPoint.position;
        SkillDirection.Normalize();

        if (skillThree != null)
        {
            skillThree.transform.position = AttackPoint.position;
            skillThree.transform.rotation = AttackPoint.rotation;
            skillThree.GetComponent<FierceFist>().SetUp(References.accountRefer.ID, SkillOne_Entity.Damage + DamageBonus);
            skillThree.SetActive(true);
            skillThree.GetComponent<Rigidbody2D>().velocity = (SkillDirection * 10);
        }
    }

    IEnumerator IE_Blessing()
    {
        SetUpBlessing(Blessing_SpeedBonus, Blessing_HealthBonus);

        yield return new WaitForSeconds(Blessing_Time);

        SetUpBlessing(-Blessing_SpeedBonus, 0);

        Blessing = null;
    }

    IEnumerator IE_SteelFist()
    {
        SetUpSteelFist(SteelFist_DamageBonus);

        yield return new WaitForSeconds(SteelFist_Time);

        SetUpSteelFist(-SteelFist_DamageBonus);

        SteelFist = null;

    }

    public void SetUpSteelFist(int Damage)
    {
        DamageBonus += Damage;
        Debug.Log(DamageBonus);
    }

    public void SetUpBlessing(int Speed, int Health)
    {
        SpeedBonus += Speed;
        HealAmountOfHealth(Health);
        Debug.Log(DamageBonus);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(AttackPoint.position - AttackPoint.up * 0, DetectGroundVector);
    }
}
