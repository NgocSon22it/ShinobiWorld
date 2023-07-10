﻿using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Friend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class References
{
    public static Account_Entity accountRefer = new Account_Entity();

    public static List<HasItem_Entity> listHasItem = new List<HasItem_Entity>();
    public static List<BagEquipment_Entity> listBagEquipment = new List<BagEquipment_Entity>();

    public static List<HasSkill_Entity> listHasSkill = new List<HasSkill_Entity>();

    public static HasWeapon_Entity hasWeapon = new HasWeapon_Entity();

    public static Weapon_Entity weapon = new Weapon_Entity();
    public static List<HasMission_Entity> listHasMission = new List<HasMission_Entity>();
    public static List<MailBox_Entity> listMailBox = new List<MailBox_Entity>();
    public static List<Friend_Entity> listAllFriend = new List<Friend_Entity>();
    public static List<FriendInfo> listFriendInfo = new List<FriendInfo>();
    public static List<FriendInfo> listRequestInfo = new List<FriendInfo>();

    public static int Maxserver = 20;

    public static List<Skin_Entity> listSkin = Skin_DAO.GetAll();
    public static List<Eye_Enity> listEye = Eye_DAO.GetAll();
    public static List<Hair_Entity> listHair = Hair_DAO.GetAll();
    public static List<Mouth_Entity> listMouth = Mouth_DAO.GetAll();

    public static List<RoleInGame_Entity> listRole = RoleInGame_DAO.GetAll();
    public static List<Item_Entity> listItem = Item_DAO.GetAll();
    public static List<Equipment_Entity> listEquipment = Equipment_DAO.GetAll();
    public static List<TypeEquipment_Entity> listTypeEquipment = TypeEquipment_DAO.GetAll();
    public static List<Mail_Entity> listMail = Mail_DAO.GetAll();

    public static IDictionary<string, string> BtnTrophy = new Dictionary<string, string>();
    public static List<Trophy_Entity> listTrophy = Trophy_DAO.GetAll();

    public static List<Mission_Entity> listMission = Mission_DAO.GetAll();

    public static List<Skill_Entity> listSkill = Skill_DAO.GetAllSkill();

    public static HasSkill_Entity hasSkillOne = new HasSkill_Entity();
    public static HasSkill_Entity hasSkillTwo = new HasSkill_Entity();
    public static HasSkill_Entity hasSkillThree = new HasSkill_Entity();

    public static float Uppercent_Skill_Damage = 3f, Uppercent_Skill_Chakra = 1f, Uppercent_Skill_CoolDown = 1f;
    public static float Uppercent_Account = 5f;
    public static float Uppercent_Equipment = 5f;
    public static int MaxUpgradeLevel = 30; 

    public static int RespawnTime = 20;
    public static int RespawnCost = 1000;

    public static int HealthBonus, ChakraBonus, StrengthBonus;

    public static int ExpercienceToNextLevel;

    public static bool IsDisconnect = false;

    public static string TrophyID_RemakeMission = TrophyID.Trophy_Jonin.ToString();

    public static string UISkillDefault = "Background/UI_OrangeFill";
    public static string UIEquipmentDefault = "Background/UI_GreenFill";
    public static string UIEquipmentShow = "Background/UI_Green";
    public static string UIInfoSelected = "Background/UI_Blue";
    
    public static Color32 ColorSelected = new Color32(190, 140, 10, 255);


    public static string MailSystem = "System";

    public static Vector3 PlayerSpawnPosition = Vector3.zero;

    public static IDictionary<string, Vector3> HouseAddress = new Dictionary<string, Vector3>()
                                                        {
                                                            {"Hokage", new(0, 0, 0)},
                                                            {"Hospital", new(17, -26, 0)},
                                                            {"Shop", new(-17, -47, 0)},
                                                            {"Arena", new(-44, -24, 0)},
                                                            {"Ramen", new(-15, -26, 0)},
                                                            {"School", new(-28, 8, 0)},
                                                            {"Casino", new(19, -47, 0)},
                                                            {"Uchiha", new(43, 0, 0)},
                                                         };

    public static IDictionary<string, string> BtnMission = new Dictionary<string, string>()
                                                        {
                                                            {StatusMission.None.ToString(), "Nhận" },
                                                            {StatusMission.Doing.ToString(), "Hủy bỏ"},
                                                            {StatusMission.Claim.ToString(), "Nhận thưởng"},
                                                            {StatusMission.Done.ToString(), "Hoàn thành"},
                                                        };

    public static IDictionary<string, Color32> Rank = new Dictionary<string, Color32>()
                                                        {
                                                            {"Background/Top1", new Color32(200, 145, 20, 255)},
                                                            {"Background/Top2", new Color32(146, 146, 174, 255)},
                                                            {"Background/Top3", new Color32(205, 110, 50, 255)},
                                                            {"Background/TopNone", new Color32(190, 160, 120, 255)},
                                                         };

    public static void UpdateAccountToDB()
    {
        if (accountRefer != null)
        {
            Account_DAO.UpdateAccountToDB(accountRefer);
        }
    }



    public static void BonusLevelUp()
    {
        if (accountRefer != null)
        {

            HealthBonus = Convert.ToInt32(accountRefer.Health * (Uppercent_Account / 100f));
            ChakraBonus = Convert.ToInt32(accountRefer.Chakra * (Uppercent_Account / 100f));
            StrengthBonus = 1;

            Debug.Log(HealthBonus + " " + ChakraBonus + " " + StrengthBonus);

            accountRefer.Health += HealthBonus;
            accountRefer.Chakra += ChakraBonus;

            accountRefer.CurrentHealth += HealthBonus;
            accountRefer.CurrentChakra += ChakraBonus;

            accountRefer.Strength += StrengthBonus;
            accountRefer.CurrentStrength += StrengthBonus;

            accountRefer.Level += 1;
        }
    }



    public static void LoadAccount()
    {
        accountRefer = Account_DAO.GetAccountByID(accountRefer.ID);
        ExpercienceToNextLevel = accountRefer.Level * 100;
    }

    public static void LoadHasWeaponNSkill(string Role)
    {
        if (accountRefer != null)
        {
            hasWeapon = HasWeapon_DAO.GetHasWeaponByID(accountRefer.ID);
            hasSkillOne = HasSkill_DAO.GetHasSkillByID(accountRefer.ID, "Skill_" + Role + "One");
            hasSkillTwo = HasSkill_DAO.GetHasSkillByID(accountRefer.ID, "Skill_" + Role + "Two");
            hasSkillThree = HasSkill_DAO.GetHasSkillByID(accountRefer.ID, "Skill_" + Role + "Three");
        }

    }

    public static void AddExperience(int Amount)
    {
        if (accountRefer != null && accountRefer.Level < 30)
        {
            accountRefer.Exp += Amount;
            while (accountRefer.Exp >= ExpercienceToNextLevel)
            {
                accountRefer.Level++;
                accountRefer.Exp -= ExpercienceToNextLevel;
                ExpercienceToNextLevel = accountRefer.Level * 100;
                LevelUpReward();
            }
        }

    }

    public static void AddCoin(int Amount)
    {
        accountRefer.Coin += Amount;
        //UpdateAccountToDB();
        Game_Manager.Instance.ReloadPlayerProperties();
    }

    public static void LevelUpReward()
    {
        BonusLevelUp();
        UpdateAccountToDB();
        Account_DAO.GetAccountPowerByID(accountRefer.ID);
        Game_Manager.Instance.ReloadPlayerProperties();
    }

    public static Equipment_Entity RandomEquipmentBonus(string CategoryEquipmentID)
    {
        var listEquipCate = listEquipment.FindAll(obj => obj.CategoryEquipmentID == CategoryEquipmentID);

        var index = UnityEngine.Random.Range(0, listEquipCate.Count);

        return listEquipCate[index];
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

public enum House
{
    Hokage, Hospital, Shop, Arena, School, Ramen, Uchiha, Casino
}

public enum StatusMission
{
    None, Doing, Claim, Done
}

public enum TrophyID
{
    Trophy_None, Trophy_Genin, Trophy_Chunin, Trophy_Jonin
}
