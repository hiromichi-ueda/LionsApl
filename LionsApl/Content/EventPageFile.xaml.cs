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

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;            // Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;          // PdfViewer
        public static String FilePath_Event = ((App)Application.Current).FilePath_Evnet;  // 連絡事項(CLUB)

        public EventPageFile(int dataNo, string fileName)
        {
            InitializeComponent();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(FilePath_Event);

            // FILEPATH取得
            var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");

            // FILEPATH生成
            var fileUrl = AppServer + filepath.Replace("\\", "/").Replace("\r\n", "") +
                         "/" + dataNo.ToString() + "/" + _utl.GetString(fileName);

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
            PdfLabel.Text = fileUrl;

        }
    }
}