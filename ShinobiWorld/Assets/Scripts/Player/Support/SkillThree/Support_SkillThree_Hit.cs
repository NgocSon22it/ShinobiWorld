using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support_SkillThree_Hit : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 1f;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
