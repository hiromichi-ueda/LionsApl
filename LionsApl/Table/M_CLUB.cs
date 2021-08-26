using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class M_CLUB
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public int Sort { get; set; }
        public string ClubCode { get; set; }
        public string ClubName { get; set; }
        public string ClubNameShort { get; set; }
        public string PassWord { get; set; }
        public string FormationDate { get; set; }
        public string CharterNight { get; set; }
        public string MeetingDate { get; set; }
        public string MeetingTime { get; set; }
        public string MeetingPlace { get; set; }
        public int AnswerDay { get; set; }
        public string AnswerTime { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string HP { get; set; }
        public string MailAddress { get; set; }
    }
}
