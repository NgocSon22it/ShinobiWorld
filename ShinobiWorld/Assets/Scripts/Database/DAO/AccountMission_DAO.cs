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
    public class AccountMission_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<AccountMission_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<AccountMission_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[AccountMission] WHERE AccountID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new AccountMission_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            MissionID = Convert.ToInt32(dr["MissionID"]),
                            Target = Convert.ToInt32(dr["Target"]),
                            Current = Convert.ToInt32(dr["Current"]),
                            Status = Convert.ToBoolean(dr["Status"])
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

        public static void ChangeStatusMission(string UserID, int MissionID, bool status)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[AccountMission] SET [Status] = @status WHERE AccountID = @UserID and MissionID = @MissionID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MissionID", MissionID);
                cmd.Parameters.AddWithValue("@status", status);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
