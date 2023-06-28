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
    public class AccountMailBox_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<AccountMailBox_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<AccountMailBox_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[AccountMailBox] " +
                                        "WHERE AccountID = @UserID and [Delete] = 0";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new AccountMailBox_Entity
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            AccountID = dr["AccountID"].ToString(),
                            MailBoxID = dr["MailBoxID"].ToString(),
                            DateAdd = Convert.ToDateTime(dr["DateAdd"]),
                            IsClaim = Convert.ToBoolean(dr["IsClaim"]),
                            IsRead = Convert.ToBoolean(dr["IsRead"]),
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

        public static void Read(int ID, string UserID, string MailBoxID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[AccountMailBox] SET IsRead = 1 WHERE  AccountID = @UserID and MailBoxID = @MailBoxID and ID = @ID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MailBoxID", MailBoxID);
                cmd.Parameters.AddWithValue("@ID", ID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Delete(int ID, string UserID, string MailBoxID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [dbo].[AccountMailBox] SET [Delete] = 1 WHERE  AccountID = @UserID and MailBoxID = @MailBoxID and ID = @ID";
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MailBoxID", MailBoxID);
                cmd.Parameters.AddWithValue("@ID", ID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void TakeBonus(int ID, string UserID, string MailBoxID, string EquipmentID1, string EquipmentID2)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "EXECUTE [dbo].[TakeBonus]  @AccountID, @MissionID, @Status, " +
                                                                "@EquipmentID1, @EquipmentID2," +
                                                                "@ID_AccountMailBox, @MailBoxID";
                cmd.Parameters.AddWithValue("@AccountID", UserID);
                cmd.Parameters.AddWithValue("@MissionID", string.Empty);
                cmd.Parameters.AddWithValue("@Status", string.Empty);
                cmd.Parameters.AddWithValue("@EquipmentID1", EquipmentID1);
                cmd.Parameters.AddWithValue("@EquipmentID2", EquipmentID2);
                cmd.Parameters.AddWithValue("@ID_AccountMailBox", ID);
                cmd.Parameters.AddWithValue("@MailBoxID", MailBoxID);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
