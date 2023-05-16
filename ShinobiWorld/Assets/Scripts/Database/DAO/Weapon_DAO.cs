using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;
using Assets.Scripts.Database.Entity;

public static class Weapon_DAO 
{
    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

    public static Weapon_Entity GetWeaponByID(string WeaponID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Select * from Weapon where ID = @WeaponID";
                cmd.Parameters.AddWithValue("@WeaponID", WeaponID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new Weapon_Entity
                    {
                        ID = dr["ID"].ToString(),
                        Name = dr["Name"].ToString(),
                        Damage = Convert.ToInt32(dr["Damage"]),
                        Uppercent = Convert.ToInt32(dr["Uppercent"]),
                        UpgradeCost = Convert.ToInt32(dr["UpgradeCost"]),
                        Image = dr["Image"].ToString(),
                        Description = dr["Description"].ToString(),
                        Delete = Convert.ToBoolean(dr["Delete"]),
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
