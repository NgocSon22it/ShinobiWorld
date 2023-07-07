﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    [System.Serializable]
    public class HasSkill_Entity
    {
        public string AccountID;
        public string SkillID;
        public string Key;
        public int Level;
        public double Cooldown;
        public int Damage;
        public int Chakra;
        public bool Delete;

        public HasSkill_Entity()
        {
        }
    }
}