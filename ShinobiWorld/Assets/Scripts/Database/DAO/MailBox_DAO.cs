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
    public class MailBox_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<MailBox_Entity> GetAll()
        {
            var list = new List<MailBox_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT *  FROM [dbo].[MailBox]";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new MailBox_Entity
                        {
                            ID = dr["ID"].ToString(),
                            CategoryEquipmentID = dr["CategoryEquipmentID"].ToString(),
                            Amount = Convert.ToInt32(dr["Amount"]),
                            Title = dr["Title"].ToString(),
                            Content = dr["Content"].ToString(),
                            Rank = Convert.ToInt32(dr["Rank"]),
                            CoinBonus = Convert.ToInt32(dr["CoinBonus"]),
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
