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
    public class CustomSetting_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static CustomSetting_Entity GetAccountCustomSettingBySettingID(string UserID, string SettingID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from CustomSetting where AccountID = @UserID and SettingID = @SettingID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@SettingID", SettingID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new CustomSetting_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            SettingID = dr["SettingID"].ToString(),
                            Value = dr["Value"].ToString(),
                            Delete = Convert.ToBoolean(dr["Delete"])                                                
                        };
                        connection.Close();
                        return obj;
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

            return null;
        }

        public static List<CustomSetting_Entity> GetAllAccountCustomSetting(string UserID)
        {
            var list = new List<CustomSetting_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from CustomSetting where AccountID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new CustomSetting_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            SettingID = dr["SettingID"].ToString(),
                            Value = dr["Value"].ToString(),
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
    }
}
