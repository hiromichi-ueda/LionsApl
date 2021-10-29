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
    public partial class EventPageFile : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;            // Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;          // PdfViewer
        public static String FilePath_Event = ((App)Application.Current).FilePath_Evnet;  // 連絡事項(CLUB)


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventDataNo"></param>
        /// <param name="fileName"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public EventPageFile(int eventDataNo, string fileName)
        {
            InitializeComponent();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_Event);

            // FILEPATH取得
            var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");

            // FILEPATH生成
            var fileUrl = AppServer + filepath.Replace("\\", "/").Replace("\r\n", "") +
                         "/" + eventDataNo.ToString() + "/" + _utl.GetString(fileName);

            // AndroidPDF Viewer
            var googleUrl = AndroidPdf + "?embedded=true&url=";

            // iOSの場合
            if (Device.RuntimePlatform == Device.iOS)
            {
                PdfWebView.Source = fileUrl;
            }
            // Androidの場合
            else if (Device.RuntimePlatform == Device.Android)
            {
                PdfWebView.Source = new UrlWebViewSource() { Url = googleUrl + fileUrl };
            }
            // URLラベル設定（テスト表示用）
            PdfLabel.Text = fileUrl;
            // URLラベル非表示にする
            PdfLabel.IsVisible = false;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ナビゲーション開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void WebviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            stack.IsVisible = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ナビゲーション終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void WebviewNavigated(object sender, WebNavigatedEventArgs e)
        {
            stack.IsVisible = false;
        }

    }
}