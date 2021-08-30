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
    public partial class EventList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        public ObservableCollection<string> Items { get; set; }

        public EventList()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // イベント情報データ取得




        }

        private void Label_List_Taped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EventPage());
        }

    }
}