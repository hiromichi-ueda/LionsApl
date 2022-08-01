
namespace LionsApl.Table
{
    public class T_BADGE
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string DataClass { get; set; }
        public int DataNo { get; set; }
        public string ClubCode { get; set; }
        public string MemberCode { get; set; }
    }
}
