using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMemberList : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス


        public ObservableCollection<string> Items { get; set; }

        public ClubMemberList()
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


            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

            // 会員情報データ取得
            GetMember();

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 会員情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMember()
        {

            string wkMemberCode;
            string wkJoinDate;
            string wkExecutiveName;
            string wkMemberName;
            string wkCommitteeName;
            List<MemberRow> items = new List<MemberRow>();

            try
            {
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER("Select * " +
                                                                    "From M_MEMBER " +
                                                                    "ORDER BY MemberNameKana"))
                {
                    wkMemberCode = row.MemberCode;
                    wkJoinDate = row.JoinDate.Substring(0, 10);
                    wkExecutiveName = row.ExecutiveName;
                    wkMemberName = row.MemberFirstName + " " + row.MemberLastName + " (" + row.TypeName + ")";
                    wkCommitteeName = row.CommitteeName;
                    items.Add(new MemberRow(wkMemberCode, wkJoinDate, wkExecutiveName, wkMemberName, wkCommitteeName));
                }
                ClubMemberListView.ItemsSource = items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_MEMBER) : &{ex.Message}", "OK");
            }

        }

        public sealed class MemberRow
        {
            public MemberRow(string membercode, string joindate, string executivename, string membername, string committeename)
            {
                MemberCode = membercode;
                JoinDate = joindate;
                ExecutiveName = executivename;
                MemberName = membername;
                CommitteeName = committeename;
            }
            public string MemberCode { get; set; }
            public string JoinDate { get; set; }
            public string ExecutiveName { get; set; }
            public string MemberName { get; set; }
            public string CommitteeName { get; set; }
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

            MemberRow item = e.Item as MemberRow;

            Navigation.PushAsync(new ClubMemberPage(item.MemberCode));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }
}