using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：例会プログラムページクラス(Android用)
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMeetingProgramPageAndroid : ContentPage
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

        // リストビュー設定内容
        public ObservableCollection<MeetingProgramFileRow> Items;

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
        public ClubMeetingProgramPageAndroid(int dataNo)
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
            string wkRowTitle = string.Empty;
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
            Items = new ObservableCollection<MeetingProgramFileRow>();

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
                    }

                    wkClubCode = _utl.GetString(row.ClubCode);
                    wkFileName = _utl.GetString(row.FileName);
                    wkFileName1 = _utl.GetString(row.FileName1);
                    wkFileName2 = _utl.GetString(row.FileName2);
                    wkFileName3 = _utl.GetString(row.FileName3);
                    wkFileName4 = _utl.GetString(row.FileName4);
                    wkFileName5 = _utl.GetString(row.FileName5);

                    wkRowTitle = "例会プログラム";
                    if(wkFileName != string.Empty)
                    {
                        Items.Add(new MeetingProgramFileRow(wkRowTitle, wkDataNo, wkClubCode, wkFileName));
                    }

                    wkRowTitle = "添付ファイル";
                    if (wkFileName1 != string.Empty)
                    {
                        Items.Add(new MeetingProgramFileRow(wkRowTitle, wkDataNo, wkClubCode, wkFileName1));
                    }
                    if (wkFileName2 != string.Empty)
                    {
                        Items.Add(new MeetingProgramFileRow(wkRowTitle, wkDataNo, wkClubCode, wkFileName2));
                    }
                    if (wkFileName3 != string.Empty)
                    {
                        Items.Add(new MeetingProgramFileRow(wkRowTitle, wkDataNo, wkClubCode, wkFileName3));
                    }
                    if (wkFileName4 != string.Empty)
                    {
                        Items.Add(new MeetingProgramFileRow(wkRowTitle, wkDataNo, wkClubCode, wkFileName4));
                    }
                    if (wkFileName5 != string.Empty)
                    {
                        Items.Add(new MeetingProgramFileRow(wkRowTitle, wkDataNo, wkClubCode, wkFileName5));
                    }
                    MeetingProgramFileListView.ItemsSource = Items;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リストタップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            MeetingProgramFileRow item = e.Item as MeetingProgramFileRow;

            // 例会プログラムファイル画面へ
            Navigation.PushAsync(new ClubMeetingProgramFileAndroid(item.DataNo, item.ClubCode, item.FileName));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public sealed class MeetingProgramFileRow
        {
            public MeetingProgramFileRow(string rowTitle, int dataNo, string clubCode, string fileName)
            {
                RowTitle = rowTitle;
                DataNo = dataNo;
                ClubCode = clubCode;
                FileName = fileName;
            }
            public string RowTitle { get; set; }
            public int DataNo { get; set; }
            public string ClubCode { get; set; }
            public string FileName { get; set; }
        }

    }
}