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
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ObservableCollection<string> Items { get; set; }

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
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

            // 年間例会スケジュールデータ取得
            GetClubSchedule();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// リストタップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void List_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            ClubScheduleRow item = e.Item as ClubScheduleRow;

            Navigation.PushAsync(new ClubSchedulePage(item.DataNo));

            //DisplayAlert("List Tap", "DataNo:" + item.DataNo , "OK");

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
            int WorkDataNo;
            string WorkDate;
            string WorkTitle;
            List<ClubScheduleRow> items = new List<ClubScheduleRow>();

            try
            {
                foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                    "From T_MEETINGSCHEDULE " +
                                                                    "ORDER BY MeetingDate DESC, MeetingTime DESC, DataNo DESC"))
                {
                    WorkDataNo = row.DataNo;
                    WorkDate = row.MeetingDate.Substring(0, 10) + "  " + row.MeetingTime;
                    WorkTitle = row.MeetingName;
                    items.Add(new ClubScheduleRow(WorkDataNo, WorkDate, WorkTitle));
                }
                ClubScheduleListView.ItemsSource = items;
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

}