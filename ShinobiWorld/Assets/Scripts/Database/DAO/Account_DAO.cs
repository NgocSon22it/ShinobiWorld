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
    //public static class Account_DAO
    //{
    //    static string ConnectionStr = new ShinobiWorldConnect().GetConnectShinobiWorld();

    //    public static void CreateAccount(Account_Entity UserID)
    //    {
    //        using (SqlConnection connection = new SqlConnection(ConnectionStr))
    //        {
    //            SqlCommand cmd = connection.CreateCommand();
    //            cmd.CommandText = "Insert into Account values(@name,@username,@password,0,'0',0,0,1,'map1_1',1,3,0,0,0)";
    //            cmd.Parameters.AddWithValue("@username", account.Username);
    //            cmd.Parameters.AddWithValue("@password", GetMD5(account.Password));
    //            cmd.Parameters.AddWithValue("@name", account.Name);
    //            connection.Open();
    //            cmd.ExecuteNonQuery();
    //            connection.Close();
    //        }
    //    }
    //}
}
