using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Mission_Entity
    {
        public int ID;
        public string Content;
        public int Level;
        public int Target;
        public int ExpBonus;
        public int CoinBonus;
        public string Image;
        public bool Delete;

        public Mission_Entity()
        {
        }
    }
}
