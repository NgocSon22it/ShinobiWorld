using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Assets.Scripts.Database.DAO
{
    public static class Account_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static Account_Entity GetAccountByID(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "select * from Account where ID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Account_Entity
                        {
                            ID = dr["ID"].ToString(),
                            RoleInGameID = Convert.ToInt32(dr["RoleInGameID"]),
                            TrophiesID = Convert.ToInt32(dr["TrophiesID"]),
                            Level = Convert.ToInt32(dr["Level"]),
                            Health = Convert.ToInt32(dr["Health"]),
                            Charka = Convert.ToInt32(dr["Charka"]),
                            Exp = Convert.ToInt32(dr["Exp"]),
                            Speed = Convert.ToInt32(dr["Speed"]),
                            Coin = Convert.ToInt32(dr["Coin"]),
                            Power = Convert.ToInt32(dr["Power"]),
                            Strength = Convert.ToInt32(dr["Strength"]),
                            EyeID = Convert.ToInt32(dr["EyeID"]),
                            HairID = Convert.ToInt32(dr["HairID"]),
                            MouthID = Convert.ToInt32(dr["MouthID"]),
                            SkinID = Convert.ToInt32(dr["SkinID"]),
                            IsDead = Convert.ToBoolean(dr["IsDead"]),
                            IsOnline = Convert.ToBoolean(dr["IsOnline"]),
                            IsTicket = Convert.ToBoolean(dr["IsTicket"]),
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

        public static void CreateAccount(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =   "INSERT INTO [Account] ([ID],[RoleInGameID],[TrophiesID],[Level],[Health],[Charka],[Exp],[Speed],[Coin],[Power],[Strength]," +
                                                            "[EyeID],[HairID],[MouthID],[SkinID],[IsDead],[IsOnline],[IsTicket],[Delete])" +
                                    "VALUES (@UserID,@RoleInGameID,@TrophiesID,1,100,100,0,5,0,0,100," +
                                            "@EyeID,@HairID,@MouthID,@SkinID,0,0,0,0)";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RoleInGameID", 1);
                cmd.Parameters.AddWithValue("@TrophiesID", 1);
                cmd.Parameters.AddWithValue("@EyeID", 1);
                cmd.Parameters.AddWithValue("@HairID", 1);
                cmd.Parameters.AddWithValue("@MouthID", 1);
                cmd.Parameters.AddWithValue("@SkinID", 1);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void ChangeStateOnline(string UserID, bool stateOnline)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[Account] SET [IsOnline] = @stateOnline WHERE Account.ID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@stateOnline", stateOnline);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static bool StateOnline(string UserID)
        {
            var isOnline = false;

            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT [IsOnline] FROM [dbo].[Account] WHERE Account.ID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        isOnline = Convert.ToBoolean(dr["IsOnline"]);
                        connection.Close();
                    }
                }
                finally
                {
                    connection.Close();
                }

            }

            return isOnline;
        }
    }
}
