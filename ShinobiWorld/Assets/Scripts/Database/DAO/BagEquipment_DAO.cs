﻿using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.DAO
{
    public class BagEquipment_DAO
    {
        static string ConnectionStr = ShinobiWorldConnect.GetConnectShinobiWorld();

        public static List<BagEquipment_Entity> GetAllByUserID(string UserID)
        {
            var list = new List<BagEquipment_Entity>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM [dbo].[BagEquipment] WHERE AccountID = @UserID and [Delete] = 0";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var obj = new BagEquipment_Entity
                        {
                            ID = Convert.ToInt32(dr["ID"]),
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
        public static void SellEquipment(int ID, string UserID, string EquipmentID, int price)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "EXECUTE [dbo].[SellEquipment] @ID,@UserID,@EquipmentID,@price";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void UseEquipment(int ID, string UserID, string EquipmentID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "EXECUTE [dbo].[UseEquipment]  @ID, @UserID ,@EquipmentID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void RemoveEquipment(int ID, string UserID, string EquipmentID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "EXECUTE [dbo].[RemoveEquipment]  @ID ,@UserID ,@EquipmentID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void UpgradeEquipment(int ID, string UserID, string EquipmentID, 
                                            int Damage, int Health, int Chakra)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Update BagEquipment " +
                                        "set [Level] += 1, Damage += @Damage, Health += @Health, Chakra += @Chakra " +
                                        "where AccountID = @UserID " +
                                        "and EquipmentID = @EquipmentID " +
                                        "and ID = @ID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Damage", Damage);
                    cmd.Parameters.AddWithValue("@Health", Health);
                    cmd.Parameters.AddWithValue("@Chakra", Chakra);
                    cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public static void DowngradeEquipment(int ID, string UserID, string EquipmentID,
                                           int Damage, int Health, int Chakra)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "Update BagEquipment " +
                                        "set [Level] = 1, Damage = @Damage, Health = @Health, Chakra = @Chakra " +
                                        "where AccountID = @UserID " +
                                        "and EquipmentID = @EquipmentID " +
                                        "and ID = @ID";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Damage", Damage);
                    cmd.Parameters.AddWithValue("@Health", Health);
                    cmd.Parameters.AddWithValue("@Chakra", Chakra);
                    cmd.Parameters.AddWithValue("@EquipmentID", EquipmentID);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    Console.WriteLine("SQL Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }
    }
}
