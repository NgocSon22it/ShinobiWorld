using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Account_Entity
{
    public string ID;
    public string Name;
    public string RoleInGameID;
    public string TrophyID;
    public bool IsHokage;
    public int Coin;
    public int Exp;
    public int Level;
    public int Health;
    public int CurrentHealth;
    public int Chakra;
    public int CurrentChakra;
    public int Strength;
    public int CurrentStrength;
    public int Speed;
    public int Power;
    public bool IsFirst;
    public bool IsOnline;
    public DateTime ResetLimitDate;
    public bool HasTicket;
    public bool IsUpgradeTrophy;
    public bool IsDead;
    public int WinTimes;
    public string EyeID;
    public string HairID;
    public string MouthID;
    public string SkinID;
    public int TimeRespawn;

    public List<CustomSetting_Entity> CustomSettings;

    public Account_Entity() { }
    public static object Deserialize(byte[] data)
    {
        var result = new Account_Entity();
        result.ID = data[0].ToString();


        return result;
    }
    public static byte[] Serialize(object customType)
    {
        var c = (Account_Entity)customType;
        return new byte[] { Convert.ToByte(c.ID)};
    }
}
