using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class AccountItem_Entity
    {
        public string AccountID;
        public string ItemID;
        public int Amount;
        public bool Limit;
        public bool Delete;

        public AccountItem_Entity()
        {
        }
    }
}