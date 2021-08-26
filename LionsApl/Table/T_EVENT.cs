namespace LionsApl.Table
{
    class T_EVENT
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string EventDate { get; set; }
        public string EventTimeStart { get; set; }
        public string EventTimeEnd { get; set; }
        public string ReceptionTime { get; set; }
        public string EventPlace { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
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
        public string Meeting { get; set; }
        public string MeetingUrl { get; set; }
        public string MeetingID { get; set; }
        public string MeetingPW { get; set; }
        public string MeetingOther { get; set; }
        public string AnswerDate { get; set; }
        public string FileName { get; set; }
        public string CabinetUser { get; set; }
        public string ClubUser { get; set; }
        public string NoticeFlg { get; set; }
    }
}
