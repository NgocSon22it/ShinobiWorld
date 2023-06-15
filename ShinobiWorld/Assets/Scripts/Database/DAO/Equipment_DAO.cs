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
    public class Equipment_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Equipment_Entity> GetAll()
        {
            var list = new List<Equipment_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from Equipment where [Delete] = 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Equipment_Entity
                        {
                            ID = dr["ID"].ToString(),
                            TypeEquipmentID = dr["TypeEquipmentID"].ToString(),
                            CategoryEquipmentID = dr["CategoryEquipmentID"].ToString(),
                            Name = dr["Name"].ToString(),
                            Health = Convert.ToInt32(dr["Health"]),
                            Damage = Convert.ToInt32(dr["Damage"]),
                            Chakra = Convert.ToInt32(dr["Chakra"]),
                            UpgradeCost = Convert.ToInt32(dr["UpgradeCost"]),
                            SellCost = Convert.ToInt32(dr["SellCost"]),
                            Image = dr["Image"].ToString(),
                            Description = dr["Description"].ToString(),
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
    }
}
