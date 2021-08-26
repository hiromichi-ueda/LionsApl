using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_CLUBSLOGAN
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string FiscalStart { get; set; }
        public string FiscalEnd { get; set; }
        public string ClubSlogan { get; set; }
        public string ExecutiveName { get; set; }
    }
}
