using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support_SkillThree_Hit : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 1f;
        player_Pool.gameObject.GetComponent<PlayerBase>().PlaySound_SkillThree_Hit();
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
