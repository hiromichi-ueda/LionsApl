﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class A_ACCOUNT
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string ClubCode { get; set; }
        public string ClubName { get; set; }
        public string MemberCode { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string AccountDate { get; set; }
        public string LastUpdDate { get; set; }
        public string VersionNo { get; set; }
        public string BadgeLastUpdDate { get; set; }
    }
}
