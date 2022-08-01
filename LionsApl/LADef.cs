using System;
using System.Collections.Generic;
using System.Text;

namespace LionsApl
{
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// LionsApl用定数クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    class LADef
    {
        // DB項目判定用文字列
        //
        public const string SEX_MAIL = "1";                 // 男性
        public const string SEX_FEMAIL = "2";               // 女性
        // イベントクラス
        public const string EVENTCLASS_EV = "1";            // キャビネットイベント
        public const string EVENTCLASS_ME = "2";            // 年間例会スケジュール
        public const string EVENTCLASS_DI = "3";            // 理事・委員会
        // クラブイベントクラス
        public const string CLUBEVENTCLASS_RI = "1";        // 理事会
        public const string CLUBEVENTCLASS_IN = "2";        // 委員会
        public const string CLUBEVENTCLASS_ET = "3";        // その他
        // 中止フラグ
        public const string CANCELFLG = "1";                // 中止
        // 出欠フラグ
        public const string ANSWER_PRE = "1";               // 出席
        public const string ANSWER_ABS = "2";               // 欠席
        public const string ANSWER_NO = "";                 // 未回答（初期値）
        // オンラインフラグ
        public const string ONLINEFLG = "1";
        // 会議フラグ
        public const string MEETING_ONLINE = "1";           // オンライン会議
        //public const string MEETING_NORMAL = "1";
        //public const string MEETING_ONLINE = "2";
        // 時期区分
        public const string SEASON_NOW = "1";               // 今期
        public const string SEASON_NEXT = "2";              // 次期
        // 連絡区分
        public const string INFOFLG_ALL = "1";              // 全会員
        public const string INFOFLG_PRIV = "2";             // 個別設定
        // データ種別
        public const string DATACLASS_EV = "1";            // 出欠
        public const string DATACLASS_DI = "2";            // 理事・委員会
        public const string DATACLASS_IN = "3";            // 連絡事項

        // 各種判定用文字列
        public const string OFFFLG = "0";                   // 区分：OFF
        public const string ONFLG = "1";                    // 区分：ON
        public const string NOFLG = "";                     // 区分：なし
        public const string NOTIME = "00:00";               // 時間設定なし
        public const string NOSTR = "";                     // 文字列なし

        public const bool NOFILE = false;                   // ファイルが存在しない
        public const bool EXFILE = true;                    // ファイルが存在する

        // 出力文字列
        public const string ST_NOSTR = "";
        public const string ST_CANCEL = "中止";
        public const string ST_SEASON_NOW = "今期";
        public const string ST_SEASON_NEXT = "時期";
        public const string ST_ON = "有り";
        public const string ST_OFF = "無し";
        public const string ST_MEETING_NORMAL = "通常";
        public const string ST_MEETING_ONLINE = "オンライン";
        public const string ST_BOARD = "理事会";
        public const string ST_COMM = "委員会";
        public const string ST_ETC = "その他";
        public const string ST_ANSWER_NO = "未回答";
        public const string ST_ANSWER_PRE = "出席";
        public const string ST_ANSWER_ABS = "欠席";
        public const string ST_MALE = "男性";
        public const string ST_FEMALE = "女性";
        public const string ST_OTHER = "その他";
        public const string ST_CHAIRMAN = "委員長";
        public const string ST_VICECHAIRMAN = "副委員長";
        public const string ST_BADGE = "未読";
        public const string ST_TOP_BADGE = "(未読あり)";


        // 基本文字列
        public const string STRCOL_STRDEF = "#151515";
        public const string STRCOL_RED = "#FF0000";
    }
}
