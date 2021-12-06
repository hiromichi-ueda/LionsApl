using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_MATCHING
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public long DataNo { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string MemberName { get; set; }
        public string CompanyName { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string HP { get; set; }
        public string FileName { get; set; }
        public string Area { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
    }
}
