using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class M_JOB
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int JobCode { get; set; }
        public string JobName { get; set; }
    }
}
