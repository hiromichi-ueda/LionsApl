using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LionsApl
{
    internal class LAUtility
    {
        // DB項目判定用文字列
        // イベントクラス
        public string EVENTCLASS_EV = "1";            // キャビネットイベント
        public string EVENTCLASS_ME = "2";            // 年間例会スケジュール
        public string EVENTCLASS_DI = "3";            // 理事・委員会
        // クラブイベントクラス
        public string CLUBEVENTCLASS_RI = "1";        // 理事会
        public string CLUBEVENTCLASS_IN = "2";        // 委員会
        // 中止フラグ
        public string CANCELFLG = "1";                // 中止
        // 出欠フラグ
        public string ANSWER_PRE = "1";               // 出席
        public string ANSWER_AB = "2";                // 欠席
        public string ANSWER_NO = "";                 // 未回答（初期値）
        // オンラインフラグ
        public string ONLINEFLG = "1";
        // 会議フラグ
        public string MEETING_ONLINE = "1";
        //public string MEETING_NORMAL = "1";
        //public string MEETING_ONLINE = "2";
        // 時期区分
        public string SEASON_NOW = "1";               // 今期
        public string SEASON_NEXT = "2";              // 次期
        // 連絡区分
        public string INFOFLG_ALL = "1";              // 全会員
        public string INFOFLG_PRIV = "2";                  // 個別設定


        // 引数用文字列
        public string NLC_OFF = "0";                  // 改行を削除しない。
        public string NLC_ON = "1";                   // 改行を削除する。

        // 各種判定用文字列
        public string OFFFLG = "0";                   // 区分：OFF
        public string ONFLG = "1";                    // 区分：ON
        public string NOFLG = "";                     // 区分：なし
        public string NOTIME = "00:00";               // 時間設定なし

        public bool NOFILE = false;                   // ファイルが存在しない
        public bool EXFILE = true;                    // ファイルが存在する

        // 出力文字列
        public string ST_NOSTR = "";
        public string ST_CANCEL = "中止";
        public string ST_SEASON_NOW = "今期";
        public string ST_SEASON_NEXT = "時期";
        public string ST_ON = "有り";
        public string ST_OFF = "無し";
        public string ST_MEETING_NORMAL = "通常";
        public string ST_MEETING_ONLINE = "オンライン";
        public string ST_BOARD = "理事会";
        public string ST_COMM = "委員会";
        public string ST_ANSWER_NO = "未回答";
        public string ST_ANSWER_ATT = "出席";
        public string ST_ANSWER_ABS = "欠席";

        // 基本文字列
        public const string STRCOL_STRDEF = "#151515";
        public const string STRCOL_RED = "Red";


        public LAUtility()
        {

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 文字列関連

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したstringがnullならstring.Emptyを返す
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string GetString(string str)
        {
            string retStr = string.Empty;
            if (str != null)
            {
                retStr = str;
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したstringがnullならstring.Emptyを返す
        /// 改行コード削除フラグがON（NLC_ON）の場合は入力値から改行コードを削除して返す
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string GetString(string str, string nlcFlg)
        {
            string retStr = string.Empty;
            if (str != null)
            {
                retStr = str;
                // 改行削除
                if (nlcFlg == NLC_ON)
                {
                    retStr = DelNewLine(retStr);
                }
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したstringがstring.Emptyなら「NULL」、文字列なら「'文字列'」を返す
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string GetSQLString(string str)
        {
            string retStr = string.Empty;
            if (str.Equals(string.Empty))
            {
                retStr = "NULL";
            }
            else
            {
                retStr = "'" + str + "'";
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したstringがNOTIMEと同じならstring.Emptyを返す
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string GetTimeString(string str)
        {
            string retStr = string.Empty;
            if (str != null)
            {
                if (str != string.Empty)
                {
                    if (!str.Equals(NOTIME))
                    {
                        retStr = str;
                    }
                }
            }
            return retStr;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したstringがnull、string.Empty以外なら先頭から10桁を返す。
        /// null、string.Emptyならstring.Emptyを返す。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string GetDateString(string str)
        {
            string retStr = string.Empty;
            if (str != null)
            { 
                if (str != string.Empty)
                {
                    retStr = str.Substring(0, 10);
                }
            }

            return retStr;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 文字列から改行コードを削除する。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string DelNewLine(string str)
        {
            string retStr = string.Empty;

            retStr = str;
            retStr = retStr.Replace("\r", "").Replace("\n", "");
            retStr = retStr.Replace("Environment.NewLine", "");
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したフラグから0：「無し」、1：「有り」、それ以外：string.Empty の文字列を返す
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrOnOff(string item)
        {
            string retStr = string.Empty;
            if (item != null)
            {
                if (item == OFFFLG)
                {
                    retStr = ST_OFF;
                }
                else if(item == ONFLG)
                {
                    retStr = ST_ON;
                }
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したフラグから0：string.Empty、1：「中止」の文字列を返す
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrCancel(string item)
        {
            string retStr = string.Empty;
            if (item != null)
            {
                if (item != string.Empty)
                {
                    if (item == CANCELFLG)
                    {
                        retStr = ST_CANCEL;
                    }
                }
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したフラグから1：「今期」、2：「次期」の文字列を返す
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrSeason(string item)
        {
            string retStr = string.Empty;
            if (item != null)
            {
                if (item != string.Empty)
                {
                    if (item == SEASON_NOW)
                    {
                        retStr = ST_SEASON_NOW;
                    }
                    else if (item == SEASON_NEXT)
                    {
                        retStr = ST_SEASON_NEXT;
                    }
                }
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したフラグから1：「通常」、2:「オンライン」、1,2以外：stirng.Emptyの文字列を返す
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        //public string StrMeeting(string item)
        //{
        //    string retStr = string.Empty;
        //    if (item != null)
        //    {
        //        if (item != string.Empty)
        //        {
        //            if (item == MEETING_NORMAL)
        //            {
        //                retStr = ST_MEETING_NORMAL;
        //            }
        //            else if (item == MEETING_ONLINE)
        //            {
        //                retStr = ST_MEETING_ONLINE;
        //            }
        //        }
        //    }
        //    return retStr;
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したフラグから1：「オンライン」、1以外：stirng.Emptyの文字列を返す
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrOnline(string item)
        {
            string retStr = string.Empty;
            if (item != null)
            {
                if (item != string.Empty)
                {
                    if (item == ONLINEFLG)
                    {
                        retStr = ST_MEETING_ONLINE;
                    }
                }
            }
            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力した回答区分から文字列を返す。
        /// 出席・欠席以外は未回答
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrAnswer(string answer)
        {
            string retStr = LADef.ST_ANSWER_NO;
            if (answer != null)
            {
                if (answer.Equals(LADef.ANSWER_PRE))
                {
                    retStr = LADef.ST_ANSWER_PRE;
                }
                else if (answer.Equals(LADef.ANSWER_ABS))
                {
                    retStr = LADef.ST_ANSWER_ABS;
                }
                //else if (answer.Equals(LADef.ANSWER_NO))
                //{
                //    retStr = LADef.ST_ANSWER_NO;
                //}
            }

            return retStr;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力した回答区分から文字色及びstring.Emptyを返す。
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrAnswerColor(string answer)
        {
            string retStr = string.Empty;
            if (answer.Equals(LADef.ANSWER_PRE))
            {
                retStr = LADef.STRCOL_STRDEF;
            }
            else if (answer.Equals(LADef.ANSWER_ABS))
            {
                retStr = LADef.STRCOL_STRDEF;
            }
            else if (answer.Equals(LADef.ANSWER_NO))
            {
                retStr = LADef.STRCOL_RED;
            }
            return retStr;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 日付チェック
        /// </summary>
        /// <param name="answerDate">開催日</param>
        /// <param name="answerTime">開催時間</param>
        /// <param name="nowDt">現在日時</param>
        /// <returns>false=対象／true=対象外</returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public bool ChkDate(string eventDate, string eventTime, DateTime nowDt)
        {
            bool ret = false;        // 初期値は対象判定
            DateTime wkDt;

            // 時間指定なしの場合
            if (eventTime.Equals(string.Empty))
            {
                wkDt = DateTime.Parse(eventDate + " 00:00:00").AddDays(1);
                // 開催日+1日以降であれば対象外
                if (nowDt >= wkDt)
                {
                    ret = true; // 
                }
            }
            // 
            else
            {
                wkDt = DateTime.Parse(eventDate + " " + eventTime + ":00").AddMinutes(1.0);
                // 開催日時+1分以降であれば対象外
                if (nowDt >= wkDt)
                {
                    ret = true; // 期限切れ
                }
            }

            return ret;
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 回答期限チェック
        /// 回答期限が設定されていない場合は期限内
        /// </summary>
        /// <param name="eventClass">イベントクラス</param>
        /// <param name="answerDate">回答期限日</param>
        /// <param name="answerTime">回答期限時間</param>
        /// <param name="nowDt">現在日時</param>
        /// <returns>false=期限内／true=期限切れ</returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public bool ChkAnswerDate(string eventClass, string answerDate, string answerTime, DateTime nowDt)
        {
            bool ret = false;        // 初期値は期限内判定
            DateTime wkDt;

            // 回答期限の確認
            if (answerDate != string.Empty)
            {
                // 回答期限が設定されている場合

                // キャビネットイベント、もしくは理事・委員会の場合
                if (eventClass.Equals(EVENTCLASS_EV) ||
                    eventClass.Equals(EVENTCLASS_DI) ||
                   (eventClass.Equals(EVENTCLASS_ME) && answerTime.Equals(string.Empty)))
                {
                    wkDt = DateTime.Parse(answerDate + " 00:00:00").AddDays(1);
                    // 回答期限日+1日以降であれば期限切れ
                    if (nowDt >= wkDt)
                    {
                        ret = true; // 期限切れ
                    }
                }
                // 年間例会スケジュールの場合
                else if (eventClass.Equals(EVENTCLASS_ME))
                {
                    wkDt = DateTime.Parse(answerDate + " " + answerTime + ":00").AddMinutes(1.0);
                    // 回答期限日時+1分以降であれば期限切れ
                    if (nowDt >= wkDt)
                    {
                        ret = true; // 期限切れ
                    }
                }
            }

            return ret;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// オブジェクト関連

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルを新規に作成する(StyleClass)
        /// </summary>
        /// <param name="labelStr">Text</param>
        /// <param name="fontsize">FontSize</param>
        /// <param name="voption">VerticalOptions</param>
        /// <param name="styleclass">StyleClass</param>
        /// <param name="rowNum">Grid.Row</param>
        /// <param name="columnNum">Grid.Column</param>
        /// <param name="columnSpan">Grid.ColumnSpan</param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Label CreateLabel_Styleclass(string labelStr,
                                    NamedSize fontsize,
                                    LayoutOptions voption,
                                    string styleclass,
                                    int rowNum,
                                    int columnNum,
                                    int columnSpan)
        {
            Label label = new Label
            {
                Text = labelStr,
                FontSize = Device.GetNamedSize(fontsize, typeof(Label)),
                VerticalOptions = voption,
                StyleClass = new[] { styleclass }
            };
            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, columnNum);
            Grid.SetColumnSpan(label, columnSpan);
            return label;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ラベルを新規に作成する(Style)
        /// </summary>
        /// <param name="labelStr">Text</param>
        /// <param name="fontsize">FontSize</param>
        /// <param name="voption">VerticalOptions</param>
        /// <param name="styleclass">StyleClass</param>
        /// <param name="rowNum">Grid.Row</param>
        /// <param name="columnNum">Grid.Column</param>
        /// <param name="columnSpan">Grid.ColumnSpan</param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public Label CreateLabel_Style(string labelStr,
                                    NamedSize fontsize,
                                    LayoutOptions voption,
                                    string style,
                                    int rowNum,
                                    int columnNum,
                                    int columnSpan)
        {
            Label label = new Label
            {
                Text = labelStr,
                FontSize = Device.GetNamedSize(fontsize, typeof(Label)),
                VerticalOptions = voption,
                Style = Application.Current.Resources[style] as Style
            };
            Grid.SetRow(label, rowNum);
            Grid.SetColumn(label, columnNum);
            Grid.SetColumnSpan(label, columnSpan);
            return label;
        }

    }
}
