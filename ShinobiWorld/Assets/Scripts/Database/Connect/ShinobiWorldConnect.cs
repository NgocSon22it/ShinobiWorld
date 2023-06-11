using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShinobiWorldConnect
{
    static string Server = "(local)";
    static string id = "sa";
    static string password = "123456";
    static string database = "ninjagame_";

    public static string GetConnectShinobiWorld()
    {
        return $"Server = {Server}; uid = {id}; pwd = {password}; Database = {database}; Trusted_Connection = False;";
    }
}
