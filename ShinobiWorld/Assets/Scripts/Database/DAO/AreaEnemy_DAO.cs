using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;
using System.Data.SqlTypes;

public static class AreaEnemy_DAO
{
    static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

    public static AreaEnemy_Entity GetAreaEnemyByID(string ID, string EnemyID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from AreaEnemy where ID = @ID and EnemyID = @EnemyID";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@EnemyID", EnemyID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dr in dataTable.Rows)
                {
                    var obj = new AreaEnemy_Entity
                    {
                        ID = dr["ID"].ToString(),
                        AreaID = dr["AreaID"].ToString(),
                        EnemyID = dr["EnemyID"].ToString(),
                        IsDead = Convert.ToBoolean(dr["isDead"]),
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

    public static void UpdateHealthAreaEnemy(AreaEnemy_Entity AreaEnemy_Entity)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Update AreaEnemy set IsDead = @IsDead, CurrentHealth = @CurrentHealth where ID = @ID and EnemyID = @EnemyID";
            cmd.Parameters.AddWithValue("@ID", AreaEnemy_Entity.ID);
            cmd.Parameters.AddWithValue("@IsDead", AreaEnemy_Entity.IsDead);
            cmd.Parameters.AddWithValue("@EnemyID", AreaEnemy_Entity.EnemyID);
            cmd.Parameters.AddWithValue("@CurrentHealth", AreaEnemy_Entity.CurrentHealth);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void SetAreaEnemyDie(string AreaID, string EnemyID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionStr))
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Update AreaEnemy set IsDead = 1, TimeSpawn = DATEADD(second, 10, GETDATE()) where ID = @ID and EnemyID = @EnemyID";
            cmd.Parameters.AddWithValue("@ID", AreaID);
            cmd.Parameters.AddWithValue("@EnemyID", EnemyID);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
