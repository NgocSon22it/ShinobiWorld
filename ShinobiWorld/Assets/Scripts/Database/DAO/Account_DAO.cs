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
        static string ConnectionStr = new ShinobiWorldConnect().GetConnectShinobiWorld();

        public static void CreateAccount(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText =   "INSERT INTO [Account] ([ID],[RoleInGameID],[TrophiesID],[Level],[Health],[Charka],[Exp],[Speed],[Coin],[Power],[Strength]," +
                                                            "[EyeID],[HairID],[FaceID],[SkinID],[ColorID],[IsDead],[IsOnline],[IsTicket],[Delete])" +
                                    "VALUES (@UserID,@RoleInGameID,@TrophiesID,1,100,100,0,5,0,0,100," +
                                            "@EyeID,@HairID,@FaceID,@SkinID,@ColorID,0,0,0,0)";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RoleInGameID", 1);
                cmd.Parameters.AddWithValue("@TrophiesID", 1);
                cmd.Parameters.AddWithValue("@EyeID", 1);
                cmd.Parameters.AddWithValue("@HairID", 1);
                cmd.Parameters.AddWithValue("@FaceID", 1);
                cmd.Parameters.AddWithValue("@SkinID", 1);
                cmd.Parameters.AddWithValue("@ColorID", 1);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
