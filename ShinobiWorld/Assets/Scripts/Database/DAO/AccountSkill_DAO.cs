using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;

public static class AccountSkill_DAO
{
    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();
    public static AccountSkill_Entity GetAccountSkillByID(string UserID, string SkillID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Select * from AccountSkill where AccountID = @UserID and SkillID = @SkillID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@SkillID", SkillID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new AccountSkill_Entity
                    {
                        AccountID = dr["AccountID"].ToString(),
                        SkillID = dr["SkillID"].ToString(),
                        Level = Convert.ToInt32(dr["Level"]),
                        Cooldown = Convert.ToDouble(dr["Cooldown"]),
                        Damage = Convert.ToInt32(dr["Damage"]),
                        Chakra = Convert.ToInt32(dr["Chakra"]),
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
    public static List<AccountSkill_Entity> GetAllSkillForAccount(string UserID)
    {
        var list = new List<AccountSkill_Entity>();
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Select * from AccountSkill where AccountID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new AccountSkill_Entity
                    {
                        AccountID = dr["AccountID"].ToString(),
                        SkillID = dr["SkillID"].ToString(),
                        Level = Convert.ToInt32(dr["Level"]),
                        Cooldown = Convert.ToDouble(dr["Cooldown"]),
                        Damage = Convert.ToInt32(dr["Damage"]),
                        Chakra = Convert.ToInt32(dr["Chakra"]),
                        Delete = Convert.ToBoolean(dr["Delete"])
                    };
                    list.Add(obj);
                };

            }
            finally
            {
                connection.Close();
            }
        }

        return list;
    }

    
}
