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
        if (context.started)
        {
            animator.SetTrigger("Attack_Support");
        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.started && SkillOneCooldown_Current <= 0f)
        {
            SkillOneCooldown_Current = SkillOneCooldown_Total;
            animator.SetTrigger("Skill1_Support");
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.started && SkillTwoCooldown_Current <= 0f)
        {
            SkillTwoCooldown_Current = SkillTwoCooldown_Total;
            animator.SetTrigger("Skill2_Support");
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.started && SkillThreeCooldown_Current <= 0f)
        {
            SkillThreeCooldown_Current = SkillThreeCooldown_Total;
            animator.SetTrigger("Skill3_Support");
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(AttackPoint.position - AttackPoint.up * 0, DetectGroundVector);
    }
}
