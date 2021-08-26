using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class A_FILEPATH
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string DataClass { get; set; }
        public string FilePath { get; set; }
    }
}
