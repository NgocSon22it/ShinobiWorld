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
    public class AccountEquipment_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<AccountEquipment_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<AccountEquipment_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[AccountEquipment] WHERE AccountID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new AccountEquipment_Entity
                        {
                            AccountID = dr["AccountID"].ToString(),
                            EquipmentID = dr["EquipmentID"].ToString(),
                            Level = Convert.ToInt32(dr["Level"]),
                            Health = Convert.ToInt32(dr["Health"]),
                            Damage = Convert.ToInt32(dr["Damage"]),
                            Chakra = Convert.ToInt32(dr["Chakra"]),
                            IsUse = Convert.ToBoolean(dr["IsUse"]),
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
