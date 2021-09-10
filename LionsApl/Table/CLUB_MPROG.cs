using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class CLUB_MPROG
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        // T_MEETINGPROGRAM
        public int Id { get; set; }
        public int DataNo { get; set; }
        public int ScheduleDataNo { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string Meeting { get; set; }
        // T_MEETINGSCHEDULE
        public string MeetingDate { get; set; }
        public string MeetingTime { get; set; }
        public string MeetingPlace { get; set; }
        public string MeetingName { get; set; }
    }
}
