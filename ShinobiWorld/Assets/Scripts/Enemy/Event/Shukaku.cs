using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shukaku : Enemy
{
    new void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            boss_Entity.ID = "Boss_Bat";
            //boss_Pool.InitializeProjectilePool("Boss/Event/Shukaku/");
            boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
            CurrentHealth = boss_Entity.Health;
        }
    }

    }
