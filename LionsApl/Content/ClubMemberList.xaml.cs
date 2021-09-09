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

            // font-size(<ListView>はCSSが効かないのでここで設定)
            this.LoginInfo.FontSize = 16.0;
            this.title.FontSize = 16.0;

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
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            if (e.Item == null)
                return;

            MemberRow item = e.Item as MemberRow;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;

            await Navigation.PushAsync(new ClubMemberPage(item.MemberCode));
        }
    }
}