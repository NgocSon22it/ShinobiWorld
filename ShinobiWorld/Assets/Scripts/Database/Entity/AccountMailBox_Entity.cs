﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class AccountMailBox_Entity
    {
        public int ID;
        public string AccountID;
        public string MailBoxID;
        public DateTime DateAdd;
        public bool IsClaim;
        public bool IsRead;
        public bool Delete;

        public AccountMailBox_Entity()
        {
        }
    }
}