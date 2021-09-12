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
    public partial class LetterList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<LetterRow> Items { get; set; }

        public LetterList()
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

            // キャビネットレター情報取得
            GetLetter();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetLetter()
        {
            int WorkDataNo;
            string WorkDate = string.Empty;
            string WorkTitle = string.Empty;
            //List<LetterRow> items = new List<LetterRow>();
            Items = new List<LetterRow>();

            try
            {
                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "ORDER BY EventDate DESC, EventTime DESC, DataNo DESC"))
                {
                    WorkDataNo = row.DataNo;
                    WorkDate = row.EventDate.Substring(0, 10) + "  " + row.EventTime;
                    WorkTitle = row.Title;
                    Items.Add(new LetterRow(WorkDataNo, WorkDate, WorkTitle));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new LetterRow(0, WorkDate, WorkTitle));
                }
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
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

            LetterRow item = e.Item as LetterRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.Title))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // キャビネットレター画面へ
            Navigation.PushAsync(new LetterPage(item.Title, item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// キャビネットレター行情報クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class LetterRow
    {
        public LetterRow(int dataNo, string dateTime, string title)
        {
            DataNo = dataNo;
            DateTime = dateTime;
            Title = title;
        }
        public int DataNo { get; set; }
        public string DateTime { get; set; }
        public string Title { get; set; }
    }

    public class MyLetterSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (LetterRow)item;
            if (!String.IsNullOrEmpty(info.Title))
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
