using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_MEETINGPROGRAM
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public int ScheduleDataNo { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string Fiscal { get; set; }
        public string MeetingDate { get; set; }
        public int MeetingCount { get; set; }
        public string MeetingName { get; set; }
        public string FileName { get; set; }
        public string FileName1 { get; set; }
        public string FileName2 { get; set; }
        public string FileName3 { get; set; }
        public string FileName4 { get; set; }
        public string FileName5 { get; set; }
        public string Meeting { get; set; }
        public string MeetingUrl { get; set; }
        public string MeetingID { get; set; }
        public string MeetingPW { get; set; }
        public string MeetingOther { get; set; }
        public string NoticeFlg { get; set; }
    }
}
