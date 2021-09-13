using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMeetingProgramPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // ContentUtilクラス
        private ContentUtil _contUtl;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                              //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                            //PdfViewer
        public static String FilePath_MeetingProgram = ((App)Application.Current).FilePath_MeetingProgram;  //例会プログラム(CLUB)

        // 対象データNo.
        private int _DataNo;
        private int _ScheduleDataNo;



        // 表示定数
        private readonly string OnlineStr = "オンライン";


        public ClubMeetingProgramPage(int dataNo)
        {
            InitializeComponent();

            // font-size
            this.lbl_MeetingDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MeetingName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingName.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Meeting.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Meeting.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MeetingUrl.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingUrl.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MeetingID.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingID.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MeetingPW.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingPW.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_MeetingOther.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingOther.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 対象データNo.設定
            _DataNo = dataNo;

            // Content Utilクラス生成
            _contUtl = new ContentUtil();

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

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_MeetingProgram);

            // 例会プログラム情報設定
            GetMeetingProgram();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会プログラム情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMeetingProgram()
        {

            // 変数宣言
            int wkDataNo = 0;
            string wkMeetingOther = string.Empty;
            string wkMeeting = string.Empty;
            string wkClubCode = string.Empty;
            string wkFileName = string.Empty;
            string wkFileName1 = string.Empty;
            string wkFileName2 = string.Empty;
            string wkFileName3 = string.Empty;
            string wkFileName4 = string.Empty;
            string wkFileName5 = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();


            // 例会プログラム情報取得
            try
            {



                foreach (Table.CLUB_MPROG row in _sqlite.Get_CLUB_MPROG(
                                                                "SELECT " +
                                                                    "t1.DataNo, " +
                                                                    "t1.ScheduleDataNo, " +
                                                                    "t1.ClubCode, " +
                                                                    "t1.ClubNameShort, " +
                                                                    "t1.Fiscal, " +
                                                                    "t1.FileName, " +
                                                                    "t1.FileName1, " +
                                                                    "t1.FileName2, " +
                                                                    "t1.FileName3, " +
                                                                    "t1.FileName4, " +
                                                                    "t1.FileName5, " +
                                                                    "t1.Meeting, " +
                                                                    "t2.MeetingDate, " +
                                                                    "t2.MeetingTime, " +
                                                                    "t2.MeetingPlace, " +
                                                                    "t2.MeetingName " +
                                                                "FROM " +
                                                                    "T_MEETINGPROGRAM t1 " +
                                                                "LEFT OUTER JOIN " +
                                                                    "T_MEETINGSCHEDULE t2 " +
                                                                "ON " +
                                                                    "t1.ScheduleDataNo = t2.DataNo " +
                                                                "WHERE " +
                                                                    "t1.DataNo = '" + _DataNo + "' " +
                                                                "ORDER BY t2.MeetingDate DESC"))
                {
                    wkDataNo = row.DataNo;

                    // 例会日
                    MeetingDate.Text = Util.GetString(row.MeetingDate).Substring(0, 10);

                    // 例会名
                    MeetingName.Text = Util.GetString(row.MeetingName);

                    wkMeeting = Util.GetString(row.Meeting);
                    wkMeeting = "1"; //TEST

                    // 例会方法がオンラインの場合
                    if (wkMeeting == "2")
                    {
                        // 例会方法
                        Meeting.Text = OnlineStr;

                        // URL
                        MeetingUrl.Text = Util.GetString(row.MeetingUrl);

                        // ID
                        MeetingID.Text = Util.GetString(row.MeetingID);

                        // PW
                        MeetingPW.Text = Util.GetString(row.MeetingPW);

                        // 備考
                        wkMeetingOther = Util.GetString(row.MeetingOther);

                        //wkMeetingOther = "テスト用文字列を設定しています。テスト用文字列を設定しています。テスト用文字列を設定しています。テスト用文字列を設定しています。";
                        MeetingOther.Text = wkMeetingOther;
                    }
                    // 例会方法が通常の場合
                    else
                    {
                        GRDef_Meeting.Height = 0;
                        GRDef_MeetingUrl.Height = 0;
                        GRDef_MeetingIDPW.Height = 0;
                        GRDef_MeetingOther.Height = 0;
                    }

                    wkClubCode = Util.GetString(row.ClubCode);
                    wkFileName = Util.GetString(row.FileName);
                    wkFileName1 = Util.GetString(row.FileName1);
                    wkFileName2 = Util.GetString(row.FileName2);
                    wkFileName3 = Util.GetString(row.FileName3);
                    wkFileName4 = Util.GetString(row.FileName4);
                    wkFileName5 = Util.GetString(row.FileName5);

                    // ファイルが1つでもある場合
                    if (wkFileName != string.Empty ||
                        wkFileName1 != string.Empty ||
                        wkFileName2 != string.Empty ||
                        wkFileName3 != string.Empty ||
                        wkFileName4 != string.Empty ||
                        wkFileName5 != string.Empty)
                    {

                        if (wkFileName != string.Empty)
                        {
                            // WebViewにファイルのURLを設定する
                            lbl_FileName.Text = SetFileUrl(wkDataNo, wkClubCode, wkFileName, ref FileName);
                            //this.lbl_FileName.HeightRequest = 0;    //非表示設定
                        }
                        if (wkFileName1 != string.Empty)
                        {
                            // WebViewにファイルのURLを設定する
                            lbl_FileName1.Text = SetFileUrl(wkDataNo, wkClubCode, wkFileName1, ref FileName1);
                            //this.lbl_FileName1.HeightRequest = 0;    //非表示設定
                        }

                        if (wkFileName2 != string.Empty)
                        {
                            // WebViewにファイルのURLを設定する
                            lbl_FileName2.Text = SetFileUrl(wkDataNo, wkClubCode, wkFileName2, ref FileName2);
                            //this.lbl_FileName2.HeightRequest = 0;    //非表示設定
                        }

                        if (wkFileName3 != string.Empty)
                        {
                            // WebViewにファイルのURLを設定する
                            lbl_FileName3.Text = SetFileUrl(wkDataNo, wkClubCode, wkFileName3, ref FileName3);
                            //this.lbl_FileName3.HeightRequest = 0;    //非表示設定
                        }

                        if (wkFileName4 != string.Empty)
                        {
                            // WebViewにファイルのURLを設定する
                            lbl_FileName4.Text = SetFileUrl(wkDataNo, wkClubCode, wkFileName4, ref FileName4);
                            //this.lbl_FileName4.HeightRequest = 0;    //非表示設定
                        }

                        if (wkFileName5 != string.Empty)
                        {
                            // WebViewにファイルのURLを設定する
                            lbl_FileName5.Text = SetFileUrl(wkDataNo, wkClubCode, wkFileName5, ref FileName5);
                            //this.lbl_FileName5.HeightRequest = 0;    //非表示設定
                        }
                    }

                    //if (wkMeetingOther != string.Empty)
                    //{
                    //    wkMeetingOther = _contUtl.CreateLabel_Style(wkMeetingOther,
                    //                                   NamedSize.Default,
                    //                                   LayoutOptions.Center,
                    //                                   "Page_HeightFree",
                    //                                   5, 1, 2);
                    //    DetailGrid.Children.Add(Remarks);
                    //}

                    // --------------------------
                    // 添付ファイル
                    // --------------------------

                    //// Option1
                    //// 項目名取得
                    //OptName1 = _contUtl.GetString(row.OptionName1);
                    //// 項目値取得
                    //OptRadio1 = _contUtl.GetString(row.OptionRadio1);
                    //// 項目有、及び項目値=有りの場合のみ表示する
                    //if ((OptName1 != string.Empty) && (OptRadio1 == "1"))
                    //{
                    //    // GridのPadding設定
                    //    OptGrid.Padding = new Thickness(10.0, 10.0, 10.0, 0.0);

                    //    // 例会オプション１（項目名）
                    //    OptionName1 = _contUtl.CreateLabel_Styleclass(OptName1,
                    //                                       NamedSize.Default,
                    //                                       LayoutOptions.Center,
                    //                                       "page_member",
                    //                                       rowCount, 0, 1);
                    //    OptGrid.Children.Add(OptionName1);

                    //    // 例会オプション１（入力）
                    //    OptRadio1 = _contUtl.StrOnOff(Util.GetString(OptRadio1));
                    //    OptionRadio1 = _contUtl.CreateLabel_Styleclass(OptRadio1,
                    //                                        NamedSize.Default,
                    //                                        LayoutOptions.Center,
                    //                                        "page_member",
                    //                                        rowCount, 1, 1);
                    //    OptGrid.Children.Add(OptionRadio1);

                    //    // 出力行カウント
                    //    rowCount++;
                    //}

                    //// Option2
                    //// 項目名取得
                    //OptName2 = _contUtl.GetString(row.OptionName2);
                    //// 項目値取得
                    //OptRadio2 = _contUtl.GetString(row.OptionRadio2);
                    //// 項目有、及び項目値=有りの場合のみ表示する
                    //if ((OptName2 != string.Empty) && (OptRadio2 == "1"))
                    //{
                    //    // 例会オプション２（項目名）
                    //    OptionName2 = _contUtl.CreateLabel_Styleclass(OptName2,
                    //                                       NamedSize.Default,
                    //                                       LayoutOptions.Center,
                    //                                       "page_member",
                    //                                       rowCount, 0, 1);
                    //    OptGrid.Children.Add(OptionName2);

                    //    // 例会オプション２（入力）
                    //    OptRadio2 = _contUtl.StrOnOff(Util.GetString(OptRadio2));
                    //    OptionRadio2 = _contUtl.CreateLabel_Styleclass(OptRadio2,
                    //                                        NamedSize.Default,
                    //                                        LayoutOptions.Center,
                    //                                        "page_member",
                    //                                        rowCount, 1, 1);
                    //    OptGrid.Children.Add(OptionRadio2);

                    //    // 出力行カウント
                    //    rowCount++;
                    //}

                    //// Option3
                    //// 項目名取得
                    //OptName3 = _contUtl.GetString(row.OptionName3);
                    //// 項目値取得
                    //OptRadio3 = _contUtl.GetString(row.OptionRadio3);
                    //// 項目有、及び項目値=有りの場合のみ表示する
                    //if ((OptName3 != string.Empty) && (OptRadio3 == "1"))
                    //{
                    //    // 例会オプション３（項目名）
                    //    OptionName3 = _contUtl.CreateLabel_Styleclass(OptName3,
                    //                                       NamedSize.Default,
                    //                                       LayoutOptions.Center,
                    //                                       "page_member",
                    //                                       rowCount, 0, 1);
                    //    OptGrid.Children.Add(OptionName3);

                    //    // 例会オプション３（入力）
                    //    OptRadio3 = _contUtl.StrOnOff(Util.GetString(OptRadio3));
                    //    OptionRadio3 = _contUtl.CreateLabel_Styleclass(OptRadio3,
                    //                                        NamedSize.Default,
                    //                                        LayoutOptions.Center,
                    //                                        "page_member",
                    //                                        rowCount, 1, 1);
                    //    OptGrid.Children.Add(OptionRadio3);

                    //    // 出力行カウント
                    //    rowCount++;
                    //}

                    //// Option4
                    //// 項目名取得
                    //OptName4 = _contUtl.GetString(row.OptionName4);
                    //// 項目値取得
                    //OptRadio4 = _contUtl.GetString(row.OptionRadio4);
                    //// 項目有、及び項目値=有りの場合
                    //if ((OptName4 != string.Empty) && (OptRadio4 == "1"))
                    //{
                    //    // 例会オプション４（項目名）
                    //    OptionName4 = _contUtl.CreateLabel_Styleclass(OptName4,
                    //                                       NamedSize.Default,
                    //                                       LayoutOptions.Center,
                    //                                       "page_member",
                    //                                       rowCount, 0, 1);
                    //    OptGrid.Children.Add(OptionName4);

                    //    // 例会オプション４（入力）
                    //    OptRadio4 = _contUtl.StrOnOff(Util.GetString(OptRadio4));
                    //    OptionRadio4 = _contUtl.CreateLabel_Styleclass(OptRadio4,
                    //                                        NamedSize.Default,
                    //                                        LayoutOptions.Center,
                    //                                        "page_member",
                    //                                        rowCount, 1, 1);
                    //    OptGrid.Children.Add(OptionRadio4);

                    //    // 出力行カウント
                    //    rowCount++;
                    //}

                    //// Option5
                    //// 項目名取得
                    //OptName5 = _contUtl.GetString(row.OptionName5);
                    //// 項目値取得
                    //OptRadio5 = _contUtl.GetString(row.OptionRadio5);
                    //// 項目有、及び項目値=有りの場合のみ表示する
                    //if ((OptName5 != string.Empty) && (OptRadio5 == "1"))
                    //{
                    //    // 例会オプション５（項目名）
                    //    OptionName5 = _contUtl.CreateLabel_Styleclass(OptName5,
                    //                                       NamedSize.Default,
                    //                                       LayoutOptions.Center,
                    //                                       "page_member",
                    //                                       rowCount, 0, 1);
                    //    OptGrid.Children.Add(OptionName5);

                    //    // 例会オプション５（入力）
                    //    OptRadio5 = _contUtl.StrOnOff(Util.GetString(OptRadio5));
                    //    OptionRadio5 = _contUtl.CreateLabel_Styleclass(OptRadio5,
                    //                                        NamedSize.Default,
                    //                                        LayoutOptions.Center,
                    //                                        "page_member",
                    //                                        rowCount, 1, 1);
                    //    OptGrid.Children.Add(OptionRadio5);

                    //    // 出力行カウント
                    //    rowCount++;
                    //}

                    //// お酒
                    //// 項目名
                    //SakeTitle = _contUtl.CreateLabel_Styleclass(SakeStr,
                    //                                 NamedSize.Default,
                    //                                 LayoutOptions.Center,
                    //                                 "page_member",
                    //                                 rowCount, 0, 1);
                    //OptGrid.Children.Add(SakeTitle);

                    //// 項目値
                    //SakeVal = _contUtl.StrOnOff(Util.GetString(row.Sake));
                    //SakeRadio = _contUtl.CreateLabel_Styleclass(SakeVal,
                    //                                 NamedSize.Default,
                    //                                 LayoutOptions.Center,
                    //                                 "page_member",
                    //                                 rowCount, 1, 1);
                    //OptGrid.Children.Add(SakeRadio);

                    //// 出力行カウント
                    //rowCount++;

                    //// 本人以外の参加
                    //// 項目名
                    //OtherTitle = _contUtl.CreateLabel_Styleclass(OtherStr,
                    //                                  NamedSize.Default,
                    //                                  LayoutOptions.Center,
                    //                                  "page_member",
                    //                                  rowCount, 0, 1);
                    //OptGrid.Children.Add(OtherTitle);

                    //// 項目値
                    //OtherVal = _contUtl.StrOnOff(Util.GetString(row.OtherUser));
                    //OtherRadio = _contUtl.CreateLabel_Styleclass(OtherVal,
                    //                                  NamedSize.Default,
                    //                                  LayoutOptions.Center,
                    //                                  "page_member",
                    //                                  rowCount, 1, 1);
                    //OptGrid.Children.Add(OtherRadio);

                    //// 出力行カウント
                    //rowCount++;

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }

        }


        /// <summary>
        /// WebViewにiOS/Android別のURLを設定する。
        /// </summary>
        /// <param name="dataNo"></param>
        /// <param name="clubCode"></param>
        /// <param name="fileName"></param>
        /// <param name="webView"></param>
        /// <returns></returns>
        private string SetFileUrl(int dataNo, string clubCode, string fileName, ref WebView webView)
        {

            // FILEPATH取得
            var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");

            // FILEPATH生成([ClubCode]変換)
            var fileUrl = AppServer + filepath.Replace("[ClubCode]", clubCode).Replace("\\", "/").Replace("\r\n", "") +
                         "/" + dataNo.ToString() + "/" + fileName;

            // AndroidPDF Viewer
            var googleUrl = AndroidPdf + "?embedded=true&url=";

            // iOSの場合
            if (Device.RuntimePlatform == Device.iOS)
            {
                webView.Source = fileUrl;
            }
            // Androidの場合
            else if (Device.RuntimePlatform == Device.Android)
            {
                webView.Source = new UrlWebViewSource() { Url = googleUrl + fileUrl };
            }
            webView.HeightRequest = 800.0;

            return fileUrl;
        }
    }
}