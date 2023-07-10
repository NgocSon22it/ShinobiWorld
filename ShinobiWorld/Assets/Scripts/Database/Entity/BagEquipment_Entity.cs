 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class BagEquipment_Entity
    {
        public int ID;
        public string AccountID;
        public string EquipmentID;
        public int Level;
        public int Health;
        public int Damage;
        public int Chakra;
        public bool IsUse;
        public bool Delete;

        public BagEquipment_Entity()
        {
        }
    }
}