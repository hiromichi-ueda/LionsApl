using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：連絡事項ページクラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubInfomationPage : ContentPage
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
        public static String AppServer = ((App)Application.Current).AppServer;                              //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                            //PdfViewer
        public static String FilePath_ClubInfometion = ((App)Application.Current).FilePath_ClubInfometion;  //連絡事項(CLUB)


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="datano">データNo.</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubInfomationPage(int dataNo)
        {
            InitializeComponent();

            // font-size
            this.lbl_AddDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.AddDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Detail.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Detail.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 前画面からの取得情報
            _dataNo = dataNo;                   // データ№.

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

            // 連絡事項（クラブ）情報設定
            GetClubInfometion();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項(CLUB)情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetClubInfometion()
        {

            // 変数宣言
            string wkClubCode;

            // 連絡事項情報取得
            try
            {
                foreach (Table.T_INFOMATION_CLUB row in _sqlite.Get_T_INFOMATION_CLUB("Select * " +
                                                                    "From T_INFOMATION_CLUB " +
                                                                    "Where DataNo='" + _dataNo + "'"))
                {

                    // 各項目情報取得
                    wkClubCode = _utl.GetString(row.ClubCode);                      //クラブコード
                    AddDate.Text = _utl.GetString(row.AddDate).Substring(0, 10);    //連絡日
                    Subject.Text = _utl.GetString(row.Subject);                     //件名
                    Detail.Text = _utl.GetString(row.Detail);                       //内容

                    // 添付ファイル
                    if (_utl.GetString(row.FileName) != string.Empty)
                    {
                        // ファイル表示高さ設定
                        this.grid.HeightRequest = 600.0;

                        // FILEPATH取得
                        var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");
                        
                        // FILEPATH生成([ClubCode]変換)
                        var fileUrl = AppServer + filepath.Replace("[ClubCode]", wkClubCode).Replace("\\", "/").Replace("\r\n", "") +
                                     "/" + row.DataNo.ToString() + "/" + _utl.GetString(row.FileName);

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
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION_CLUB) : &{ex.Message}", "OK");
            }
        }


    }
}