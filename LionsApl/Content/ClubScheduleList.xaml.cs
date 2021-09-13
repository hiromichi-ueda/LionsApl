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
    public partial class ClubScheduleList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // リストビュー設定内容
        public List<ClubScheduleRow> Items { get; set; }

        public ClubScheduleList()
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

            // 年間例会スケジュールデータ取得
            GetClubSchedule();

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

            ClubScheduleRow item = e.Item as ClubScheduleRow;

            if (string.IsNullOrEmpty(item.Title))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 年間例会スケジュール画面へ
            Navigation.PushAsync(new ClubSchedulePage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 年間例会スケジュール情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetClubSchedule()
        {
            int WorkDataNo = 0;
            string WorkDate = string.Empty;
            string WorkTitle = string.Empty;
//            List<ClubScheduleRow> items = new List<ClubScheduleRow>();
            Items = new List<ClubScheduleRow>();

            try
            {
                foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                    "From T_MEETINGSCHEDULE " +
                                                                    "ORDER BY MeetingDate DESC, MeetingTime DESC, DataNo DESC"))
                {
                    WorkDataNo = row.DataNo;
                    WorkDate = row.MeetingDate.Substring(0, 10) + "  " + row.MeetingTime;
                    WorkTitle = row.MeetingName;
                    Items.Add(new ClubScheduleRow(WorkDataNo, WorkDate, WorkTitle));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubScheduleRow(0, WorkDate, WorkTitle));
                }
                BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 年間例会ケジュール行情報クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class ClubScheduleRow
    {
        public ClubScheduleRow(int dataNo, string dateTime, string title)
        {
            DataNo = dataNo;
            DateTime = dateTime;
            Title = title;
        }
        public int DataNo { get; set; }
        public string DateTime { get; set; }
        public string Title { get; set; }
    }

    public class MyClubScheduleSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubScheduleRow)item;
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