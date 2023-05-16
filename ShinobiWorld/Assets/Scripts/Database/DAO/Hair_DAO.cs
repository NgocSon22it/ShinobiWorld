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
    public static class Hair_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Hair_Entity> GetAll()
        {
            var list = new List<Hair_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from Hair where [Delete] = 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Hair_Entity
                        {
                            ID = dr["ID"].ToString(),
                            Image = dr["Image"].ToString()
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
