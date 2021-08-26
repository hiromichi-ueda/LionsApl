using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class M_CABINET
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string FiscalStart { get; set; }
        public string FiscalEnd { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string DistrictClass { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictClass1 { get; set; }
        public string DistrictCode1 { get; set; }
        public string DistrictName1 { get; set; }
        public string DistrictClass2 { get; set; }
        public string DistrictCode2 { get; set; }
        public string DistrictName2 { get; set; }
        public string DistrictClass3 { get; set; }
        public string DistrictCode3 { get; set; }
        public string DistrictName3 { get; set; }
        public string DistrictClass4 { get; set; }
        public string DistrictCode4 { get; set; }
        public string DistrictName4 { get; set; }
        public string DistrictClass5 { get; set; }
        public string DistrictCode5 { get; set; }
        public string DistrictName5 { get; set; }

    }
}
