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
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ObservableCollection<string> Items { get; set; }

        public List<MyItem> ItemList { get; set; }


        public ClubInfomationList()
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
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + " " + _sqlite.Db_A_Account.MemberLastName;

            // 連絡事項データ取得
            GetInfomation();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetInfomation()
        {
            string WorkCode;
            string WorkDate;
            string WorkSubject;
            List<InfomationRow> items = new List<InfomationRow>();

            try
            {
                //foreach (Table.T_INFOMATION row in _sqlite.Get_T_INFOMATION("SELECT * " +
                //                                                            "FROM T_INFOMATION " +
                //                                                            "ORDER BY AddDate DESC"))
                //{
                //    WorkCode = row.ClubCode;
                //    WorkDate = row.AddDate.Substring(0, 10);
                //    WorkSubject = row.Subject;
                //    items.Add(new InfomationRow(WorkCode, WorkDate, WorkSubject));
                //}
                if(items.Count > 0)
                {
                    ClubInfomationListView.ItemsSource = items;
                }
                else
                {
                    ItemList = new List<MyItem>();
                    ItemList.Add(new MyItem { TemplateId = 1 });
                    this.BindingContext = this;
                    //items.Add(new InfomationRow("", "連絡事項がありません。", ""));
                    //ClubInfomationListView.ItemsSource = items;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION) : &{ex.Message}", "OK");
            }
        }

        private void Initialize()
        {
            
        }
    }

    public sealed class InfomationRow
    {
        public InfomationRow(string clubCode, string addDate, string subject)
        {
            ClubCode = clubCode;
            AddDate = addDate;
            Subject = subject;
        }
        public string ClubCode { get; set; }
        public string AddDate { get; set; }
        public string Subject { get; set; }
    }

    public class MyDataTemplateSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate FirstTemplate { get; set; }
        public DataTemplate SecondTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //ここに条件を書き
            //条件にマッチするプロパティ(DataTemplate)を返す
            return ((MyItem)item).TemplateId == 0 ? FirstTemplate : SecondTemplate;

            //itemパラメータにはXaml側から各項目にバインドされたオブジェクト
            //今回はMyItemオブジェクトが入ってくる
            //今回はそれを利用してテンプレートを切り替える
        }
    }

    public class MyItem
    {
        public int TemplateId { get; set; }
    }
}