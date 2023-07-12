using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Assets.Scripts.Database.Entity;

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

        public static void UpgradeTrophyRegister(string UserID, bool status)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Update Account set HasTicket = @status where ID = @UserID";
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
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();

                    if (stateOnline)
                    {
                        var LastLoginDate = new DateTime();
                        var Strength = 0;
                        var CurrentStrength = 0;

                        cmd.CommandText = "SELECT [LastLoginDate], Strength, CurrentStrength FROM [dbo].[Account] WHERE Account.ID = @UserID";
                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow dr in dataTable.Rows)
                        {
                            LastLoginDate = Convert.ToDateTime(dr["LastLoginDate"]);
                            Strength = Convert.ToInt32(dr["Strength"]);
                            CurrentStrength = Convert.ToInt32(dr["CurrentStrength"]);
                        }

                        var strengthUpdate = (int)(DateTime.Now - LastLoginDate).TotalMinutes / 6 + CurrentStrength;
                        CurrentStrength = (strengthUpdate >= Strength) ? Strength : strengthUpdate;

                        cmd.CommandText = "UPDATE [dbo].[Account] " +
                                            "SET [IsOnline] = @stateOnline, " +
                                            "LastLoginDate = GETDATE(), " +
                                            "CurrentStrength = @CurrentStrength " +
                                            "WHERE Account.ID = @UserID";

                        cmd.Parameters.AddWithValue("@stateOnline", stateOnline);
                        cmd.Parameters.AddWithValue("@CurrentStrength", CurrentStrength);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE [dbo].[Account] " +
                                           "SET [IsOnline] = @stateOnline, " +
                                           "LastLoginDate = GETDATE()" +
                                           "WHERE Account.ID = @UserID";

                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@stateOnline", stateOnline);
                        cmd.ExecuteNonQuery();
                    }

                }
                finally
                {
                    connection.Close();
                }
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

        public static void SaveLayout(string UserID, string Name, string RoleInGameID, string EyeID, string HairID, 
            string MouthID, string SkinID)
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
                                    "[Name]  = @Name, " +
                                    "[IsFirst] = 0 " +
                                    "WHERE ID  = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Name", Name);
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
                            Name = dr["Name"].ToString(),
                            RoleInGameID = dr["RoleInGameID"].ToString(),
                            TrophyID = dr["TrophyID"].ToString(),
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
                            HasTicket = Convert.ToBoolean(dr["HasTicket"]),
                            IsFirst = Convert.ToBoolean(dr["IsFirst"]),
                            IsHokage = Convert.ToBoolean(dr["IsHokage"]),
                            WinTimes = Convert.ToInt32(dr["WinTimes"]),
                            IsUpgradeTrophy = Convert.ToBoolean(dr["IsUpgradeTrophy"]),
                            ResetLimitDate = Convert.ToDateTime(dr["ResetLimitDate"])
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
                cmd.CommandText = "Update Account set TrophyID = @Trophy, [Level] = @Level, Health = @Health, CurrentHealth = @CurrentHealth, Chakra = @Chakra, CurrentChakra = @CurrentChakra, [Exp] = @Exp, Coin = @Coin, [Power] = @Power, Strength = @Strenth, CurrentStrength = @CurrentStrength where ID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", account_Entity.ID);
                cmd.Parameters.AddWithValue("@Trophy", account_Entity.TrophyID);
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

        public static void UpgradeTrophy(string UserID, string TrophyID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Update Account set TrophyID = @TrophyID, IsUpgradeTrophy = 0 where ID = @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@TrophyID", TrophyID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<Account_Entity> GetAllAccount()
        {
            var list = new List<Account_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[Account]";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Account_Entity
                        {
                            ID = dr["ID"].ToString(),
                            Name = dr["Name"].ToString(),
                            TrophyID = dr["TrophyID"].ToString(),
                            Level = Convert.ToInt32(dr["Level"]),
                            Power = Convert.ToInt32(dr["Power"])
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

        public static bool IsDisplayNameExist(string displayname)
        {
            var isExist = false;
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select [Name] from Account where [Name]= @Name";
                    cmd.Parameters.AddWithValue("@Name", displayname);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    isExist = (dataTable.Rows.Count > 0);
                }
                finally
                {
                    connection.Close();
                }
            }
            return isExist;
        }
    }
}
