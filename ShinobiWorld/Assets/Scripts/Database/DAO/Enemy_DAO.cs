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
    public static class Enemy_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static Enemy_Entity GetEnemyByID(string EnemyID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "select * from Enemy where ID = @EnemyID";
                    cmd.Parameters.AddWithValue("@EnemyID", EnemyID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Enemy_Entity
                        {
                            ID = dr["ID"].ToString(),
                            Name = dr["Name"].ToString(),
                            Health = Convert.ToInt32(dr["Health"]),
                            Speed = Convert.ToInt32(dr["Speed"]),
                            CoinBonus = Convert.ToInt32(dr["CoinBonus"]),
                            ExpBonus = Convert.ToInt32(dr["ExpBonus"]),
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
    }
}
