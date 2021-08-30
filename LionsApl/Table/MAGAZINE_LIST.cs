using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class MAGAZINE_LIST
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
        // T_MAGAZINEBUY
        public int MagazineDataNo { get; set; }
    }
}
