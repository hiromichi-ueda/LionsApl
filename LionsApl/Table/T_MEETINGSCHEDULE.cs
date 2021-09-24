namespace LionsApl.Table
{
    public partial class T_MEETINGSCHEDULE
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string Fiscal { get; set; }
        public string MeetingDate { get; set; }
        public string MeetingTime { get; set; }
        public string MeetingPlace { get; set; }
        public int MeetingCount { get; set; }
        public string MeetingName { get; set; }
        public string Online { get; set; }
        public string AnswerDate { get; set; }
        public string AnswerTime { get; set; }
        public string RemarksCheck { get; set; }
        public string RemarksItems { get; set; }
        public string Remarks { get; set; }
        public string OptionName1 { get; set; }
        public string OptionRadio1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionRadio2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionRadio3 { get; set; }
        public string OptionName4 { get; set; }
        public string OptionRadio4 { get; set; }
        public string OptionName5 { get; set; }
        public string OptionRadio5 { get; set; }
        public string Sake { get; set; }
        public string OtherUser { get; set; }
        public string CancelFlg { get; set; }
    }
}
