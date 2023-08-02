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
    public class Trophy_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Trophy_Entity> GetAll()
        {
            var list = new List<Trophy_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT *  FROM [dbo].[Trophy] order by Health asc";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Trophy_Entity
                        {
                            ID = dr["ID"].ToString(),
                            BossID = dr["BossID"].ToString(),
                            Name = dr["Name"].ToString(),
                            Cost = Convert.ToInt32(dr["Cost"]),
                            ContraitLevelAccount = Convert.ToInt32(dr["ContraitLevelAccount"]),
                            Health = Convert.ToInt32(dr["Health"]),
                            Speed = Convert.ToInt32(dr["Speed"]),
                            Delete = Convert.ToBoolean(dr["Delete"])
                        };
                        list.Add(obj);
                        References.BtnTrophy.Add(obj.ID, obj.Name);
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
