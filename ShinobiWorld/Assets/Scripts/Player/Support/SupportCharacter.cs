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
    [SerializeField] GameObject SteelFistEffect;
    //Skill Two
    float Blessing_Time = 7f;
    private Coroutine Blessing;
    int Blessing_SpeedBonus = 3;
    int Blessing_HealthBonus = 200;
    [SerializeField] GameObject BlessingEffect;

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

                if (Enemy.transform.CompareTag("Enemy") || Enemy.transform.CompareTag("Clone"))
                {
                    PlaySound_NormalAttack_Hit();
                    Enemy.transform.GetComponent<Enemy>().TakeDamage(photonView.ViewID, Weapon_Entity.Damage + DamageBonus);
                }
                if (Enemy.transform.gameObject.CompareTag("Player")
                    && Enemy.transform.gameObject.GetComponent<PhotonView>() != photonView
                    && Enemy.transform.gameObject.GetComponent<PlayerBase>().accountStatus == AccountStatus.PK
                    )
                {
                    PlaySound_NormalAttack_Hit();
                    Enemy.transform.GetComponent<PlayerBase>().TakeDamage(Weapon_Entity.Damage + DamageBonus);
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
            StopCoroutine(Blessing);
            SetUpBlessing(-Blessing_SpeedBonus, 0);
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
            skillThree.GetComponent<Support_SkillThree>().SetUp(SkillOne_Entity.Damage + DamageBonus);
            skillThree.GetComponent<Support_SkillThree>().SetUpDirection(SkillDirection);
            skillThree.SetActive(true);
            skillThree.GetComponent<Rigidbody2D>().velocity = (SkillDirection * 10);
        }

    }

    IEnumerator IE_Blessing()
    {
        BlessingEffect.SetActive(true);
        SetUpBlessing(Blessing_SpeedBonus, Blessing_HealthBonus);

        yield return new WaitForSeconds(Blessing_Time);

        BlessingEffect.SetActive(false);
        SetUpBlessing(-Blessing_SpeedBonus, 0);

        Blessing = null;
    }

    IEnumerator IE_SteelFist()
    {
        SteelFistEffect.SetActive(true);
        SetUpSteelFist(SteelFist_DamageBonus);

        yield return new WaitForSeconds(SteelFist_Time);

        SteelFistEffect.SetActive(false);
        SetUpSteelFist(-SteelFist_DamageBonus);

        SteelFist = null;

    }

    public void SetUpSteelFist(int Damage)
    {

        DamageBonus += Damage;

    }

    public void SetUpBlessing(int Speed, int Health)
    {
        SpeedBonus += Speed;
        HealAmountOfHealth(Health);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(AttackPoint.position - AttackPoint.up * 0, DetectGroundVector);
    }
}
