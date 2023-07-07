using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;
using Assets.Scripts.Database.Entity;

public static class Skill_DAO
{
    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

    public static Skill_Entity GetSkillByID(string SkillID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Select * from Skill where ID =  @SkillID";
                cmd.Parameters.AddWithValue("@SkillID", SkillID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new Skill_Entity
                    {
                        ID = dr["ID"].ToString(),
                        RoleInGameID = dr["RoleInGameID"].ToString(),
                        Name = dr["Name"].ToString(),
                        Cooldown = Convert.ToDouble(dr["Cooldown"]),
                        Damage = Convert.ToInt32(dr["Damage"]),
                        Chakra = Convert.ToInt32(dr["Chakra"]),
                        LevelUnlock = Convert.ToInt32(dr["LevelUnlock"]),
                        UpgradeCost = Convert.ToInt32(dr["UpgradeCost"]),
                        BuyCost = Convert.ToInt32(dr["BuyCost"]),
                        Image = dr["Image"].ToString(),
                        Description = dr["Description"].ToString(),
                        Delete = Convert.ToBoolean(dr["Delete"])
                    };
                    connection.Close();
                    return obj;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        return null;
    }

    public static List<Skill_Entity> GetAllSkill()
    {
        var list = new List<Skill_Entity>();
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Select * from Skill where [Delete] = 0";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new Skill_Entity
                    {
                        ID = dr["ID"].ToString(),
                        RoleInGameID = dr["RoleInGameID"].ToString(),
                        Name = dr["Name"].ToString(),
                        Cooldown = Convert.ToDouble(dr["Cooldown"]),
                        Damage = Convert.ToInt32(dr["Damage"]),
                        Chakra = Convert.ToInt32(dr["Chakra"]),
                        LevelUnlock = Convert.ToInt32(dr["LevelUnlock"]),
                        UpgradeCost = Convert.ToInt32(dr["UpgradeCost"]),
                        BuyCost = Convert.ToInt32(dr["BuyCost"]),
                        Image = dr["Image"].ToString(),
                        Description = dr["Description"].ToString(),
                        Delete = Convert.ToBoolean(dr["Delete"])
                    };
                    list.Add(obj);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        return list;
    }

    public static void BuySkill(string UserID, Skill_Entity skill_Entity)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Insert Into HasSkill  ([AccountID] ,[SkillID] ,[Level] ,[Cooldown] ,[Damage] ,[Chakra] ,[Key]) values(@UserID, @SkillID, @Level, @Cooldown, @Damage, @Chakra, @Key)";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@SkillID", skill_Entity.ID);
            cmd.Parameters.AddWithValue("@Level", 1);
            cmd.Parameters.AddWithValue("@Cooldown", skill_Entity.Cooldown);
            cmd.Parameters.AddWithValue("@Damage", skill_Entity.Damage);
            cmd.Parameters.AddWithValue("@Chakra", skill_Entity.Chakra);
            cmd.Parameters.AddWithValue("@)\";", skill_Entity.Key);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void UpgradeSkill(string UserID, string SkillID, int Damage, double Cooldown, int Chakra)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Update HasSkill set [Level] += 1, Damage = @Damage, Cooldown = @Cooldown, Chakra = @Chakra where AccountID = @UserID and SkillID = @SkillID";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@Damage", Damage);
            cmd.Parameters.AddWithValue("@Cooldown", Cooldown);
            cmd.Parameters.AddWithValue("@Chakra", Chakra);
            cmd.Parameters.AddWithValue("@SkillID", SkillID);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

}
