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

        public static void CreateAccount(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO [dbo].[Account] ([ID]) VALUES (@UserID)";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void UpgradeTrophyRegister(string UserID, int status)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Update Account set IsTicket = @status where ID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@status", status);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }


        public static void UpdateAccountCoin(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Update Account set Coin = Coin + 100 where ID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
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

        public static void SaveLayout(string UserID, string RoleInGameID, string EyeID, string HairID, string MouthID, string SkinID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[Account]   " +
                                    "SET [RoleInGameID] = @RoleInGameID," +
                                    "[EyeID]   = @EyeID," +
                                    "[HairID]  = @HairID," +
                                    "[MouthID] = @MouthID," +
                                    "[SkinID]  = @SkinID, " +
                                    "[IsFirst] = 0 " +
                                    "WHERE ID  = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RoleInGameID", RoleInGameID);
                cmd.Parameters.AddWithValue("@EyeID", EyeID);
                cmd.Parameters.AddWithValue("@HairID", HairID);
                cmd.Parameters.AddWithValue("@MouthID", MouthID);
                cmd.Parameters.AddWithValue("@SkinID", SkinID);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static Account_Entity GetAccountByID(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[Account] WHERE ID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Account_Entity
                        {
                            ID = dr["ID"].ToString(),
                            RoleInGameID = dr["RoleInGameID"].ToString(),
                            TrophiesID = dr["TrophiesID"].ToString(),
                            Level = Convert.ToInt32(dr["Level"]),
                            Health = Convert.ToInt32(dr["Health"]),
                            CurrentHealth = Convert.ToInt32(dr["CurrentHealth"]),
                            Chakra = Convert.ToInt32(dr["Chakra"]),
                            CurrentChakra = Convert.ToInt32(dr["CurrentChakra"]),
                            Exp = Convert.ToInt32(dr["Exp"]),
                            Speed = Convert.ToInt32(dr["Speed"]),
                            Coin = Convert.ToInt32(dr["Coin"]),
                            Power = Convert.ToInt32(dr["Power"]),
                            Strength = Convert.ToInt32(dr["Strength"]),
                            CurrentStrength = Convert.ToInt32(dr["CurrentStrength"]),
                            EyeID = dr["EyeID"].ToString(),
                            HairID = dr["HairID"].ToString(),
                            MouthID = dr["MouthID"].ToString(),
                            SkinID = dr["SkinID"].ToString(),
                            IsDead = Convert.ToBoolean(dr["IsDead"]),
                            IsOnline = Convert.ToBoolean(dr["IsOnline"]),
                            IsTicket = Convert.ToBoolean(dr["IsTicket"]),
                            IsFirst = Convert.ToBoolean(dr["IsFirst"]),
                            DateReset = Convert.ToDateTime(dr["DateReset"])
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

        public static int GetAccountPowerByID(string UserID)
        {
            int Power = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Exec GetPower @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        if (dr["Power"] != DBNull.Value)
                        {
                            Power = Convert.ToInt32(dr["Power"]);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return Power;
        }

        public static bool IsFirstLogin(string UserID)
        {
            var IsFirst = false;

            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT [IsFirst] FROM [dbo].[Account] WHERE Account.ID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IsFirst = Convert.ToBoolean(dr["IsFirst"]);
                        connection.Close();
                    }
                }
                finally
                {
                    connection.Close();
                }

            }
            return IsFirst;
        }
        public static void UpdateAccountToDB(Account_Entity account_Entity)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Update Account set TrophiesID = @Trophie, [Level] = @Level, Health = @Health, CurrentHealth = @CurrentHealth, Chakra = @Chakra, CurrentChakra = @CurrentChakra, [Exp] = @Exp, Coin = @Coin, [Power] = @Power, Strength = @Strenth, CurrentStrength = @CurrentStrength where ID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", account_Entity.ID);
                cmd.Parameters.AddWithValue("@Trophie", account_Entity.TrophiesID);
                cmd.Parameters.AddWithValue("@Level", account_Entity.Level);
                cmd.Parameters.AddWithValue("@Health", account_Entity.Health);
                cmd.Parameters.AddWithValue("@CurrentHealth", account_Entity.CurrentHealth);
                cmd.Parameters.AddWithValue("@Chakra", account_Entity.Chakra);
                cmd.Parameters.AddWithValue("@CurrentChakra", account_Entity.CurrentChakra);
                cmd.Parameters.AddWithValue("@Exp", account_Entity.Exp);
                cmd.Parameters.AddWithValue("@Coin", account_Entity.Coin);
                cmd.Parameters.AddWithValue("@Power", account_Entity.Power);
                cmd.Parameters.AddWithValue("@Strenth", account_Entity.Strength);
                cmd.Parameters.AddWithValue("@CurrentStrength", account_Entity.CurrentStrength);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
