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
    public class Item_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Item_Entity> GetAll()
        {
            var list = new List<Item_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from Item where [Delete] = 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Item_Entity
                        {
                            ID = dr["ID"].ToString(),
                            Name = dr["Name"].ToString(),
                            HealthBonus = Convert.ToInt32(dr["HealthBonus"]),
                            ChakraBonus = Convert.ToInt32(dr["ChakraBonus"]),
                            DamageBonus = Convert.ToInt32(dr["DamageBonus"]),
                            BuyCost = Convert.ToInt32(dr["BuyCost"]),
                            Limit = Convert.ToInt32(dr["Limit"]),
                            Image = dr["Image"].ToString(),
                            Description = dr["Description"].ToString()
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
    }
}
