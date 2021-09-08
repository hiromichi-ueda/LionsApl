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
    public partial class EventPage : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        // 前画面からの取得情報
        private string _titleName;                           // タイトル
        private int _dataNo;                                 // データNo.
        private int _eventDataNo;                            // イベントNo.

        public EventPage(string title, int dataNo, int eventDataNo)
        {
            InitializeComponent();

            // font-size
            this.LoginInfo.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));      //Login
            this.title.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));        //Title

            _titleName = title;
            _dataNo = dataNo;
            _eventDataNo = eventDataNo;

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

            // イベント情報データ取得




        }
    }
}