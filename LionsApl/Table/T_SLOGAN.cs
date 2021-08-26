namespace LionsApl.Table
{
    class T_SLOGAN
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string FiscalStart { get; set; }
        public string FiscalEnd { get; set; }
        public string Slogan { get; set; }
        public string DistrictGovernor { get; set; }
    }
}
