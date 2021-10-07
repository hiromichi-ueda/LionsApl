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
        public static String AppServer = ((App)Application.Current).AppServer;                      //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                    //PdfViewer
        public static String FilePath_Magazine = ((App)Application.Current).FilePath_Magazine;      //地区誌PATH

        // 前画面からのデータNo取得情報
        private int _dataNo;

        public MagazinePage(int dataNo)
        {
            InitializeComponent();

            // DataNo取得(Key)
            _dataNo = dataNo;

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
            _sqlite.GetFilePath(FilePath_Magazine);

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

            // 変数
            string wkDataNo = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("Select * " +
                                                                        "From T_MAGAZINE " +
                                                                        "Where DataNo='" + _dataNo + "'"))
                {
                    // Data№取得
                    wkDataNo = row.DataNo.ToString();

                    // 添付ファイル
                    if (Util.GetString(row.FileName) != "")
                    {

                        // ファイル表示高さ設定
                        this.grid.HeightRequest = 600.0;

                        // FILEPATH取得(地区誌)
                        var pdfUrl = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "") +
                                     "/" + wkDataNo + "/" + Util.GetString(row.FileName);

                        // AndroidPDF Viewer
                        var googleUrl = AndroidPdf + "?embedded=true&url=";

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            FileName.Source = pdfUrl;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            FileName.Source = new UrlWebViewSource() { Url = googleUrl + pdfUrl };
                        }
                        lbl_FileName.Text = pdfUrl;             //FileName表示
                        this.lbl_FileName.HeightRequest = 0;    //非表示設定
                    }
                    else
                    {
                        // WebViewの高さ消す
                        this.grid.HeightRequest = 0;
                        this.FileName.IsVisible = false;
                        lbl_FileName.Text = "";
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