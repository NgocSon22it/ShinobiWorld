using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class MailBox_Entity
    {
        public string ID; 
        public string CategoryEquipmentID;
        public int Amount;
        public string Title;
        public string Content;
        public int Rank;
        public int CoinBonus;
        public bool Delete;

        public MailBox_Entity()
        {
        }
    }
}
