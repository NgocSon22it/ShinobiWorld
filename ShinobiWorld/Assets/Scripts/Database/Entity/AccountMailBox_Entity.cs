﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Database.Entity
{
    public class AccountMailBox_Entity
    {
        public string AccountId;
        public int MailBoxId;
        public int RankId;
        public int Rank;
        public bool IsClaim;
        public bool IsRead;
        public bool Delete;

        public AccountMailBox_Entity()
        {
        }
    }
}
