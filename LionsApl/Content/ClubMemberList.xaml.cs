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
    public partial class ClubMemberList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<ClubMemberRow> Items { get; set; }

        public ClubMemberList()
        {
            InitializeComponent();

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
            List<ClubMemberRow> items = new List<ClubMemberRow>();

            Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER("Select * " +
                                                                    "From M_MEMBER " +
                                                                    "ORDER BY MemberNameKana"))
                {
                    wkMemberCode = row.MemberCode;
                    wkJoinDate = Util.GetString(row.JoinDate).Substring(0, 10);
                    wkExecutiveName = Util.GetString(row.ExecutiveName);
                    wkMemberName = Util.GetString(row.MemberFirstName) + " " +
                                   Util.GetString(row.MemberLastName) + " (" +
                                   Util.GetString(row.TypeName) + ")";
                    wkCommitteeName = Util.GetString(row.CommitteeName);
                    items.Add(new ClubMemberRow(wkMemberCode, wkJoinDate, wkExecutiveName, wkMemberName, wkCommitteeName));
                }
                ClubMemberListView.ItemsSource = items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(M_MEMBER) : &{ex.Message}", "OK");
            }

        }

        
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            ClubMemberRow item = e.Item as ClubMemberRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.MemberCode))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 会員情報画面へ
            Navigation.PushAsync(new ClubMemberPage(item.MemberCode));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    public sealed class ClubMemberRow
    {
        public ClubMemberRow(string membercode, string joindate, string executivename, string membername, string committeename)
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

    public class MyClubMembetrSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubMemberRow)item;
            if (!String.IsNullOrEmpty(info.MemberCode))
            {
                return ExistDataTemplate;
            }
            else
            {
                return NoDataTemplate;
            }
        }
    }

}