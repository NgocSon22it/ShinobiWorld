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
    public class RoleInGame_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<RoleInGame_Entity> GetAll()
        {
            var list = new List<RoleInGame_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Select RoleInGame.*, Weapon.Image from RoleInGame, Weapon where RoleInGame.WeaponID = Weapon.ID and RoleInGame.[Delete] = 0 and Weapon.[Delete] = 0";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new RoleInGame_Entity
                        {
                            ID = dr["ID"].ToString(),
                            WeaponID = dr["WeaponID"].ToString(),
                            Name = dr["Name"].ToString(),
                            Image = dr["Image"].ToString(),
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
