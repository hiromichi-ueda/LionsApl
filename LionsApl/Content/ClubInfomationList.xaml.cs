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
    public partial class ClubInfomationList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<ClubInfomationRow> Items { get; set; }


        public ClubInfomationList()
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

            // 連絡事項データ取得
            GetInfomation();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            ClubInfomationRow item = e.Item as ClubInfomationRow;

            // 連絡事項が1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.ClubCode))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            Navigation.PushAsync(new ClubInfomationPage(item.ClubCode, item.AddDate));

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetInfomation()
        {
            string WorkTypeCode = string.Empty;
            string WorkClubCode = string.Empty;
            string WorkDate = string.Empty;
            string WorkSubject = string.Empty;
            string WorkFlg = string.Empty;
            string[] WorkCodeList = null;
            bool AddListFlg = false;
            Items = new List<ClubInfomationRow>();

            try
            {
                // 会員マスタよりログインユーザーの会員情報を取得
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER(
                                                                 "SELECT * " +
                                                                 "FROM M_MEMBER " +
                                                                 "WHERE MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' "))
                {
                    // 会員種別を保持
                    WorkTypeCode = row.TypeCode;
                }

                // 連絡事項(クラブ)のデータを全件取得
                foreach (Table.T_INFOMATION_CLUB row in _sqlite.Get_T_INFOMATION_CLUB(
                                                                 "SELECT * " +
                                                                 "FROM T_INFOMATION_CLUB " +
                                                                 "ORDER BY AddDate DESC"))
                {
                    AddListFlg = false;
                    WorkFlg = row.InfoFlg;

                    // 全会員の場合
                    if (WorkFlg == "1")
                    {
                        WorkCodeList = row.TypeCode.Split(',');
                        foreach (string code in WorkCodeList)
                        {
                            // 会員種別を条件にログインユーザーが対象か判定
                            if (WorkTypeCode.Equals(code))
                            {
                                AddListFlg = true;
                                break;
                            }
                        }
                    }

                    // 個別設定の場合
                    else
                    {
                        WorkCodeList = row.InfoUser.Split(',');
                        foreach (string code in WorkCodeList)
                        {
                            // 連絡者(会員番号)を条件にログインユーザーが対象か判定
                            if (_sqlite.Db_A_Account.MemberCode.Equals(code))
                            {
                                AddListFlg = true;
                                break;
                            }
                        }
                    }

                    // ログインユーザーが対象の連絡事項を設定
                    if (AddListFlg)
                    {
                        WorkClubCode = row.ClubCode;
                        WorkDate = row.AddDate.Substring(0, 10);
                        WorkSubject = row.Subject;
                        Items.Add(new ClubInfomationRow(WorkClubCode, WorkDate, WorkSubject));
                    }
                }

                // ログインユーザーが対象の連絡事項が1件もない場合
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubInfomationRow(WorkClubCode,WorkDate,WorkSubject));
                }
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION_CLUB) : &{ex.Message}", "OK");
            }
        }
    }

    public sealed class ClubInfomationRow
    {
        public ClubInfomationRow(string clubCode, string addDate, string subject)
        {
            ClubCode = clubCode;
            AddDate = addDate;
            Subject = subject;
        }
        public string ClubCode { get; set; }
        public string AddDate { get; set; }
        public string Subject { get; set; }
    }

    public class MyInfomationTemplateSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubInfomationRow)item;
            if (!String.IsNullOrEmpty(info.ClubCode)){
                return ExistDataTemplate;
            }
            else
            {
                return NoDataTemplate;
            }
        }
    }
}