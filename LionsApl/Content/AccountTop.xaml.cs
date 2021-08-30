using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountTop : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ObservableCollection<string> Items { get; set; }

        public AccountTop()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // アカウント情報データ取得



        }

        //---------------------------------------
        // アカウント設定画面(編集)
        //---------------------------------------
        void Button_Edit_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new AccountSetting();
        }

    }
}