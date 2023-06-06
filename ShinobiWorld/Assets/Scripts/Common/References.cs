using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References
{
    public static Account_Entity accountRefer = new Account_Entity();
    public static List<AccountItem_Entity> listAccountItem = new List<AccountItem_Entity>();
    public static List<AccountEquipment_Entity> listAccountEquipment = new List<AccountEquipment_Entity>();
    public static List<AccountSkill_Entity> listAccountSkill = new List<AccountSkill_Entity>();
    public static AccountWeapon_Entity accountWeapon = new AccountWeapon_Entity();
    public static Weapon_Entity weapon = new Weapon_Entity();

    public static int Maxserver = 20;

    public static List<Skin_Entity> listSkin = Skin_DAO.GetAll();
    public static List<Eye_Enity> listEye = Eye_DAO.GetAll();
    public static List<Hair_Entity> listHair = Hair_DAO.GetAll();
    public static List<Mouth_Entity> listMouth = Mouth_DAO.GetAll();

    public static List<RoleInGame_Entity> listRole = RoleInGame_DAO.GetAll();
    public static List<Item_Entity> listItem = Item_DAO.GetAll();
    public static List<Equipment_Entity> listEquipment = Equipment_DAO.GetAll();
    public static List<TypeEquipment_Entity> listTypeEquipment = TypeEquipment_DAO.GetAll();
    public static List<Trophy_Entity> listTrophy = Trophy_DAO.GetAll();

    public static List<Skill_Entity> ListSkill = Skill_DAO.GetAllSkill();

    public static float Uppercent_Skill_Damage = 3f, Uppercent_Skill_Chakra = 1f, Uppercent_Skill_CoolDown = 1f;
    public static float Uppercent_Account = 5f;
    public static float Uppercent_Equipment = 5f;
    public static int MaxUpgradeLevel = 30;

    public static Vector3 Hokake = new(0, 0, 0);
    public static Vector3 Hospital = new(17, -26, 0);

    public static int RespawnTime = 20;
    public static int RespawnCost = 1000;

    public static void LoadAccount()
    {
        if (accountRefer != null)
        {
            Account_DAO.LoadAccount(accountRefer);
        }
    }

}


public enum TypeSell
{
    Item, Equipment
}

public enum Intention
{
    Sell, Bag
}