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
    public class Mission_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Mission_Entity> GetAll()
        {
            var list = new List<Mission_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select * from Mission where [Delete] = 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Mission_Entity
                        {
                            ID = dr["ID"].ToString(),
                            TrophyID = dr["TrophyID"].ToString(),
                            EnemyID = dr["EnemyID"].ToString(),
                            CategoryEquipmentID = dr["CategoryEquipmentID"].ToString(),
                            RequiredStrength = Convert.ToInt32(dr["RequiredStrength"]),
                            Content = dr["Content"].ToString(),
                            Target = Convert.ToInt32(dr["Target"]),
                            ExpBonus = Convert.ToInt32(dr["ExpBonus"]),
                            CoinBonus = Convert.ToInt32(dr["CoinBonus"]),
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
