using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class T_DIRECTOR
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int DataNo { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string EventClass { get; set; }
        public string CommitteeCode { get; set; }
        public string CommitteeName { get; set; }
        public string Season { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventPlace { get; set; }
        public string Subject { get; set; }
        public string Agenda { get; set; }
        public string Member { get; set; }
        public string MemberAdd { get; set; }
        public string AnswerDate { get; set; }
        public string CancelFlg { get; set; }
        public string NoticeFlg { get; set; }
    }
}
