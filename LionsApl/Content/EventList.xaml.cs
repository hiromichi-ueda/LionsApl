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

        // リストビュー設定内容
        public List<EventRow> Items { get; set; }

        public EventList()
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


            // イベント情報データ取得
            GetEventData();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventData()
        {




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

            EventRow item = e.Item as EventRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.EventClass))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 出欠確認画面へ
            //Navigation.PushAsync(new EventPage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    public sealed class EventRow
    {
        public EventRow(int datano, string eventclass, string eventdate)
        {
            DataNo = datano;
            EventClass = eventclass;
            EventDate = eventdate;
        }
        public int DataNo { get; set; }
        public string EventClass { get; set; }
        public string EventDate { get; set; }
    }

    public class MyEventSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (EventRow)item;
            if (!String.IsNullOrEmpty(info.EventClass))
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