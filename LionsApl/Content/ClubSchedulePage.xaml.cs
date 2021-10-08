using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.StyleSheets;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubSchedulePage : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private int _dataNo;             // データNo.

        // 表示定数
        private readonly string CancelStr = "中止";
        private readonly string OnlineStr = "　※オンライン例会";


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo">データNo</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubSchedulePage(int dataNo)
        {
            InitializeComponent();

            // 一覧から取得
            _dataNo = dataNo;

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 年間例会スケジュール情報を取得する
            GetMeetingScheduleInfo();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 年間例会スケジュール情報を取得する
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMeetingScheduleInfo()
        {
            Label Remarks;
            Label OptionName1;
            Label OptionRadio1;
            Label OptionName2;
            Label OptionRadio2;
            Label OptionName3;
            Label OptionRadio3;
            Label OptionName4;
            Label OptionRadio4;
            Label OptionName5;
            Label OptionRadio5;
            Label SakeTitle;
            Label SakeRadio;
            Label OtherTitle;
            Label OtherRadio;

            string RemarksStr = string.Empty;
            string OptName1 = string.Empty;
            string OptRadio1 = string.Empty;
            string OptName2 = string.Empty;
            string OptRadio2 = string.Empty;
            string OptName3 = string.Empty;
            string OptRadio3 = string.Empty;
            string OptName4 = string.Empty;
            string OptRadio4 = string.Empty;
            string OptName5 = string.Empty;
            string OptRadio5 = string.Empty;
            string SakeStr = "お酒";
            string SakeVal = string.Empty;
            string OtherStr = "本人以外の参加";
            string OtherVal = string.Empty;

            string WrkRIStr = string.Empty;
            string WrkRCStr = string.Empty;
            string WrkRStr = string.Empty;

            int rowCount = 0;

            //Table.TableUtil Util = new Table.TableUtil();

            // 会員情報取得
            try
            {
                foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                    "From T_MEETINGSCHEDULE " +
                                                                    "Where DataNo = '" + _dataNo + "'"))
                {

                    // 例会日
                    MeetingDate.Text = _utl.GetString(row.MeetingDate).Substring(0, 10);

                    // 中止
                    if (_utl.GetString(row.CancelFlg) == "1")
                    {
                        Cancel.Text = CancelStr;
                    }

                    // 時間
                    MeetingTime.Text = _utl.GetString(row.MeetingTime) + "～";

                    // オンライン
                    if (_utl.GetString(row.Online) == "1")
                    {
                        MeetingTime.Text = MeetingTime.Text + OnlineStr;
                    }

                    // 例会名
                    MeetingName.Text = _utl.GetString(row.MeetingName);

                    // 会場
                    MeetingPlace.Text = _utl.GetString(row.MeetingPlace);

                    // 備考項目名（例会備考1～10）
                    WrkRIStr = _utl.GetString(row.RemarksItems);
                    // 備考（例会備考1～10）
                    WrkRCStr = _utl.GetString(row.RemarksCheck);

                    // 備考項目名（例会備考1～10）と備考（例会備考1～10）が両方存在する場合
                    if (!WrkRIStr.Equals(string.Empty) &&
                        !WrkRCStr.Equals(string.Empty))
                    {
                        string[] WrkRIStrArr = WrkRIStr.TrimEnd(',').Split(',');
                        string[] WrkRCStrArr = WrkRCStr.TrimEnd(',').Split(',');

                        // 備考作成
                        int idx = 0;
                        foreach (string ws in WrkRCStrArr)
                        {
                            idx = int.Parse(ws) - 1;
                            WrkRStr += WrkRIStrArr[idx] + '、';
                        }
                        RemarksItems.Text = WrkRStr.TrimEnd('、');
                    }

                    // 備考欄
                    RemarksStr = _utl.GetString(row.Remarks);
                    if (!RemarksStr.Equals(string.Empty))
                    {
                        Remarks = _utl.CreateLabel_Style(RemarksStr,
                                                       NamedSize.Default,
                                                       LayoutOptions.Center,
                                                       "Page_HeightFree",
                                                       5, 1, 2);
                        DetailGrid.Children.Add(Remarks);
                    }

                    // --------------------------
                    // 例会オプション
                    // --------------------------

                    // Option1
                    // 項目名取得
                    OptName1 = _utl.GetString(row.OptionName1);
                    // 項目値取得
                    OptRadio1 = _utl.GetString(row.OptionRadio1);
                    // 項目有、及び項目値=有りの場合のみ表示する
                    if ((OptName1 != string.Empty) && (OptRadio1 == "1"))
                    {
                        // GridのPadding設定
                        OptGrid.Padding = new Thickness(10.0, 10.0, 10.0, 0.0);

                        // 例会オプション１（項目名）
                        OptionName1 = _utl.CreateLabel_Style(OptName1,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "Page_Base",
                                                           rowCount, 0, 1);
                        OptGrid.Children.Add(OptionName1);

                        // 例会オプション１（入力）
                        OptRadio1 = _utl.StrOnOff(OptRadio1);
                        OptionRadio1 = _utl.CreateLabel_Style(OptRadio1,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "Page_Base",
                                                            rowCount, 1, 1);
                        OptGrid.Children.Add(OptionRadio1);

                        // 出力行カウント
                        rowCount++;
                    }

                    // Option2
                    // 項目名取得
                    OptName2 = _utl.GetString(row.OptionName2);
                    // 項目値取得
                    OptRadio2 = _utl.GetString(row.OptionRadio2);
                    // 項目有、及び項目値=有りの場合のみ表示する
                    if ((OptName2 != string.Empty) && (OptRadio2 == "1"))
                    {
                        // 例会オプション２（項目名）
                        OptionName2 = _utl.CreateLabel_Style(OptName2,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "Page_Base",
                                                           rowCount, 0, 1);
                        OptGrid.Children.Add(OptionName2);

                        // 例会オプション２（入力）
                        OptRadio2 = _utl.StrOnOff(OptRadio2);
                        OptionRadio2 = _utl.CreateLabel_Style(OptRadio2,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "Page_Base",
                                                            rowCount, 1, 1);
                        OptGrid.Children.Add(OptionRadio2);

                        // 出力行カウント
                        rowCount++;
                    }

                    // Option3
                    // 項目名取得
                    OptName3 = _utl.GetString(row.OptionName3);
                    // 項目値取得
                    OptRadio3 = _utl.GetString(row.OptionRadio3);
                    // 項目有、及び項目値=有りの場合のみ表示する
                    if ((OptName3 != string.Empty) && (OptRadio3 == "1"))
                    {
                        // 例会オプション３（項目名）
                        OptionName3 = _utl.CreateLabel_Style(OptName3,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "Page_Base",
                                                           rowCount, 0, 1);
                        OptGrid.Children.Add(OptionName3);

                        // 例会オプション３（入力）
                        OptRadio3 = _utl.StrOnOff(OptRadio3);
                        OptionRadio3 = _utl.CreateLabel_Style(OptRadio3,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "Page_Base",
                                                            rowCount, 1, 1);
                        OptGrid.Children.Add(OptionRadio3);

                        // 出力行カウント
                        rowCount++;
                    }

                    // Option4
                    // 項目名取得
                    OptName4 = _utl.GetString(row.OptionName4);
                    // 項目値取得
                    OptRadio4 = _utl.GetString(row.OptionRadio4);
                    // 項目有、及び項目値=有りの場合
                    if ((OptName4 != string.Empty) && (OptRadio4 == "1"))
                    {
                        // 例会オプション４（項目名）
                        OptionName4 = _utl.CreateLabel_Style(OptName4,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "Page_Base",
                                                           rowCount, 0, 1);
                        OptGrid.Children.Add(OptionName4);

                        // 例会オプション４（入力）
                        OptRadio4 = _utl.StrOnOff(OptRadio4);
                        OptionRadio4 = _utl.CreateLabel_Style(OptRadio4,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "Page_Base",
                                                            rowCount, 1, 1);
                        OptGrid.Children.Add(OptionRadio4);

                        // 出力行カウント
                        rowCount++;
                    }

                    // Option5
                    // 項目名取得
                    OptName5 = _utl.GetString(row.OptionName5);
                    // 項目値取得
                    OptRadio5 = _utl.GetString(row.OptionRadio5);
                    // 項目有、及び項目値=有りの場合のみ表示する
                    if ((OptName5 != string.Empty) && (OptRadio5 == "1"))
                    {
                        // 例会オプション５（項目名）
                        OptionName5 = _utl.CreateLabel_Style(OptName5,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "Page_Base",
                                                           rowCount, 0, 1);
                        OptGrid.Children.Add(OptionName5);

                        // 例会オプション５（入力）
                        OptRadio5 = _utl.StrOnOff(OptRadio5);
                        OptionRadio5 = _utl.CreateLabel_Style(OptRadio5,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "Page_Base",
                                                            rowCount, 1, 1);
                        OptGrid.Children.Add(OptionRadio5);

                        // 出力行カウント
                        rowCount++;
                    }

                    // お酒
                    // 項目名
                    SakeTitle = _utl.CreateLabel_Style(SakeStr,
                                                     NamedSize.Default,
                                                     LayoutOptions.Center,
                                                     "Page_Base",
                                                     rowCount, 0, 1);
                    OptGrid.Children.Add(SakeTitle);

                    // 項目値
                    SakeVal = _utl.StrOnOff(row.Sake);
                    SakeRadio = _utl.CreateLabel_Style(SakeVal,
                                                     NamedSize.Default,
                                                     LayoutOptions.Center,
                                                     "Page_Base",
                                                     rowCount, 1, 1);
                    OptGrid.Children.Add(SakeRadio);

                    // 出力行カウント
                    rowCount++;

                    // 本人以外の参加
                    // 項目名
                    OtherTitle = _utl.CreateLabel_Style(OtherStr,
                                                      NamedSize.Default,
                                                      LayoutOptions.Center,
                                                      "Page_Base",
                                                      rowCount, 0, 1);
                    OptGrid.Children.Add(OtherTitle);

                    // 項目値
                    OtherVal = _utl.StrOnOff(row.OtherUser);
                    OtherRadio = _utl.CreateLabel_Style(OtherVal,
                                                      NamedSize.Default,
                                                      LayoutOptions.Center,
                                                      "Page_Base",
                                                      rowCount, 1, 1);
                    OptGrid.Children.Add(OtherRadio);

                    // 出力行カウント
                    rowCount++;

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGSCHEDULE) : {ex.Message}", "OK");
            }

        }

    }

}
