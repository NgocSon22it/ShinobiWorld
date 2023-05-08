using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Trophy_Entity
    {
        public int Id;
        public int BossId;
        public string Name;
        public int ContraitLevelAccount;
        public int Cost;
        public string Image;
        public string Description;
        public bool Delete;

        public Trophy_Entity()
        {
        }
    }
}
