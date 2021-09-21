using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class EVENT_LIST
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
        // T_EVENT
        public string EventPlace { get; set; }
        public string Title { get; set; }
        // T_MEETINGSCHEDULE
        public string MeetingName { get; set; }
        public string MeetingPlace { get; set; }
        // T_DIRECTOR
        public string Subject { get; set; }
        public string ClubEventClass { get; set; }
        public string ClubEventPlace { get; set; }
    }
}
