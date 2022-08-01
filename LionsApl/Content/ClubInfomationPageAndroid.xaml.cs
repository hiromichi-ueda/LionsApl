using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：連絡事項ページクラス(Android用)
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubInfomationPageAndroid : ContentPage
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

        // リストビュー設定内容
        public ObservableCollection<InfomationFileRow> Items;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="datano">データNo.</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubInfomationPageAndroid(int dataNo)
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
            string wkFileName;
            Items = new ObservableCollection<InfomationFileRow>();

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
                    wkFileName = _utl.GetString(row.FileName);
                    if (wkFileName != string.Empty)
                    {
                        // 遷移先の画面でファイルを表示するため、ファイル名の表示に留める
                        Items.Add(new InfomationFileRow(row.DataNo, wkClubCode, wkFileName));
                    }
                    InfomationFileListView.ItemsSource = Items;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION_CLUB) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リストタップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            InfomationFileRow item = e.Item as InfomationFileRow;

            // 連絡事項ファイル表示画面へ
            Navigation.PushAsync(new ClubInfomationFileAndroid(item.DataNo, item.ClubCode, item.FileName));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public sealed class InfomationFileRow
        {
            public InfomationFileRow(int dataNo, string clubCode, string fileName)
            {
                DataNo = dataNo;
                ClubCode = clubCode;
                FileName = fileName;
            }
            public int DataNo { get; set; }
            public string ClubCode { get; set; }
            public string FileName { get; set; }
        }

    }
}