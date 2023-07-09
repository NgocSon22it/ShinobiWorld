using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Friend_Entity
    {
        public int ID;
        public string MyAccountID;
        public string FriendAccountID;
        public bool IsFriend;
        public bool Delete;

        public Friend_Entity()
        {
        }
    }
}
