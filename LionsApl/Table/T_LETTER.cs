
namespace LionsApl.Table
{
    class T_LETTER
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image1FileName { get; set; }
        public string Image2FileName { get; set; }
        public string NoticeFlg { get; set; }
    }
}
