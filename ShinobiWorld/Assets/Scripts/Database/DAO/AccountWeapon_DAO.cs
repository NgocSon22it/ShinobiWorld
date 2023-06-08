using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;

public static class AccountWeapon_DAO 
{
    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();
    public static AccountWeapon_Entity GetAccountWeaponByID(string UserID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from AccountWeapon where AccountID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new AccountWeapon_Entity
                    {
                        AccountID = dr["AccountID"].ToString(),
                        WeaponID = dr["WeaponID"].ToString(),
                        Level = Convert.ToInt32(dr["Level"]),
                        Damage = Convert.ToInt32(dr["Damage"]),
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
