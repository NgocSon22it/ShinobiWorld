using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Equipment_Entity
    {
        public int ID;
        public int TypeEquipmentID;
        public string Name;
        public int Health;
        public int Damage;
        public int Chakra;
        public int UpgradeCost;
        public int SellCost;
        public string Image;
        public string Description;
        public bool Delete;

        public Equipment_Entity()
        {
        }
    }
}
