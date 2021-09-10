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
    public partial class ClubInfomationPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Config取得
        public static String AppServer = ((App)Application.Current).AppServer;                              //Url
        public static String AndroidPdf = ((App)Application.Current).AndroidPdf;                            //PdfViewer
        public static String FilePath_ClubInfometion = ((App)Application.Current).FilePath_ClubInfometion;  //連絡事項(CLUB)

        // 前画面からの取得情報
        private int _DataNo;

        public ClubInfomationPage(int datano)
        {
            InitializeComponent();

            // font-size
            this.lbl_AddDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.AddDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Subject.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.lbl_Detail.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.Detail.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 一覧から取得
            _DataNo = datano;

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
            _sqlite.GetFilePath(FilePath_ClubInfometion);

            // 連絡事項（クラブ）情報設定
            GetClubInfometion();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項情報をSQLiteファイルから取得して画面に設定する。
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
                                                                    "Where DataNo='" + _DataNo + "'"))
                {

                    wkClubCode = row.ClubCode;
                    AddDate.Text = row.AddDate.Substring(0, 10);
                    Subject.Text = row.Subject;
                    Detail.Text = row.Detail;

                    if (row.FileName != null)
                    {
                        // FILEPATH取得
                        var filepath = _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "");
                        
                        // FILEPATH生成([ClubCode]変換)
                        var fileUrl = AppServer + filepath.Replace("[ClubCode]", wkClubCode).Replace("\\", "/").Replace("\r\n", "") +
                                     "/" + row.DataNo.ToString() + "/" + row.FileName;

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
                        lbl_FileName.Text = fileUrl;
                    }
                    else
                    {
                        //lbl_FileName.Text = "連絡事項―添付ファイルなし";
                        this.FileName.IsEnabled = false;
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