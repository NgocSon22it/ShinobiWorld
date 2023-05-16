using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Rank_Entity
    {
        public string ID;
        public string EquipmentIDbonus;
        public bool IsEvent;
        public int Rank;
        public int CoinBonus;
        public int EquipmentAmount;
        public bool Delete;

        public Rank_Entity()
        {
        }
    }
}
