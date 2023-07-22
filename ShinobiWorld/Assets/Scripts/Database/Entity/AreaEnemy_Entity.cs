using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[System.Serializable]
public class AreaEnemy_Entity
{
    public string ID;
    public string AreaID;
    public string EnemyID;
    public bool IsDead;
    public SqlDateTime TimeSpawn;
    public bool Delete;

    public AreaEnemy_Entity()
    {
    }

}
