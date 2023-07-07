using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShinobiWorldConnect
{
    static string Server = "139.180.190.203,1433";
    static string id = "sa";
    static string password = "Uyw$l9*L1D@yx#Gt";
    static string database = "Shinobi";

    public static string GetConnectShinobiWorld()
    {
        return $"Server = {Server}; uid = {id}; pwd = {password}; Database = {database};";
    }

    //static string Server = "(local)";
    //static string id = "sa";
    //static string password = "123456";
    //static string database = "ninjagame_";

    //public static string GetConnectShinobiWorld()
    //{
    //    return $"Server = {Server}; uid = {id}; pwd = {password}; Database = {database};";
    //}
}
