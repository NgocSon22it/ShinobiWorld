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
    public string TrophiesID;
    public int Level;
    public int Health;
    public int CurrentHealth;
    public int Chakra;
    public int CurrentChakra;
    public int Exp;
    public int Speed;
    public int Coin;
    public int Power;
    public int Strength;
    public int CurrentStrength;
    public string EyeID;
    public string HairID;
    public string MouthID;
    public string SkinID;
    public bool IsDead;
    public bool IsOnline;
    public bool IsTicket;
    public bool IsFirst;
    public bool IsUpgradeTrophy;
    public DateTime DateReset;

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
