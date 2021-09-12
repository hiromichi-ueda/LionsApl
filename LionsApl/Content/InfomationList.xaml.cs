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
    public partial class InfomationList : ContentPage
    {

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<InfomationRow> Items { get; set; }

        public InfomationList()
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

            // 連絡事項(キャビネット)データ取得
            GetInfomation();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項(キャビネット)情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetInfomation() 
        {
            int wkDataNo;
            string wkAddDate = string.Empty;
            string wkSubject = string.Empty;
            Items = new List<InfomationRow>();

            try
            {




                // ログインユーザーが対象の連絡事項が1件もない場合
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new InfomationRow(0, wkAddDate, wkSubject));
                }
                this.BindingContext = this;

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION) : &{ex.Message}", "OK");
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

            InfomationRow item = e.Item as InfomationRow;

            // 連絡事項が1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.AddDate))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 連絡事項(クラブ)画面へ
            Navigation.PushAsync(new InfomationPage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    public sealed class InfomationRow
    {
        public InfomationRow(int datano, string addDate, string subject)
        {
            DataNo = datano;
            AddDate = addDate;
            Subject = subject;
        }
        public int DataNo { get; set; }
        public string AddDate { get; set; }
        public string Subject { get; set; }
    }

    public class MyInfomationSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (InfomationRow)item;
            if (!String.IsNullOrEmpty(info.AddDate))
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