using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References
{
    public static Account_Entity accountRefer = new Account_Entity();

    public static int Maxserver = 20;

    public static List<Skin_Entity> listSkin = Skin_DAO.GetAll();
    public static List<Eye_Enity> listEye = Eye_DAO.GetAll();
    public static List<Hair_Entity> listHair = Hair_DAO.GetAll();
    public static List<Mouth_Entity> listMouth = Mouth_DAO.GetAll();

    public static List<RoleInGame_Entity> listRole = RoleInGame_DAO.GetAll();
    public static List<Item_Entity> listItem = Item_DAO.GetAll();

    public static List<Skill_Entity> ListSkill = Skill_DAO.GetAllSkill();

    public static void LoadAccount()
    {
        if (accountRefer != null)
        {
            Account_DAO.LoadAccount(accountRefer);
        }
    }
}
