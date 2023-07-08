using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iruka_SkillOne_Explosion : Boss_Skill
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
