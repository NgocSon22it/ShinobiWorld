using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeCharacter : PlayerBase
{
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
            CallSyncAnimation("Attack_Range");
        }
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.started && SkillOneCooldown_Current <= 0f)
        {
            PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered);
            SkillOneCooldown_Current = SkillOneCooldown_Total;
            Debug.Log(Enemy.name);
        }
    }

    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.started && SkillTwoCooldown_Current <= 0f)
        {
            PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered);
            SkillTwoCooldown_Current = SkillTwoCooldown_Total;
            Debug.Log(Enemy.name);
        }
    }

    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.started && SkillThreeCooldown_Current <= 0f)
        {
            PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered);
            SkillThreeCooldown_Current = SkillThreeCooldown_Total;
            Debug.Log(Enemy.name);
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

}
