namespace LionsApl.Table
{
    public partial class T_EVENTRET
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string EventClass { get; set; }
        public int EventDataNo { get; set; }
        public string EventDate { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string DirectorName { get; set; }
        public string Answer { get; set; }
        public string AnswerLate { get; set; }
        public string AnswerEarly { get; set; }
        public string Online { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public int OtherCount { get; set; }
        public string TargetFlg { get; set; }
        public string CancelFlg { get; set; }
    }
}
