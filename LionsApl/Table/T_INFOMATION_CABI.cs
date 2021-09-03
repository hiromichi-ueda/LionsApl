using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_INFOMATION_CABI
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string AddDate { get; set; }
        public string Subject { get; set; }
        public string Detail { get; set; }
        public string FileName { get; set; }
        public string InfoFlg { get; set; }
        public string InfoUser { get; set; }
        public string NoticeFlg { get; set; }
    }
}
