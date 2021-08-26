using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl.Table
{
    class M_MEMBER
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameShort { get; set; }
        public string MemberCode { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberNameKana { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string Sex { get; set; }
        public string JoinDate { get; set; }
        public string LeaveDate { get; set; }
        public string LeaveFlg { get; set; }
        public string AccountDate { get; set; }
        public string ExecutiveCode { get; set; }
        public string ExecutiveName { get; set; }
        public string ExecutiveCode1 { get; set; }
        public string ExecutiveName1 { get; set; }
        public string CommitteeCode { get; set; }
        public string CommitteeName { get; set; }
        public string CommitteeFlg { get; set; }
        public string CommitteeCode1 { get; set; }
        public string CommitteeName1 { get; set; }
        public string CommitteeFlg1 { get; set; }
        public string CommitteeCode2 { get; set; }
        public string CommitteeName2 { get; set; }
        public string CommitteeFlg2 { get; set; }
        public string CommitteeCode3 { get; set; }
        public string CommitteeName3 { get; set; }
        public string CommitteeFlg3 { get; set; }
        public string J_ExecutiveCode { get; set; }
        public string J_ExecutiveName { get; set; }
        public string J_ExecutiveCode1 { get; set; }
        public string J_ExecutiveName1 { get; set; }
        public string J_CommitteeCode { get; set; }
        public string J_CommitteeName { get; set; }
        public string J_CommitteeFlg { get; set; }
        public string J_CommitteeCode1 { get; set; }
        public string J_CommitteeName1 { get; set; }
        public string J_CommitteeFlg1 { get; set; }
        public string J_CommitteeCode2 { get; set; }
        public string J_CommitteeName2 { get; set; }
        public string J_CommitteeFlg2 { get; set; }
        public string J_CommitteeCode3 { get; set; }
        public string J_CommitteeName3 { get; set; }
        public string J_CommitteeFlg3 { get; set; }
    }
}
