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
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 地区誌ページクラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MagazinePageAndroid : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                      //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                    //PdfViewer
        public static String FilePath_Magazine = ((App)Application.Current).FilePath_Magazine;      //地区誌PATH

        // 前画面からのデータNo取得情報
        private int _dataNo;


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public MagazinePageAndroid(int dataNo)
        {
            InitializeComponent();

            // DataNo取得(Key)
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

            //Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.T_MAGAZINE row in _sqlite.Get_T_MAGAZINE("Select * " +
                                                                        "From T_MAGAZINE " +
                                                                        "Where DataNo='" + _dataNo + "'"))
                {
                    // Data№取得
                    wkDataNo = row.DataNo.ToString();

                    // 添付ファイル
                    if (_utl.GetString(row.FileName) != string.Empty)
                    {

                        // ファイル表示高さ設定
                        PdfWebView.HeightRequest = 600.0;

                        // FILEPATH取得(地区誌)
                        var pdfUrl = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "") +
                                     "/" + wkDataNo + "/" + _utl.GetString(row.FileName);

                        // AndroidPDF Viewer
                        var googleUrl = AndroidPdf + "?embedded=true&url=";

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            PdfWebView.Source = pdfUrl;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            PdfWebView.Source = new UrlWebViewSource() { Url = googleUrl + pdfUrl };
                        }

                    }
                    else
                    {
                        // 非表示設定
                        PdfWebView.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MAGAZINE) : &{ex.Message}", "OK");
            }
        }

    }
}