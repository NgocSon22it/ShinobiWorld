using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Mission_Entity
    {
        public string ID;
        public string TrophyID;
        public string BossID;
        public string CategoryEquipmentID;
        public int RequiredStrength;
        public string Content;
        public int Target;
        public int ExpBonus;
        public int CoinBonus;
        public bool Delete;

        public Mission_Entity()
        {
        }
    }
}
