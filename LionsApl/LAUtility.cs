using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LionsApl
{
    internal class LAUtility
    {
        public string NLC_OFF = "0";                        // 改行を削除しない。
        public string NLC_ON = "1";                         // 改行を削除する。
        private readonly string OffVal = "0";
        private readonly string OnStr = "有り";
        private readonly string OffStr = "無し";

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
        /// 入力したフラグから0：「無し」、1：「有り」の文字列を返す
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public string StrOnOff(string item)
        {
            string retStr = string.Empty;
            if (item != string.Empty)
            {
                if (item == OffVal)
                {
                    retStr = OffStr;
                }
                else
                {
                    retStr = OnStr;
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
