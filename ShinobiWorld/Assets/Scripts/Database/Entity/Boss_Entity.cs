using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Boss_Entity
    {
        public string ID;
        public string TypeBossID;
        public string Name;
        public int Health;
        public int Speed;
        public int CoinBonus;
        public int ExpBonus;
        public string Image;
        public string Description;
        public bool Delete;

        public Boss_Entity()
        {
        }
    }
}