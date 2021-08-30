using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_MAGAZINE
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public int SortNo { get; set; }
        public string Magazine { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string MagazineClass { get; set; }
        public int MagazinePrice { get; set; }
        public string OpenFlg { get; set; }
        public string DelFlg { get; set; }
    }
}
