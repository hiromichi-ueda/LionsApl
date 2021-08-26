using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class A_APLLOG
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogClass { get; set; }
        public string MachineClass { get; set; }
        public string ClubCode { get; set; }
        public string ClubName { get; set; }
        public string MembwrCode { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string ViewName { get; set; }
        public string KeyValue { get; set; }
        public string ErrSql { get; set; }
        public string ErrMsg { get; set; }
    }
}
