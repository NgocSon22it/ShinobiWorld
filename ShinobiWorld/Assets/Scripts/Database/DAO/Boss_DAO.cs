using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

public static class Boss_DAO
{

    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

    public static Boss_Entity GetBossByID(string BossID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from Boss where ID = @BossID";
                cmd.Parameters.AddWithValue("@BossID", BossID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new Boss_Entity
                    {
                        ID = dr["ID"].ToString(),
                        TrophyID = dr["TrophyID"].ToString(),
                        Name = dr["Name"].ToString(),
                        Health = Convert.ToInt32(dr["Health"]),
                        Speed = Convert.ToInt32(dr["Speed"]),
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
