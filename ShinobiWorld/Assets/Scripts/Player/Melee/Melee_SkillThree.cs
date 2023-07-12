using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Melee_SkillThree : PlayerSkill
{
    public Vector3 EndPoint;

    bool Reach;
    public void SetUpPoint(Vector3 EndPoint)
    {
        this.EndPoint = EndPoint;
    }

    private void Update()
    {
        if (EndPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPoint, 8 * Time.deltaTime);
        }

        if (transform.position == EndPoint && !Reach)
        {
            Reach = true;
            TurnOff();
            HitEffect = player_Pool.GetSkillThree_Hit_FromPool();
            if (HitEffect != null)
            {
                HitEffect.transform.position = EndPoint;
                HitEffect.SetActive(true);
            }
        }
    }

    new void OnEnable()
    {
        LifeTime = 5f;
        Reach = false;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
