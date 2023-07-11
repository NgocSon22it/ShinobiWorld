using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class HasMission_Entity
    {
        public string AccountID;
        public string MissionID;
        public int Target;
        public int Current;
        public StatusMission Status;

        public HasMission_Entity()
        {
        }
    }
}