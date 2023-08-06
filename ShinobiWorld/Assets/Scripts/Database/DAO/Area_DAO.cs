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
    public static class Area_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<Area_Entity> GetAllAreaByMissionID(string MissionID)
        {
            var list = new List<Area_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "exec TeleportMission @MissionID";
                    cmd.Parameters.AddWithValue("@MissionID", MissionID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new Area_Entity
                        {
                            ID = dr["ID"].ToString(),
                            MapID = dr["MapID"].ToString(),
                            Name = dr["Name"].ToString(),
                            XPosition = (float) Convert.ToDouble(dr["XPosition"]),
                            YPosition = (float) Convert.ToDouble(dr["YPosition"]),
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
