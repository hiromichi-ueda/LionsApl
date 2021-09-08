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
    public partial class MatchingList : ContentPage
    {

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        public ObservableCollection<string> Items { get; set; }

        public MatchingList()
        {
            InitializeComponent();

            // font-size
            this.LoginInfo.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));      //Login
            this.title.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));        //Title

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // マッチング情報データ取得



        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //MemberRow item = e.Item as MemberRow;

            //Navigation.PushAsync(new ClubMemberPage(item.MemberCode));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    
    }
}