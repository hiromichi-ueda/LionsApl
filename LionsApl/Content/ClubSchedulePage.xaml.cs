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

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // ContentUtilクラス
        private ContentUtil _contUtl;

        // 年間例会スケジュールのデータNo.
        private int _DataNo;

        // 表示定数
        private readonly string CancelStr = "中止";
        private readonly string OnlineStr = "　※オンライン例会";

        public ClubSchedulePage(int dataNo)
        {
            InitializeComponent();

            // 一覧から取得
            _DataNo = dataNo;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _contUtl = new ContentUtil();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

            GetMeetingScheduleInfo();

        }

        private void GetMeetingScheduleInfo()
        {
            Label Remarks;
            Label OptionName1;
            Label OptionName2;
            Label OptionName3;
            Label OptionName4;
            Label OptionName5;
            Label OptionRadio1;
            Label OptionRadio2;
            Label OptionRadio3;
            Label OptionRadio4;
            Label OptionRadio5;

            string RemarksStr = string.Empty;
            string OptName1 = string.Empty;
            string OptName2 = string.Empty;
            string OptName3 = string.Empty;
            string OptName4 = string.Empty;
            string OptName5 = string.Empty;
            string OptRadio1 = string.Empty;
            string OptRadio2 = string.Empty;
            string OptRadio3 = string.Empty;
            string OptRadio4 = string.Empty;
            string OptRadio5 = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();

            // 会員情報取得
            try
            {
                foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                    "From T_MEETINGSCHEDULE " +
                                                                    "Where DataNo = '" + _DataNo + "'"))
                {
                    //DisplayAlert("List Tap", "MeetingDate:" + row.MeetingDate, "OK");

                    // 例会日
                    MeetingDate.Text = Util.GetString(row.MeetingDate).Substring(0, 10);

                    // 中止
                    if (Util.GetString(row.CancelFlg) == "1")
                    {
                        Cancel.Text = CancelStr;
                    }
                    Cancel.Text = CancelStr;

                    //DisplayAlert("List Tap", "MeetingTime:" + row.MeetingTime, "OK");
                    // 時間
                    MeetingTime.Text = Util.GetString(row.MeetingTime) + "～" + OnlineStr;

                    // オンライン
                    if (Util.GetString(row.Online) == "1")
                    {
                        MeetingTime.Text = MeetingTime.Text + OnlineStr;
                    }

                    // 例会名
                    MeetingName.Text = Util.GetString(row.MeetingName);

                    // 会場
                    MeetingPlace.Text = Util.GetString(row.MeetingPlace);

                    // 備考（項目名）
                    RemarksItems.Text = Util.GetString(row.RemarksItems);

                    // 備考（備考欄）
                    //Remarks.Text = Util.GetString(row.Remarks);
                    //RemarksStr = Util.GetString(row.Remarks);
                    RemarksStr = "テスト用文字列を設定しています。テスト用文字列を設定しています。テスト用文字列を設定しています。テスト用文字列を設定しています。";
                    //Remarks.Text = RemarksStr;
                    if (RemarksStr != string.Empty)
                    {
                        Remarks = _contUtl.LabelCreate(RemarksStr,
                                                       NamedSize.Default,
                                                       LayoutOptions.Center,
                                                       "page_member_hfree",
                                                       5, 1, 2);
                        //Remarks = new Label
                        //{
                        //    Text = RemarksStr,
                        //    FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                        //    VerticalOptions = LayoutOptions.Center,
                        //    StyleClass = new[] { "page_member_hfree" }
                        //};
                        //Grid.SetRow(Remarks, 5);
                        //Grid.SetColumn(Remarks, 1);
                        //Grid.SetColumnSpan(Remarks, 2);
                        DetailGrid.Children.Add(Remarks);
                    }

                    // --------------------------
                    // 例会オプション
                    // --------------------------

                    // Option1
                    // Opt1Name = _contUtl.GetString(row.OptionName1);
                    // OptRadio1 = _contUtl.GetString(row.OptionRadio1);
                    OptName1 = "テスト１";
                    OptRadio1 = "0";
                    if (OptName1 != string.Empty)
                    {
                        // GridのPadding設定
                        OptGrid.Padding = new Thickness(10);

                        // 例会オプション１（項目名）
                        OptionName1 = _contUtl.LabelCreate(OptName1,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "page_member",
                                                           0, 0, 1);
                        OptGrid.Children.Add(OptionName1);

                        // 例会オプション１（入力）
                        OptRadio1 = _contUtl.LabelOnOff(Util.GetString(OptRadio1));
                        OptionRadio1 = _contUtl.LabelCreate(OptRadio1,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "page_member",
                                                            0, 1, 1);
                        OptGrid.Children.Add(OptionRadio1);
                    }

                    // Option2
                    // OptName2 = _contUtl.GetString(row.OptionName2);
                    // OptRadio2 = _contUtl.GetString(row.OptionRadio2);
                    OptName2 = "テスト２";
                    OptRadio2 = "1";
                    if (OptName2 != string.Empty)
                    {
                        // 例会オプション２（項目名）
                        OptionName2 = _contUtl.LabelCreate(OptName2,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "page_member",
                                                           1, 0, 1);
                        OptGrid.Children.Add(OptionName2);

                        // 例会オプション２（入力）
                        OptRadio2 = _contUtl.LabelOnOff(Util.GetString(OptRadio2));
                        OptionRadio2 = _contUtl.LabelCreate(OptRadio2,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "page_member",
                                                            1, 1, 1);
                        OptGrid.Children.Add(OptionRadio2);
                    }

                    // Option3
                    // OptName3 = _contUtl.GetString(row.OptionName3);
                    // OptRadio3 = _contUtl.GetString(row.OptionRadio3);
                    OptName3 = "テスト３";
                    OptRadio3 = "0";
                    if (OptName3 != string.Empty)
                    {
                        // 例会オプション３（項目名）
                        OptionName3 = _contUtl.LabelCreate(OptName3,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "page_member",
                                                           2, 0, 1);
                        OptGrid.Children.Add(OptionName3);

                        // 例会オプション３（入力）
                        OptRadio3 = _contUtl.LabelOnOff(Util.GetString(OptRadio3));
                        OptionRadio3 = _contUtl.LabelCreate(OptRadio3,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "page_member",
                                                            2, 1, 1);
                        OptGrid.Children.Add(OptionRadio3);
                    }

                    // Option4
                    // OptName4 = _contUtl.GetString(row.OptionName4);
                    // OptRadio4 = _contUtl.GetString(row.OptionRadio4);
                    OptName4 = "テスト４";
                    OptRadio4 = "0";
                    if (OptName4 != string.Empty)
                    {
                        // 例会オプション４（項目名）
                        OptionName4 = _contUtl.LabelCreate(OptName4,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "page_member",
                                                           3, 0, 1);
                        OptGrid.Children.Add(OptionName4);

                        // 例会オプション４（入力）
                        OptRadio4 = _contUtl.LabelOnOff(Util.GetString(OptRadio4));
                        OptionRadio4 = _contUtl.LabelCreate(OptRadio4,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "page_member",
                                                            3, 1, 1);
                        OptGrid.Children.Add(OptionRadio4);
                    }

                    // Option5
                    // OptName5 = _contUtl.GetString(row.OptionName5);
                    // OptRadio5 = _contUtl.GetString(row.OptionRadio5);
                    OptName5 = "テスト５";
                    OptRadio5 = "0";
                    if (OptName5 != string.Empty)
                    {
                        // 例会オプション５（項目名）
                        OptionName5 = _contUtl.LabelCreate(OptName5,
                                                           NamedSize.Default,
                                                           LayoutOptions.Center,
                                                           "page_member",
                                                           4, 0, 1);
                        OptGrid.Children.Add(OptionName5);

                        // 例会オプション５（入力）
                        OptRadio5 = _contUtl.LabelOnOff(Util.GetString(OptRadio5));
                        OptionRadio5 = _contUtl.LabelCreate(OptRadio5,
                                                            NamedSize.Default,
                                                            LayoutOptions.Center,
                                                            "page_member",
                                                            4, 1, 1);
                        OptGrid.Children.Add(OptionRadio5);
                    }

                    // お酒
                    Sake.Text = _contUtl.LabelOnOff(Util.GetString(row.Sake));


                    // 本人以外の参加
                    OtherUser.Text = _contUtl.LabelOnOff(Util.GetString(row.OtherUser));

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }

        }

    }

}
