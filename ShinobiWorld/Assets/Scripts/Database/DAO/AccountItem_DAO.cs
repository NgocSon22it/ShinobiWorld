using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Database.Entity;

namespace Assets.Scripts.Database.DAO
{
    public class AccountItem_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<AccountItem_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<AccountItem_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[AccountItem] WHERE AccountID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new AccountItem_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            ItemID = dr["ItemID"].ToString(),
                            Amount = Convert.ToInt32(dr["Amount"]),
                            Limit = Convert.ToInt32(dr["Limit"]),
                            Delete = Convert.ToBoolean(dr["Delete"])
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

        public static void BuyItem(string UserID, string ItemID, int amount, int cost)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[BuyItem] @UserID,@ItemID,@amount,@cost";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@cost", cost);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void SellItem(string UserID, string ItemID, int amount, int price)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[SellItem] @UserID,@ItemID,@amount,@price";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@price", price);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void UseItem(string UserID, string ItemID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[UseItem] @UserID, @ItemID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void ResetLimitBuyItem(string UserID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[ResetLimitBuyItem] @UserID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
