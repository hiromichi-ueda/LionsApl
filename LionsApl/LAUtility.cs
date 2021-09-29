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
        public string EVENTCLASS_CV = "1";
        public string EVENTCLASS_CL = "2";
        public string EVENTCLASS_DR = "3";
        // クラブイベントクラス
        public string CLUBEVENTCLASS_RI = "1";
        public string CLUBEVENTCLASS_IN = "2";
        // 中止フラグ
        public string CANCELFLG = "1";
        // 出欠フラグ
        public string ANSWER_PRE = "1";
        public string ANSWER_AB = "2";
        public string ANSWER_NO = string.Empty;
        // オンラインフラグ
        public string ONLINEFLG = "1";
        // 会議フラグ
        public string MEETING_NORMAL = "1";
        public string MEETING_ONLINE = "2";

        // 時期区分
        public string SEASON_NOW = "1";
        public string SEASON_NEXT = "2";

        // 引数用文字列
        public string NLC_OFF = "0";                        // 改行を削除しない。
        public string NLC_ON = "1";                         // 改行を削除する。

        // 各種判定用文字列
        public readonly string OFFFLG = "0";
        public readonly string ONFLG = "1";
        public readonly string NOFLG = string.Empty;

        // 出力文字列
        public readonly string ST_CANCEL = "中止";
        public readonly string ST_SEASON_NOW = "今期";
        public readonly string ST_SEASON_NEXT = "時期";
        public readonly string ST_ON = "有り";
        public readonly string ST_OFF = "無し";
        public readonly string ST_MEETING_NORMAL = "通常";
        public readonly string ST_MEETING_ONLINE = "オンライン";


        public LAUtility()
        {

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 入力したstringがnullかどうかをチェックして入力値かstring.Emptyを返す
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
        /// 入力したstringがnullかどうかをチェックして入力値かstring.Emptyを返す
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
        public string StrMeeting(string item)
        {
            string retStr = string.Empty;
            if (item != null)
            {
                if (item != string.Empty)
                {
                    if (item == MEETING_NORMAL)
                    {
                        retStr = ST_MEETING_NORMAL;
                    }
                    else if (item == MEETING_ONLINE)
                    {
                        retStr = ST_MEETING_ONLINE;
                    }
                }
            }
            return retStr;
        }


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
