﻿using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.DAO
{
    public class AccountMission_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<AccountMission_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<AccountMission_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[AccountMission] WHERE AccountID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new AccountMission_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            MissionID = dr["MissionID"].ToString(),
                            Target = Convert.ToInt32(dr["Target"]),
                            Current = Convert.ToInt32(dr["Current"]),
                            Status = (StatusMission) Convert.ToInt32(dr["Status"])
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

        public static void ChangeStatusMission(string UserID, string MissionID, StatusMission status)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[AccountMission] SET [Status] = @status WHERE AccountID = @UserID and MissionID = @MissionID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MissionID", MissionID);
                cmd.Parameters.AddWithValue("@status", status);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void TakeBonus(string UserID, string MissionID, int Status, string EquipmentID1)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[TakeBonus]  @AccountID, @MissionID, @Status, " +
                                                                "@EquipmentID1, @EquipmentID2," +
                                                                "@ID_AccountMailBox, @MailBoxID";
                cmd.Parameters.AddWithValue("@AccountID", UserID);
                cmd.Parameters.AddWithValue("@MissionID", MissionID);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@EquipmentID1", EquipmentID1);
                cmd.Parameters.AddWithValue("@EquipmentID2", null);
                cmd.Parameters.AddWithValue("@ID_AccountMailBox", null);
                cmd.Parameters.AddWithValue("@MailBoxID", null);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void DoingMission(string UserID, string MissionID, int Current)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[AccountMission] SET [Current] = @Current WHERE AccountID = @UserID and MissionID = @MissionID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MissionID", MissionID);
                cmd.Parameters.AddWithValue("@Current", Current);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
