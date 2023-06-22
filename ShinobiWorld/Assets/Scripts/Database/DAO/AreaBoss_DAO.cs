using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;
using System.Data.SqlTypes;

public static class AreaBoss_DAO
{
    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

    public static AreaBoss_Entity GetAreaBossByID(string ID, string BossID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from AreaBoss where ID = @ID and BossID = @BossID";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@BossID", BossID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new AreaBoss_Entity
                    {
                        ID = dr["ID"].ToString(),
                        AreaID = dr["AreaID"].ToString(),
                        BossID = dr["BossID"].ToString(),
                        isDead = Convert.ToBoolean(dr["isDead"]),
                        TimeSpawn = Convert.ToDateTime(dr["TimeSpawn"]),
                        CurrentHealth = Convert.ToInt32(dr["CurrentHealth"]),
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

    public static void UpdateHealthAreaBoss(AreaBoss_Entity areaBoss_Entity)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Update AreaBoss set isDead = @isDead, CurrentHealth = @CurrentHealth where ID = @ID and BossID = @BossID";
            cmd.Parameters.AddWithValue("@ID", areaBoss_Entity.ID);
            cmd.Parameters.AddWithValue("@isDead", areaBoss_Entity.isDead);
            cmd.Parameters.AddWithValue("@BossID", areaBoss_Entity.BossID);
            cmd.Parameters.AddWithValue("@CurrentHealth", areaBoss_Entity.CurrentHealth);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void SetAreaBossDie(string AreaID, string BossID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Update AreaBoss set isDead = 1, TimeSpawn = DATEADD(second, 10, GETDATE()) where ID = @ID and BossID = @BossID";
            cmd.Parameters.AddWithValue("@ID", AreaID);
            cmd.Parameters.AddWithValue("@BossID", BossID);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
