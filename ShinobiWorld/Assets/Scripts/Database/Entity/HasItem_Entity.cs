using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class HasItem_Entity
    {
        public string AccountID;
        public string ItemID;
        public int Amount;
        public int Limit;
        public bool Delete;

        public HasItem_Entity()
        {
        }
    }
}