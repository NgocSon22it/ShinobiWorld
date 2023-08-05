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
    public class Setting_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Setting_Entity> GetAllSetting()
        {
            var list = new List<Setting_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "select * from Setting";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Setting_Entity
                        {
                            ID = dr["ID"].ToString(),
                            Name = dr["Name"].ToString(),
                            Value = dr["Value"].ToString(),
                            Delete = Convert.ToBoolean(dr["Delete"]),
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
