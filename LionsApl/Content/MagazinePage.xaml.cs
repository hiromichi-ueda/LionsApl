using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MagazinePage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;      //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;    //PdfViewer

        // 前画面からのデータNo取得情報
        private int _dataNo;

        public MagazinePage(int InDataNo)
        {
            InitializeComponent();

            // DataNo取得(Key)
            _dataNo = InDataNo;

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
            _sqlite.GetFilePath(_sqlite.DATACLASS_MAGAZINE);

            // 地区誌情報設定
            GetMagazine();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMagazine()
        {

            try
            {
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("Select * " +
                                                                        "From T_MAGAZINE " +
                                                                        "Where DataNo='" + _dataNo + "'"))
                {
                    if (row.FileName != null)
                    {
                        // 地区誌パス
                        var pdfUrl = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "") +
                                     "/" + row.DataNo.ToString() + "/" + row.FileName;

                        // AndroidPDF Viewer
                        var googleUrl = AndroidPdf + "?embedded=true&url=";

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            Pdf.Source = pdfUrl;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            Pdf.Source = new UrlWebViewSource() { Url = googleUrl + pdfUrl };
                        }
                        PdfLabel.Text = pdfUrl;
                    }
                    else
                    {
                        PdfLabel.Text = "地区誌ファイルなし";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MAGAZINE) : &{ex.Message}", "OK");
            }
        }

        private void Push_MagazineList()
        {
            Navigation.PushAsync(new MagazineList());
        }
    }
}