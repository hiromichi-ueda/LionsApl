using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class M_DISTRICTOFFICER
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string DistrictClass { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }

    }
}
