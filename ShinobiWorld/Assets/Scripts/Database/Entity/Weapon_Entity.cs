using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Weapon_Entity
    {
        public int ID;
        public string Name;
        public int Damage;
        public int Uppercent;
        public int UpgradeCost;
        public string Image;
        public string Description;
        public bool Delete;

        public Weapon_Entity()
        {
        }
    }
}
