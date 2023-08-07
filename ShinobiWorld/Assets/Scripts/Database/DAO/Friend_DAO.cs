using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Friend;

namespace Assets.Scripts.Database.DAO
{
    public static class Friend_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Friend_Entity> GetAll(string UserID)
        {
            var list = new List<Friend_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[Friend] where (FriendAccountID = @UserID or MyAccountID = @UserID)";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Friend_Entity
                        {
                            MyAccountID = dr["MyAccountID"].ToString(),
                            FriendAccountID = dr["FriendAccountID"].ToString(),
                            IsFriend = Convert.ToBoolean(dr["IsFriend"])
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

        public static List<FriendInfo> GetAllFriendInfo(List<string> listFriendAccountID)
        {
            var list = new List<FriendInfo>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].Account where ID = @UserID";
                    cmd.Parameters.Add("@UserID", SqlDbType.NVarChar);

                    foreach (var ID in listFriendAccountID)
                    {
                        cmd.Parameters["@UserID"].Value = ID;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow dr in dataTable.Rows)
                        {
                            var obj = new FriendInfo
                            {
                                ID = dr["ID"].ToString(),
                                Name = dr["Name"].ToString(),
                                TrophyID = dr["TrophyID"].ToString(),
                                IsOnline = Convert.ToBoolean(dr["IsOnline"])
                            };

                            list.Add(obj);
                        }
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

        public static void AcceptFriend(string MyAccountID, string FriendAccountID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Update Friend set IsFriend = 1  where MyAccountID = @UserID and FriendAccountID =  @FriendAccountID";
                    cmd.Parameters.AddWithValue("@UserID", MyAccountID);
                    cmd.Parameters.AddWithValue("@FriendAccountID", FriendAccountID);
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

        public static void DeleteFriend(string MyAccountID, string FriendAccountID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM [dbo].[Friend] " +
                        "WHERE (MyAccountID = @UserID and FriendAccountID = @FriendAccountID) " +
                        "or (MyAccountID = @FriendAccountID and FriendAccountID = @UserID)";
                    cmd.Parameters.AddWithValue("@UserID", MyAccountID);
                    cmd.Parameters.AddWithValue("@FriendAccountID", FriendAccountID);
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

        public static void AddFriend(string MyAccountID, string FriendAccountID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO [dbo].[Friend] ([MyAccountID],[FriendAccountID]) VALUES (@FriendAccountID ,@UserID)";
                    cmd.Parameters.AddWithValue("@UserID", MyAccountID);
                    cmd.Parameters.AddWithValue("@FriendAccountID", FriendAccountID);
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

        public static void UpdateFriend(string MyAccountID, string FriendAccountID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Update Friend set [Delete] = 0, IsFriend = 0  " +
                                        "where (MyAccountID = @UserID and FriendAccountID =  @FriendAccountID) " +
                                        "or (MyAccountID = @FriendAccountID and FriendAccountID =  @UserID)";
                    cmd.Parameters.AddWithValue("@UserID", MyAccountID);
                    cmd.Parameters.AddWithValue("@FriendAccountID", FriendAccountID);
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

    }
}
