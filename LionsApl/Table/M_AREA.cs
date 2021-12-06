using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class M_AREA
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int SortNo { get; set; }
        public string AreaName { get; set; }
    }
}
