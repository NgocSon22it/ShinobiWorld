using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Skill_Entity
    {
        public int ID;
        public int RoleInGameID;
        public string Name;
        public double Cooldown;
        public int Damage;
        public int Chakra;
        public int Uppercent;
        public int LevelUnlock;
        public int UpgradeCost;
        public int BuyCost;
        public string Image;
        public string Description;
        public bool Delete;

        public Skill_Entity()
        {
        }
    }
}
