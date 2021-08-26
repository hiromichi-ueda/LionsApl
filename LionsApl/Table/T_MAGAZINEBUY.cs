using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_MAGAZINEBUY
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public int MagazineDataNo { get; set; }
        public string Magazine { get; set; }
        public string BuyDate { get; set; }
        public int BuyNumber { get; set; }
        public int MagazinePrice { get; set; }
        public int MoneyTotal { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string ShippingDate { get; set; }
        public string PaymentDate { get; set; }
        public string Payment { get; set; }
        public string DelFlg { get; set; }
    }
}
