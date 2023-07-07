﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    [System.Serializable]
    public class HasWeapon_Entity
    {
        public string AccountID;
        public string WeaponID;
        public int Level;
        public int Damage;
        public bool Delete;

        public HasWeapon_Entity()
        {
        }
    }
}