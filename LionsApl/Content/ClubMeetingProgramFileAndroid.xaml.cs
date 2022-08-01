
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
    public partial class ClubMeetingProgramFileAndroid : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private int _dataNo;                // データNo.
        private string _clubCode;           // クラブコード
        private string _fileName;           // ファイル名

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
        public ClubMeetingProgramFileAndroid(int dataNo, string clubCode, string fileName)
        {
            InitializeComponent();

            // 前画面の情報設定
            _dataNo = dataNo;
            _clubCode = clubCode;
            _fileName = fileName;

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
            SetFileUrl(dataNo, clubCode, fileName, ref FileName);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// WebViewにURLを設定する。
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

            webView.Source = new UrlWebViewSource() { Url = googleUrl + fileUrl };
            webView.HeightRequest = 600.0;

            return fileUrl;
        }
    }
}