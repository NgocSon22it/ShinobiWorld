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
    public class TypeEquipment_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<TypeEquipment_Entity> GetAll()
        {
            var list = new List<TypeEquipment_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from TypeEquipment where [Delete] = 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new TypeEquipment_Entity
                        {
                            ID = dr["ID"].ToString(),
                            Name = dr["Name"].ToString(),
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
