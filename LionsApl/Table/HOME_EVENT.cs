using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    internal class HOME_EVENT
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string EventClass { get; set; }
        public int EventDataNo { get; set; }
        public string EventDate { get; set; }
        public string ClubCode { get; set; }
        public string MemberCode { get; set; }
        public string Answer { get; set; }
        public string CancelFlg { get; set; }
        // 会議名
        // T_EVENT
        public string Title { get; set; }
        // T_MEETINGSCHEDULE
        public string MeetingName { get; set; }
        // T_DIRECTOR
        public string Subject { get; set; }
        // 回答期限
        // T_EVENT
        public string AnswerDateEv { get; set; }
        // T_MEETINGSCHEDULE
        public string AnswerDateMe { get; set; }
        public string AnswerTime { get; set; }
        // T_DIRECTOR
        public string AnswerDateDi { get; set; }
    }
}
