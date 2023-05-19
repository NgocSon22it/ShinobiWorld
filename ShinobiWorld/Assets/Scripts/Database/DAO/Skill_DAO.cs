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
                        Uppercent = Convert.ToInt32(dr["Uppercent"]),
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

}
