using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class MailBox_Entity
    {
        public int ID;
        public string Title;
        public string Content;
        public bool IsRank;
        public bool Delete;

        public MailBox_Entity()
        {
        }
    }
}
