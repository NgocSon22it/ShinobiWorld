﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class Event_Entity
    {
        public int ID;
        public int BossID;
        public string Name;
        public int Weekday;
        public string Description;
        public bool Delete;

        public Event_Entity()
        {
        }
    }
}