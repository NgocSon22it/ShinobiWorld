using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinobiWorldConnect
{
    string Server = "(local)";
    string id = "sa";
    string password = "123456";
    string database = "Shinobi";

    public string GetConnectShinobiWorld()
    {
        return $"Server = {Server}; uid = {id}; pwd = {password}; Database = {database}; Trusted_Connection = False;";
    }
}
