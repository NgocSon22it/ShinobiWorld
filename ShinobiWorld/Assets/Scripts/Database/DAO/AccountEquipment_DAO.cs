using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.DAO
{
    public class AccountEquipment_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<AccountEquipment_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<AccountEquipment_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[AccountEquipment] WHERE AccountID = @UserID and [Delete] = 0";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new AccountEquipment_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            EquipmentID = dr["EquipmentID"].ToString(),
                            Level = Convert.ToInt32(dr["Level"]),
                            Health = Convert.ToInt32(dr["Health"]),
                            Damage = Convert.ToInt32(dr["Damage"]),
                            Chakra = Convert.ToInt32(dr["Chakra"]),
                            IsUse = Convert.ToBoolean(dr["IsUse"]),
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
        public static void SellEquipment(string UserID, string EquipmentID, int price)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[SellEquipment] @UserID,@EquipmentID,@price";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                cmd.Parameters.AddWithValue("@price", price);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void UseEquipment(string UserID, string EquipmentID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[UseEquipment]  @UserID ,@EquipmentID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void RemoveEquipment(string UserID, string EquipmentID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[RemoveEquipment]  @UserID ,@EquipmentID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void UpgradeEquipment(string UserID, string EquipmentID, 
                                            int Damage, int Health, int Chakra)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Update AccountEquipment set [Level] += 1, Damage = @Damage, Health = @Health, Chakra = @Chakra where AccountID = @UserID and EquipmentID = @EquipmentID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Damage", Damage);
                cmd.Parameters.AddWithValue("@Health", Health);
                cmd.Parameters.AddWithValue("@Chakra", Chakra);
                cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
