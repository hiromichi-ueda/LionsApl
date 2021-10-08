using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 年間例会スケジュール一覧クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubScheduleList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // リストビュー設定内容
        public List<ClubScheduleRow> Items { get; set; }
        //public ObservableCollection<ClubScheduleRow> Items;

        // 表示定数
        private readonly string CancelStr = "中止";

        public ClubScheduleList()
        {
            InitializeComponent();

            // Content Utilクラス生成
            _utl = new LAUtility();

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

            if (item.DataNo == 0)
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
            string WorkCancel = string.Empty;
            string nowFiscal = string.Empty;
            Items = new List<ClubScheduleRow>();
            //Items = new ObservableCollection<ClubScheduleRow>();

            // 処理日時取得
            DateTime nowDt = DateTime.Now;
            // 年度取得
            nowFiscal = _sqlite.GetFiscal(nowDt.ToString("yyyy/MM/dd"));

            try
            {
                foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                    "From T_MEETINGSCHEDULE " +
                                                                    "Where Fiscal = '" + nowFiscal + "' " +
                                                                    "ORDER BY MeetingDate ASC, MeetingTime ASC"))
                {
                    WorkDataNo = row.DataNo;
                    WorkDate = _utl.GetString(row.MeetingDate).Substring(0, 10) + "  " + _utl.GetString(row.MeetingTime);
                    WorkCancel = "";
                    if (_utl.GetString(row.CancelFlg) == "1")
                    {
                        WorkCancel = CancelStr;
                    }
                    WorkTitle = _utl.GetString(row.MeetingName);
                    Items.Add(new ClubScheduleRow(WorkDataNo, WorkDate, WorkCancel, WorkTitle));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubScheduleRow(WorkDataNo, WorkDate, WorkCancel, WorkTitle));
                }
                BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : {ex.Message}", "OK");
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

        public ClubScheduleRow(int dataNo, string dateTime, string cancel, string title)
        {
            DataNo = dataNo;
            DateTime = dateTime;
            CancelFlg = cancel;
            Title = title;
        }
        public int DataNo { get; set; }
        public string DateTime { get; set; }
        public string CancelFlg { get; set; }
        public string Title { get; set; }
    }
    //public sealed class ClubScheduleRow : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private int _dataNo = 0;
    //    private string _dateTime = string.Empty;
    //    private string _cancelFlg = string.Empty;
    //    private string _title = string.Empty;

    //    public ClubScheduleRow(int dataNo, string dateTime, string cancelFlg, string title)
    //    {
    //        DataNo = dataNo;
    //        DateTime = dateTime;
    //        CancelFlg = cancelFlg;
    //        Title = title;
    //    }

    //    public int DataNo 
    //    {
    //        get { return _dataNo; }
    //        set 
    //        { 
    //            if (_dataNo != value)
    //            {
    //                _dataNo = value;
    //                if (PropertyChanged != null)
    //                {
    //                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DataNo)));
    //                }
    //            }
    //        } 
    //    }
    //    public string DateTime
    //    {
    //        get { return _dateTime; }
    //        set 
    //        {
    //            if (_dateTime != value)
    //            {
    //                _dateTime = value;
    //                if (PropertyChanged != null)
    //                {
    //                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(DateTime)));
    //                }
    //            }
    //        }
    //    }
    //    public string CancelFlg 
    //    {
    //        get { return _cancelFlg; }
    //        set
    //        {
    //            if (_cancelFlg != value)
    //            {
    //                _cancelFlg = value;
    //                if (PropertyChanged != null)
    //                {
    //                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(CancelFlg)));
    //                }
    //            }
    //        }
    //    }
    //    public string Title 
    //    {
    //        get { return _title; }
    //        set 
    //        {
    //            if (_title != value)
    //            {
    //                _title = value;
    //                if (PropertyChanged != null)
    //                {
    //                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Title)));
    //                }
    //            }
    //        }
    //    }
    //}

    public class MyClubScheduleSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubScheduleRow)item;
            if (info.DataNo != 0)
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