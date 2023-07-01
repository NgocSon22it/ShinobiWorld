using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi_SkillOneExplosion : Boss_Skill
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
