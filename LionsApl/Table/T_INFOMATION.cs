
namespace LionsApl.Table
{
    class T_INFOMATION
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string AddDate { get; set; }
        public string Subject { get; set; }
        public string Detail { get; set; }
        public string FileName { get; set; }
        public string InfoFlg { get; set; }
        public string TypeCode { get; set; }
        public string InfoUser { get; set; }
        public string NoticeFlg { get; set; }
    }
}
