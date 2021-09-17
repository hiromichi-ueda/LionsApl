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
    public partial class InfomationPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                      //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                    //PdfViewer
        public static String FilePath_Infometion = ((App)Application.Current).FilePath_Infometion;  //連絡事項(CABINET)

        // 前画面からの取得情報-
        private string _DataNo;        // データNo.

        public InfomationPage(string dataNo)
        {
            InitializeComponent();

            // font-size
            this.lbl_AddDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.AddDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Detail.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Detail.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 一覧から取得(データ№)
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
            _sqlite.GetFilePath(FilePath_Infometion);

            // 連絡事項(キャビネット)情報設定
            GetCabiInfometion();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項(CABINET)情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetCabiInfometion()
        {
            // 変数
            string wkDataNo = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.T_INFOMATION_CABI row in _sqlite.Get_T_INFOMATION_CABI("Select * " +
                                                                    "From Get_T_INFOMATION_CABI " +
                                                                    "Where DataNo='" + _DataNo + "'"))
                {

                    // Data№取得
                    wkDataNo = row.DataNo.ToString();

                    // 各項目情報取得
                    AddDate.Text = Util.GetString(row.AddDate).Substring(0, 10);    //連絡日
                    Subject.Text = Util.GetString(row.Subject);                     //件名
                    Detail.Text = Util.GetString(row.Detail);                       //内容

                    // 添付ファイル
                    if (Util.GetString(row.FileName) != "")
                    {
                        // ファイル表示高さ設定
                        this.grid.HeightRequest = 600.0;

                        // FILEPATH取得(連絡事項)
                        var fileUrl = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "") +
                                     "/" + wkDataNo + "/" + Util.GetString(row.FileName);

                        // AndroidPDF Viewer
                        var googleUrl = AndroidPdf + "?embedded=true&url=";

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            FileName.Source = fileUrl;
                        }
                        else if (Device.RuntimePlatform == Device.Android)
                        {
                            FileName.Source = new UrlWebViewSource() { Url = googleUrl + fileUrl };
                        }
                        lbl_FileName.Text = fileUrl;            //FileName表示
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
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION) : &{ex.Message}", "OK");
            }
        }

    }
}