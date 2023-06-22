using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeChacracter : PlayerBase
{
    [SerializeField] float AttackRange;
    

    //Skill One
    [SerializeField] SpriteRenderer Sword;
    float TimeCount = 5f;
    private Coroutine Righteous;
    int Righteous_BonusDamage = 50;

    // Start is called before the first frame update
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
        if (context.performed && CanExecuteNormalAttack(AttackCooldown_Current))
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
                        Enemy.GetComponent<Enemy>().TakeDamage(AccountEntity.ID, Weapon_Entity.Damage + DamageBonus);
                    }
                }
            }
        }
    }

    public void Animation_SkillOne()
    {

        if (Righteous != null)
        {
            // If a color change coroutine is already running, stop it
            StopCoroutine(Righteous);
            SetUpRighteous(Color.black, 0f, -Righteous_BonusDamage);
            Righteous = null;
        }

        Righteous = StartCoroutine(RighteousSword());
    }

    public void Animation_SkillTwo()
    {
        GameObject skillTwo = playerPool.GetSkillTwoFromPool();

        if (skillTwo != null)
        {
            skillTwo.transform.position = AttackPoint.position;
            skillTwo.transform.rotation = AttackPoint.rotation;
            if (photonView.IsMine)
            {
                skillTwo.GetComponent<Melee_SkillTwo>().SetUp(AccountEntity.ID, SkillTwo_Entity.Damage + DamageBonus);
            }
            skillTwo.SetActive(true);
        }

    }

    public void Animation_SkillThree()
    {

        GameObject skillThree = playerPool.GetSkillThreeFromPool();
        FlipToMouse();
        if (skillThree != null)
        {
            skillThree.transform.position = targetPosition + new Vector3(0, 8, 0);
            if (photonView.IsMine)
            {
                skillThree.GetComponent<Melee_SkillThree>().SetUp(AccountEntity.ID, SkillThree_Entity.Damage + DamageBonus);
            }
            skillThree.GetComponent<Melee_SkillThree>().SetUpPoint(targetPosition, playerPool.GetSkillThreeExplosionFromPool());
            skillThree.SetActive(true);
        }

    }

    public IEnumerator RighteousSword()
    {
        SetUpRighteous(Color.yellow, 3f, Righteous_BonusDamage);

        yield return new WaitForSeconds(TimeCount);

        SetUpRighteous(Color.black, 0f, -Righteous_BonusDamage);

        Righteous = null;
    }

    public void SetUpRighteous(Color color, float Intensity, int Damage)
    {
        DamageBonus += Damage;

        Sword.material.SetColor("_GlowColor", color * Intensity);

        Debug.Log(DamageBonus);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
