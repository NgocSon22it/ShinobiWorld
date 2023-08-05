using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support_BuffEffect : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 1.2f;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
