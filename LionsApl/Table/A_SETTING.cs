using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class A_SETTING
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string CabinetName { get; set; }
        public string PeriodStart { get; set; }
        public string PeriodEnd { get; set; }
        public string DistrictID { get; set; }
        public string MagazineMoney { get; set; }
        public string EventDataDay { get; set; }
        public string AdminID { get; set; }
        public string AdminName { get; set; }
        public string AdminPassWord { get; set; }
        public string CabinetEmailAddeess { get; set; }
        public string AdminEmailAddress { get; set; }
        public string CabinetTelNo { get; set; }
        public string AdminTelNo { get; set; }
    }
}
