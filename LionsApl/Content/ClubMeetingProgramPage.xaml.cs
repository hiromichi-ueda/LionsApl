using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：例会プログラムページクラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMeetingProgramPage : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private int _dataNo;         // データNo.

        // Config取得
        public static string AppServer = ((App)Application.Current).AppServer;                              //Url
        public static string AndroidPdf = ((App)Application.Current).AndroidPdf;                            //PdfViewer
        public static string FilePath_MeetingProgram = ((App)Application.Current).FilePath_MeetingProgram;  //例会プログラム(CLUB)

        // 表示定数
        private readonly string OnlineStr = "オンライン";


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo">DataNo</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubMeetingProgramPage(int dataNo)
        {
            InitializeComponent();

            // font-size
            this.lbl_MeetingDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.MeetingDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Cancel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
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
            _dataNo = dataNo;

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

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
                                                                    "t1.MeetingUrl, " +
                                                                    "t1.MeetingID, " +
                                                                    "t1.MeetingPW, " +
                                                                    "t1.MeetingOther, " +
                                                                    "t2.MeetingDate, " +
                                                                    "t2.MeetingTime, " +
                                                                    "t2.MeetingPlace, " +
                                                                    "t2.MeetingName, " +
                                                                    "t2.CancelFlg " +
                                                                "FROM " +
                                                                    "T_MEETINGPROGRAM t1 " +
                                                                "LEFT OUTER JOIN " +
                                                                    "T_MEETINGSCHEDULE t2 " +
                                                                "ON " +
                                                                    "t1.ScheduleDataNo = t2.DataNo " +
                                                                "WHERE " +
                                                                    "t1.DataNo = '" + _dataNo + "' " +
                                                                "ORDER BY t2.MeetingDate DESC"))
                {
                    wkDataNo = row.DataNo;

                    // 例会日
                    MeetingDate.Text = _utl.GetDateString(row.MeetingDate);

                    // 中止
                    Cancel.Text = _utl.StrCancel(row.CancelFlg);

                    // 例会名
                    MeetingName.Text = _utl.GetString(row.MeetingName);

                    wkMeeting = _utl.GetString(row.Meeting);

                    // 例会方法がオンラインの場合
                    if (wkMeeting == "2")
                    {
                        // 例会方法
                        Meeting.Text = OnlineStr;

                        // URL
                        MeetingUrl.Text = _utl.GetString(row.MeetingUrl);

                        // ID
                        MeetingID.Text = _utl.GetString(row.MeetingID);

                        // PW
                        MeetingPW.Text = _utl.GetString(row.MeetingPW);

                        // 備考
                        MeetingOther.Text = _utl.GetString(row.MeetingOther, _utl.NLC_ON);
                    }
                    // 例会方法が通常の場合
                    else
                    {
                        lbl_Meeting.IsVisible = false;
                        Meeting.IsVisible = false;
                        lbl_MeetingUrl.IsVisible = false;
                        MeetingUrl.IsVisible = false;
                        lbl_MeetingID.IsVisible = false;
                        MeetingID.IsVisible = false;
                        lbl_MeetingPW.IsVisible = false;
                        MeetingPW.IsVisible = false;
                        lbl_MeetingOther.IsVisible = false;
                        MeetingOther.IsVisible = false;
                        // Meeting以下の項目の高さを0にする。
                        //GRDef_Meeting.Height = 0;
                        //GRDef_MeetingUrl.Height = 0;
                        //GRDef_MeetingIDPW.Height = 0;
                        //GRDef_MeetingOther.Height = 0;
                    }

                    wkClubCode = _utl.GetString(row.ClubCode);
                    wkFileName = _utl.GetString(row.FileName);
                    wkFileName1 = _utl.GetString(row.FileName1);
                    wkFileName2 = _utl.GetString(row.FileName2);
                    wkFileName3 = _utl.GetString(row.FileName3);
                    wkFileName4 = _utl.GetString(row.FileName4);
                    wkFileName5 = _utl.GetString(row.FileName5);

                    // ファイルが1つでもある場合
                    if (wkFileName != string.Empty ||
                        wkFileName1 != string.Empty ||
                        wkFileName2 != string.Empty ||
                        wkFileName3 != string.Empty ||
                        wkFileName4 != string.Empty ||
                        wkFileName5 != string.Empty)
                    {
                        // 添付ファイルの表示情報を設定する
                        SetAttachFileInfo(wkDataNo, wkClubCode, wkFileName, ref FileName, ref lbl_FileName, ref stack);

                        // 添付ファイル1の表示情報を設定する
                        SetAttachFileInfo(wkDataNo, wkClubCode, wkFileName1, ref FileName1, ref lbl_FileName1, ref stack1);

                        // 添付ファイル2の表示情報を設定する
                        SetAttachFileInfo(wkDataNo, wkClubCode, wkFileName2, ref FileName2, ref lbl_FileName2, ref stack2);

                        // 添付ファイル3の表示情報を設定する
                        SetAttachFileInfo(wkDataNo, wkClubCode, wkFileName3, ref FileName3, ref lbl_FileName3, ref stack3);

                        // 添付ファイル4の表示情報を設定する
                        SetAttachFileInfo(wkDataNo, wkClubCode, wkFileName4, ref FileName4, ref lbl_FileName4, ref stack4);

                        // 添付ファイル5の表示情報を設定する
                        SetAttachFileInfo(wkDataNo, wkClubCode, wkFileName5, ref FileName5, ref lbl_FileName5, ref stack5);

                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添付ファイルの表示情報を設定する
        /// </summary>
        /// <param name="dataNo">DataNo</param>
        /// <param name="clubCode">ClubCode</param>
        /// <param name="fileName">FileName</param>
        /// <param name="webView">WebView</param>
        /// <param name="label">Label</param>
        private void SetAttachFileInfo(int dataNo, string clubCode, string fileName, ref WebView webView, ref Label label, ref StackLayout stackLayout)
        {
            if (fileName != string.Empty)
            {
                // WebViewにファイルのURLを設定する（＆テスト表示用URLラベル設定）
                label.Text = SetFileUrl(dataNo, clubCode, fileName, ref webView);
                // URLラベルは非表示にする
                label.IsVisible = false;
            }
            else
            {
                //非表示設定
                label.IsVisible = false;
                stackLayout.IsVisible = false;
                webView.IsVisible = false;
                //webView.HeightRequest = 0.0;    //非表示設定
                //rowDef.Height = 0.0;            //非表示設定
            }
            //label.HeightRequest = 0.0;    //非表示設定
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WebViewにiOS/Android別のURLを設定する。
        /// </summary>
        /// <param name="dataNo">DataNo</param>
        /// <param name="clubCode">ClubCode</param>
        /// <param name="fileName">FileName</param>
        /// <param name="webView">WebViwe</param>
        /// <returns></returns>
        ///////////////////////////////////////////////////////////////////////////////////////////
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
            webView.HeightRequest = 600.0;

            return fileUrl;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ナビゲーション開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void WebviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (ReferenceEquals(sender, FileName))
            {
                stack.IsVisible = true;
            }
            else if (ReferenceEquals(sender, FileName1))
            {
                stack1.IsVisible = true;
            }
            else if (ReferenceEquals(sender, FileName2))
            {
                stack2.IsVisible = true;
            }
            else if (ReferenceEquals(sender, FileName3))
            {
                stack3.IsVisible = true;
            }
            else if (ReferenceEquals(sender, FileName4))
            {
                stack4.IsVisible = true;
            }
            else if (ReferenceEquals(sender, FileName5))
            {
                stack5.IsVisible = true;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ナビゲーション終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void WebviewNavigated(object sender, WebNavigatedEventArgs e)
        {
            if (ReferenceEquals(sender, FileName))
            {
                stack.IsVisible = false;
                lbl_FileName.IsVisible = false;
            }
            else if (ReferenceEquals(sender, FileName1))
            {
                stack1.IsVisible = false;
                lbl_FileName1.IsVisible = false;
            }
            else if (ReferenceEquals(sender, FileName2))
            {
                stack2.IsVisible = false;
                lbl_FileName2.IsVisible = false;
            }
            else if (ReferenceEquals(sender, FileName3))
            {
                stack3.IsVisible = false;
                lbl_FileName3.IsVisible = false;
            }
            else if (ReferenceEquals(sender, FileName4))
            {
                stack4.IsVisible = false;
                lbl_FileName4.IsVisible = false;
            }
            else if (ReferenceEquals(sender, FileName5))
            {
                stack5.IsVisible = false;
                lbl_FileName5.IsVisible = false;
            }
        }

    }
}