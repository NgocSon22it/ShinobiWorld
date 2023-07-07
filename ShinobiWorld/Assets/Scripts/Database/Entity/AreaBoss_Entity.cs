using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[System.Serializable]
public class AreaBoss_Entity
{
    public string ID;
    public string AreaID;
    public string BossID;
    public bool isDead;
    public SqlDateTime TimeSpawn;
    public int CurrentHealth;
    public bool Delete;

    public AreaBoss_Entity()
    {
    }

}
