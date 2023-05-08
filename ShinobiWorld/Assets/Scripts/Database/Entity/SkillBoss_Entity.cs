using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class SkillBoss_Entity
    {
        public int ID;
        public int BossID;
        public string Name;
        public int Damage;
        public string Image;
        public string Description;
        public bool Delete;

        public SkillBoss_Entity()
        {
        }
    }
}
