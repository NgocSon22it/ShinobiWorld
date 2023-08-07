using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range_SkillOne_Hit : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 1f;
        player_Pool.gameObject.GetComponent<PlayerBase>().PlaySound_SkillOne_Hit();
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
