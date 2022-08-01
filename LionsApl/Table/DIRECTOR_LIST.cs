using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class DIRECTOR_LIST
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int DataNo { get; set; }
        public string EventClass { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string Subject { get; set; }
        public string Member { get; set; }
        public string MemberAdd { get; set; }
        public string CancelFlg { get; set; }
        public string Answer { get; set; }
        public string Badge { get; set; }
    }
}
