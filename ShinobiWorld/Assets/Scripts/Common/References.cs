using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
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

    public static string PlayerName;

    public static List<AccountItem_Entity> listAccountItem = new List<AccountItem_Entity>();
    public static List<AccountEquipment_Entity> listAccountEquipment = new List<AccountEquipment_Entity>();

    public static List<AccountSkill_Entity> listAccountSkill = new List<AccountSkill_Entity>();

    public static AccountWeapon_Entity accountWeapon = new AccountWeapon_Entity();

    public static Weapon_Entity weapon = new Weapon_Entity();
    public static List<AccountMission_Entity> listAccountMission = new List<AccountMission_Entity>();
    public static List<AccountMailBox_Entity> listAccountMailBox = new List<AccountMailBox_Entity>();

    public static int Maxserver = 20;

    public static List<Skin_Entity> listSkin = Skin_DAO.GetAll();
    public static List<Eye_Enity> listEye = Eye_DAO.GetAll();
    public static List<Hair_Entity> listHair = Hair_DAO.GetAll();
    public static List<Mouth_Entity> listMouth = Mouth_DAO.GetAll();

    public static List<RoleInGame_Entity> listRole = RoleInGame_DAO.GetAll();
    public static List<Item_Entity> listItem = Item_DAO.GetAll();
    public static List<Equipment_Entity> listEquipment = Equipment_DAO.GetAll();
    public static List<TypeEquipment_Entity> listTypeEquipment = TypeEquipment_DAO.GetAll();
    public static List<MailBox_Entity> listMailBox = MailBox_DAO.GetAll();

    public static IDictionary<string, string> BtnTrophies = new Dictionary<string, string>();
    public static List<Trophy_Entity> listTrophy = Trophy_DAO.GetAll();

    public static List<Mission_Entity> listMission = Mission_DAO.GetAll();

    public static List<Skill_Entity> ListSkill = Skill_DAO.GetAllSkill();

    public static AccountSkill_Entity accountSkillOne = new AccountSkill_Entity();
    public static AccountSkill_Entity accountSkillTwo = new AccountSkill_Entity();
    public static AccountSkill_Entity accountSkillThree = new AccountSkill_Entity();

    public static float Uppercent_Skill_Damage = 3f, Uppercent_Skill_Chakra = 1f, Uppercent_Skill_CoolDown = 1f;
    public static float Uppercent_Account = 5f;
    public static float Uppercent_Equipment = 5f;
    public static int MaxUpgradeLevel = 30; 

    public static int RespawnTime = 20;
    public static int RespawnCost = 1000;

    public static int HealthBonus, ChakraBonus, StrengthBonus;

    public static int ExpercienceToNextLevel;

    public static bool IsDisconnect = false;

    public static string TrophyID_RemakeMission = TrophiesID.Trophie_Jonin.ToString();

    public static string UISkillDefault = "Background/UI_OrangeFill";
    public static string UIEquipmentDefault = "Background/UI_GreenFill";
    public static string UIEquipmentShow = "Background/UI_Green";
    public static string UIInfoSelected = "Background/UI_Blue";

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

    public static void LoadAccountWeaponNSkill(string Role)
    {
        if (accountRefer != null)
        {
            accountWeapon = AccountWeapon_DAO.GetAccountWeaponByID(accountRefer.ID);
            accountSkillOne = AccountSkill_DAO.GetAccountSkillByID(accountRefer.ID, "Skill_" + Role + "One");
            accountSkillTwo = AccountSkill_DAO.GetAccountSkillByID(accountRefer.ID, "Skill_" + Role + "Two");
            accountSkillThree = AccountSkill_DAO.GetAccountSkillByID(accountRefer.ID, "Skill_" + Role + "Three");
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
        UpdateAccountToDB();
        Game_Manager.Instance.ReloadPlayerProperties();
    }

    public static void LevelUpReward()
    {
        BonusLevelUp();
        UpdateAccountToDB();
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

public enum TrophiesID
{
    Trophie_None, Trophie_Genin, Trophie_Chunin, Trophie_Jonin
}
