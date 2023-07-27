using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Trophy_Entity
    {
        public string ID;
        public string BossID;
        public string Name;
        public int Cost;
        public int ContraitLevelAccount;
        public int Health;
        public int Speed;
        public string Description;
        public bool Delete;

        public Trophy_Entity()
        {
        }
    }
}
