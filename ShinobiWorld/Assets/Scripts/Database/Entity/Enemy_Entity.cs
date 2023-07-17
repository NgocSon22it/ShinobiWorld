using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    [System.Serializable]
    public class Enemy_Entity
    {
        public string ID;
        public string Name;
        public int Health;
        public int Speed;
        public int CoinBonus;
        public int ExpBonus;
        public bool Delete;

        public Enemy_Entity()
        {
        }

        public static object Deserialize(byte[] data)
        {
            var result = new Enemy_Entity();
            result.ID = data[0].ToString();

            return result;
        }

        public static byte[] Serialize(object customType)
        {
            var c = (Enemy_Entity)customType;
            return new byte[] { Convert.ToByte(c.ID) };
        }
    }
}
