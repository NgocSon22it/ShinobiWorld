using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public static class Disconnect
    {
        public static void WriteFile()
        {
            string filePath = Path.Combine(Application.dataPath, "CheckConnect.txt");

            // Text content you want to
            string textToWrite = string.Empty;
            if (References.accountRefer != null) textToWrite = References.accountRefer.ID;

            // Write the text to the file
            using (StreamWriter writer = new StreamWriter(filePath, false)) // 'false' means overwrite existing content
            {
                writer.Write(textToWrite);
            }
        }

        public static bool ReadFile()
        {
            // Path to the file you want to read
            string filePath = Path.Combine(Application.dataPath, "CheckConnect.txt");

            // Check if the file exists
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                // Read the text from the file
                if(fileContent != null)
                        Account_DAO.ChangeStateOnline(fileContent, false);
                File.Delete(filePath);
                return true;
            }
            else
            {

                return false;

            }
        }
    }
}
