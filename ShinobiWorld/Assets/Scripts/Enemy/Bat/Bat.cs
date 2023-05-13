using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        boss_Entity.ID = 3;
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
