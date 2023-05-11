using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeCharacter : PlayerBase
{
    [SerializeField] GameObject NormalAttackPrefabs;

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
            animator.SetTrigger("Skill1_Range");
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.started && SkillTwoCooldown_Current <= 0f && PV.IsMine)
        {
            SkillTwoCooldown_Current = SkillTwoCooldown_Total;
            animator.SetTrigger("Skill2_Range");
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.started && SkillThreeCooldown_Current <= 0f && PV.IsMine)
        {
            SkillThreeCooldown_Current = SkillThreeCooldown_Total;
            animator.SetTrigger("Skill3_Range");
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
        PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered);

        Vector2 direction = (Vector2)Enemy.transform.position - (Vector2)AttackPoint.position;
        direction.Normalize();

        if (normalAttack != null)
        {
            normalAttack.transform.position = AttackPoint.position;
            normalAttack.transform.rotation = AttackPoint.rotation;
            normalAttack.SetActive(true);
            normalAttack.GetComponent<Rigidbody2D>().AddForce(direction * 500);
        }




    }

}
