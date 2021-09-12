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

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                              //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                            //PdfViewer
        public static String FilePath_MeetingProgram = ((App)Application.Current).FilePath_MeetingProgram;  //例会プログラム(CLUB)

        // 対象データNo.
        private int _DataNo;
        private int _ScheduleDataNo;

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
            string wkClubCode;

            // 例会プログラム情報取得
            try
            {
                
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGPROGRAM) : &{ex.Message}", "OK");
            }



        }

    }
}