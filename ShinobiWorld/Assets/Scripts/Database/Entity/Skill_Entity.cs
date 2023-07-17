using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Skill_Entity
    {
        public string ID;
        public string RoleInGameID;
        public string Name;
        public double Cooldown;
        public int Damage;
        public int Chakra;
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
