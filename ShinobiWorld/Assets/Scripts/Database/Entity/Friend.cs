using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Friend
    {
        public string ID;
        public string MyAccountID;
        public string FriendAccountID;
        public int IsFriend;
        public bool Delete;

        public Friend()
        {
        }
    }
}
