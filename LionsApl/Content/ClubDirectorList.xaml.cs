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
    public partial class ClubDirectorList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<ClubDirectorRow> Items { get; set; }

        public ClubDirectorList()
        {
            InitializeComponent();

            // font-size(<ListView>はCSSが効かないのでここで設定)
            //this.LoginInfo.FontSize = 16.0;
            //this.title.FontSize = 16.0;

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

            // 理事・委員会データ取得
            GetClubDirector();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetClubDirector()
        {
            int wkDataNo;
            string wkEventClass = string.Empty;
            string wkEventDate = string.Empty;
            string wkEventTime = string.Empty;
            string wkSubject = string.Empty;
            Items = new List<ClubDirectorRow>();

            try
            {
                foreach (Table.T_DIRECTOR row in _sqlite.Get_T_DIRECTOR(
                                                                 "SELECT * " +
                                                                 "FROM T_DIRECTOR " +
                                                                 "ORDER BY EventDate DESC"))
                {
                    wkDataNo = row.DataNo;
                    if (row.EventClass == "1")
                    {
                        wkEventClass = "理事会";
                    }
                    else
                    {
                        wkEventClass = "委員会";
                    }
                    wkEventTime = row.EventTime.Substring(0, 5);
                    if (wkEventTime == "00:00")
                    {
                        wkEventDate = row.EventDate.Substring(0, 10);
                    }
                    else
                    {
                        wkEventDate = row.EventDate.Substring(0, 10) + " " + wkEventTime;
                    }
                    wkSubject = row.Subject;
                    Items.Add(new ClubDirectorRow(wkDataNo, wkEventClass, wkEventDate, wkSubject));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubDirectorRow(0, wkEventClass, wkEventDate, wkSubject));
                    //DisplayAlert("Alert", $"Data Nothing", "OK");
                }
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
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

            ClubDirectorRow item = e.Item as ClubDirectorRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.EventClass))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            Navigation.PushAsync(new ClubDirectorPage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }


    public sealed class ClubDirectorRow
    {
        public ClubDirectorRow(int datano, string eventclass, string eventdate, string subject)
        {
            DataNo = datano;
            EventClass = eventclass;
            EventDate = eventdate;
            Subject = subject;
        }
        public int DataNo { get; set; }
        public string EventClass { get; set; }
        public string EventDate { get; set; }
        public string Subject { get; set; }
    }

    public class MyDirectorTemplateSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubDirectorRow)item;
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