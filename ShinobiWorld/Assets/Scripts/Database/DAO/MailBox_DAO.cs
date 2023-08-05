using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Assets.Scripts.Database.DAO
{
    public class MailBox_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<MailBox_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<MailBox_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[MailBox] " +
                                        "WHERE AccountID = @UserID and [Delete] = 0";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new MailBox_Entity
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            AccountID = dr["AccountID"].ToString(),
                            MailID = dr["MailID"].ToString(),
                            AddDate = Convert.ToDateTime(dr["AddDate"]),
                            IsClaim = Convert.ToBoolean(dr["IsClaim"]),
                            IsRead = Convert.ToBoolean(dr["IsRead"]),
                            Delete = Convert.ToBoolean(dr["Delete"])
                        };

                        list.Add(obj);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }

            return list;
        }

        public static void Read(int ID, string UserID, string MailID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE [dbo].[MailBox] SET IsRead = 1 WHERE  AccountID = @UserID and MailID = @MailID and ID = @ID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@MailID", MailID);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void Delete(int ID, string UserID, string MailID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE [dbo].[MailBox] SET [Delete] = 1 WHERE  AccountID = @UserID and MailID = @MailID and ID = @ID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@MailID", MailID);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void DeleteReadAll(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE [dbo].[MailBox] SET [Delete] = 1 WHERE AccountID = @UserID and IsRead = 1";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                
            }
        }

        public static void DeleteReadAndReceivedBonus(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE [dbo].[MailBox] SET [Delete] = 1 WHERE AccountID = @UserID and IsRead = 1 and IsClaim = 1";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void TakeBonus(int MailBoxID, string UserID, string MailID, string EquipmentID1, string EquipmentID2)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "EXECUTE [dbo].[TakeBonus]  @AccountID, @MissionID, @Status, " +
                                                                    "@EquipmentID1, @EquipmentID2," +
                                                                    "@MailBoxID, @MailID";
                    cmd.Parameters.AddWithValue("@AccountID", UserID);
                    cmd.Parameters.AddWithValue("@MissionID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                    cmd.Parameters.AddWithValue("@EquipmentID1", EquipmentID1);
                    cmd.Parameters.AddWithValue("@EquipmentID2", (string.IsNullOrEmpty(EquipmentID2) ? DBNull.Value : EquipmentID2));
                    cmd.Parameters.AddWithValue("@MailBoxID", MailBoxID);
                    cmd.Parameters.AddWithValue("@MailID", MailID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void AddMailbox(string UserID, string MailID, bool IsClaim = false)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO [dbo].[MailBox]  ([AccountID] ,[MailID], [IsClaim]) VALUES (@UserID ,@MailID, @IsClaim)";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@MailID", MailID);
                    cmd.Parameters.AddWithValue("@IsClaim", IsClaim);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static bool CheckSentMailRank(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[MailBox] WHERE AccountID = @UserID and AddDate = CAST( GETDATE() AS Date )";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0) return true; else return false;
                }
                finally
                {
                    connection.Close();
                }

            }
        }
    }
}
