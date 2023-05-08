using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Item_Entity
    {
        public int Id;
        public string Name;
        public int HealthBonus;
        public int ChakraBonus;
        public int DamageBonus;
        public int BuyCost;
        public int Limit;
        public string Image;
        public string Description;
        public bool Delete;

        public Item_Entity()
        {
        }
    }
}
