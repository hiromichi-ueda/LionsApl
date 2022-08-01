using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：連絡事項ファイルクラス(Android用)
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubInfomationFileAndroid : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private int _dataNo;            // データNo.
        private string _clubCode;       // クラブコード
        private string _fileName;       // ファイル名

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                              //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                            //PdfViewer
        public static String FilePath_ClubInfometion = ((App)Application.Current).FilePath_ClubInfometion;  //連絡事項(CLUB)


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo">データNo.</param>
        /// <param name="clubCode">クラブコード</param>
        /// <param name="fileName">ファイル名</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubInfomationFileAndroid(int dataNo, string clubCode, string fileName)
        {
            InitializeComponent();

            // 前画面からの取得情報
            _dataNo = dataNo;                       // データ№.
            _clubCode = clubCode;                   // クラブコード
            _fileName = fileName;                   // ファイル名

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_ClubInfometion);

            // 連絡事項（クラブ）ファイル情報設定
            SetClubInfometionFile();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 前画面の情報からファイルビューアを画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetClubInfometionFile()
        {
            // ファイル表示高さ設定
            this.grid.HeightRequest = 600.0;

            // FILEPATH取得
            var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");

            // FILEPATH生成([ClubCode]変換)
            var fileUrl = AppServer + filepath.Replace("[ClubCode]", _clubCode).Replace("\\", "/").Replace("\r\n", "") +
                         "/" + _dataNo.ToString() + "/" + _fileName;

            // AndroidPDF Viewer
            var googleUrl = AndroidPdf + "?embedded=true&url=";

            FileName.Source = new UrlWebViewSource() { Url = googleUrl + fileUrl };
        }


    }
}