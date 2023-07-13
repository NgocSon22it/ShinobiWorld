using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : Boss_Skill
{
    new void OnEnable()
    {
        LifeTime = 1.5f;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
