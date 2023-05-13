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
    bool IsDetectEnemy;
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
                normalAttack.SetActive(true);
                normalAttack.GetComponent<Rigidbody2D>().AddForce(direction * 500);
            }
        }
        else
        {
            if (normalAttack != null)
            {
                normalAttack.transform.position = AttackPoint.position;
                normalAttack.transform.rotation = AttackPoint.rotation;
                normalAttack.SetActive(true);
                normalAttack.GetComponent<Rigidbody2D>().AddForce(500 * new Vector2(transform.localScale.x,0));
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
                skillOne.GetComponent<Rigidbody2D>().AddForce(direction * 500);
            }
        }
        else
        {
            if (skillOne != null)
            {
                skillOne.transform.position = AttackPoint.position;
                skillOne.transform.rotation = AttackPoint.rotation;
                skillOne.SetActive(true);
                skillOne.GetComponent<Rigidbody2D>().AddForce(500 * new Vector2(transform.localScale.x, 0));
            }
        }
    }

    public void SpawnThreeDarts()
    {

        List<GameObject> list = new List<GameObject>();

        for(int i = 0; i < 3; i++)
        {
            list[i] = playerPool.GetSkillTwoFromPool();
        }

        PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered, (int)AttackRange);

        if (Enemy != null)
        {
            FlipToEnemy();
            Vector2 direction = (Vector2)Enemy.transform.position - (Vector2)AttackPoint.position;
            direction.Normalize();

            if (list.Count == 3)
            {
                foreach(GameObject obj in list)
                {
                    obj.transform.position = AttackPoint.position;
                    obj.transform.rotation = AttackPoint.rotation;
                    obj.SetActive(true);
                }

                list[0].GetComponent<Rigidbody2D>().AddForce(direction * 500);

            }
        }
        else
        {

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
