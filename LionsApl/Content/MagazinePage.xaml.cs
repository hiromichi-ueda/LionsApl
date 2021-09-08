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

        // url取得
        public static String AppServer = ((App)Application.Current).AppServer;

        // 前画面からのデータNo取得情報
        private int _dataNo;

        public MagazinePage(int InDataNo)
        {
            InitializeComponent();

            // font-size
            this.LoginInfo.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));      //Login
            this.title.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));        //Title

            // DataNo取得(Key)
            _dataNo = InDataNo;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // ツールバーに一覧ボタンを設定
            //ToolbarItems.Add(new ToolbarItem { Text = "一覧", Command = new Command(Push_MagazineList) });

            // A_FILEPATHデータ取得
            _sqlite.GetFilePath(_sqlite.DATACLASS_MAGAZINE);

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

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
                        string urlStr = AppServer + _sqlite.Db_A_FilePath.FilePath.Substring(2).Replace("\\", "/").Replace("\r\n", "") + 
                                        "/" + row.DataNo.ToString() + "/" + row.FileName;
                        string source = urlStr;
                        Pdf.Source = source;

                        PdfLabel.Text = urlStr;

                    }
                    else
                    {
                        PdfLabel.Text = "地区誌ファイルなし";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
            }
        }

        private void Push_MagazineList()
        {
            Navigation.PushAsync(new MagazineList());
        }
    }
}