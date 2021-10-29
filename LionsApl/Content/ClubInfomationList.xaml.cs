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
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：連絡事項一覧クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubInfomationList : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // リストビュー設定内容
        public List<ClubInfomationRow> Items { get; set; }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubInfomationList()
        {
            InitializeComponent();

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

            // 連絡事項データ取得
            GetClubInfomation();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetClubInfomation()
        {
            int wkDataNo = 0;
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
                    WorkTypeCode = _utl.GetString(row.TypeCode);
                }

                // 連絡事項(クラブ)のデータを全件取得
                foreach (Table.T_INFOMATION_CLUB row in _sqlite.Get_T_INFOMATION_CLUB(
                                                                 "SELECT * " +
                                                                 "FROM T_INFOMATION_CLUB " +
                                                                 "ORDER BY AddDate DESC"))
                {
                    AddListFlg = false;
                    WorkFlg = _utl.GetString(row.InfoFlg);

                    // データ№
                    wkDataNo = row.DataNo;

                    // 全会員の場合
                    if (WorkFlg == _utl.INFOFLG_ALL)
                    {
                        WorkCodeList = _utl.GetString(row.TypeCode).Split(',');
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
                    else if (WorkFlg == _utl.INFOFLG_PRIV)
                    {
                        WorkCodeList = _utl.GetString(row.InfoUser).Split(',');
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
                        WorkClubCode = _utl.GetString(row.ClubCode);
                        WorkDate = _utl.GetString(row.AddDate).Substring(0, 10);
                        WorkSubject = _utl.GetString(row.Subject);
                        Items.Add(new ClubInfomationRow(wkDataNo, WorkClubCode, WorkDate, WorkSubject));

                    }
                }

                // ログインユーザーが対象の連絡事項が1件もない場合
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    //Items.Add(new ClubInfomationRow(wkDataNo, WorkClubCode, WorkDate, WorkSubject));
                    Items.Add(new ClubInfomationRow(0, WorkClubCode, WorkDate, WorkSubject));
                }
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION_CLUB) : &{ex.Message}", "OK");
            }
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

            ClubInfomationRow item = e.Item as ClubInfomationRow;

            // 連絡事項が1件もない(メッセージ行のみ表示している)場合は処理しない
            if (item.DataNo == 0)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 連絡事項(クラブ)画面へ
            Navigation.PushAsync(new ClubInfomationPage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    public sealed class ClubInfomationRow
    {
        public ClubInfomationRow(int dataNo, string clubCode, string addDate, string subject)
        {
            DataNo = dataNo;
            ClubCode = clubCode;
            AddDate = addDate;
            Subject = subject;
        }
        public int DataNo { get; set; }
        public string ClubCode { get; set; }
        public string AddDate { get; set; }
        public string Subject { get; set; }
    }

    public class MyClubInfomationSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubInfomationRow)item;
            if (info.DataNo > 0){
                return ExistDataTemplate;
            }
            else
            {
                return NoDataTemplate;
            }
        }
    }
}