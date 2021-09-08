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
    public partial class HomeTop : ContentPage
    {

        public ObservableCollection<CHomeTopLetter> _letterLt = new ObservableCollection<CHomeTopLetter>();
        public ObservableCollection<CHomeTopEvent> _eventLt = new ObservableCollection<CHomeTopEvent>();

        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス
        //private Table.A_SETTING _a_setting;                 // A_SETTINGテーブルクラス
        //private Table.A_ACCOUNT _a_account;                 // A_ACCOUNTテーブルクラス
        private Table.T_SLOGAN _t_slogan;                   // T_SLOGANテーブルクラス
        //private Table.T_LETTER _t_letter;                   // T_LETTERテーブルクラス
        //private Table.T_EVENTRET _t_eventret;               // T_EVENTRETテーブルクラス
        //private Table.T_EVENT _t_event;                     // T_EVENTテーブルクラス


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HomeTop()
        {
            InitializeComponent();

            // font-size
            this.LoginInfo.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            this.title_slogan.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LabelSlogun.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LabelDistrictGovernor.FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label));
            this.title_letter.FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label));
            this.LetterDate0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterTitle0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterMark0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterDate1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterTitle1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterMark1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterDate2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterTitle2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterMark2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterDate3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterTitle3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LetterMark3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.title_event.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventDate0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventTitle0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventCount0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMsg0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMark0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventTitle1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventCount1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMsg1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMark1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventTitle2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventCount2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMsg2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMark2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventTitle3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventCount3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMsg3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.EventMark3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.btn_infometion.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            this.btn_magazine.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            this.btn_lcif.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            this.btn_matching.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));


            // 画面表示の初期化
            // Slogan
            LabelSlogun.Text = "スローガン情報はありません。";
            LabelDistrictGovernor.Text = "";
            // Letter
            LetterDate0.Text = "";
            LetterTitle0.Text = "キャビネットレター情報はありません。";
            LetterDate1.Text = "";
            LetterTitle1.Text = "";
            LetterDate2.Text = "";
            LetterTitle2.Text = "";
            LetterDate3.Text = "";
            LetterTitle3.Text = "";
            // Event
            EventDate0.Text = "";
            EventTitle0.Text = "参加予定のイベントはありません。";
            EventCount0.Text = "";
            EventMsg0.Text = "";
            EventDate1.Text = "";
            EventTitle1.Text = "";
            EventCount1.Text = "";
            EventMsg1.Text = "";
            EventDate2.Text = "";
            EventTitle2.Text = "";
            EventCount2.Text = "";
            EventMsg2.Text = "";
            EventDate3.Text = "";
            EventTitle3.Text = "";
            EventCount3.Text = "";
            EventMsg3.Text = "";

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


            // スローガン情報取得
            SetSlogan();

            // キャビネットレター情報取得
            SetLetterLt();

            // 参加予定一覧情報取得
            SetEventLt();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Infomation_Button_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new InfomationList());
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 地区誌ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Magazine_Button_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MagazineList());
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// LCIFボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Lcif_Button_Clicked(object sender, System.EventArgs e)
        {

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// マッチングボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Matching_Button_Clicked(object sender, System.EventArgs e)
        {

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報タップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Letter_Label_Tap(object sender, System.EventArgs e, int listNo)
        {
            Navigation.PushAsync(new LetterPage(_letterLt[listNo].LetterTitle, _letterLt[listNo].LetterDataNo));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 参加予定一覧情報タップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Event_Label_Tap(object sender, System.EventArgs e, int listNo)
        {
            Navigation.PushAsync(new EventPage(_eventLt[listNo].EventTitle, _eventLt[listNo].DataNo, _eventLt[listNo].EventDataNo));
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// スローガン情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetSlogan()
        {
            _t_slogan = null;

            // データ取得
            try
            {
                foreach (Table.T_SLOGAN row in _sqlite.Get_T_SLOGAN("SELECT * FROM T_SLOGAN"))
                {
                    //_t_slogan = new Table.T_SLOGAN
                    //{
                    //    DataNo = row.DataNo,
                    //    FiscalStart = row.FiscalStart,
                    //    FiscalEnd = row.FiscalEnd,
                    //    Slogan = row.Slogan,
                    //    DistrictGovernor = row.DistrictGovernor
                    //};
                    // スローガン設定
                    if (row.Slogan != null)
                    {
                        LabelSlogun.Text = row.Slogan;
                    }
                    // ガバナー設定
                    if (row.DistrictGovernor != null)
                    {
                        LabelDistrictGovernor.Text = "地区ガバナー " + row.DistrictGovernor;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(スローガン) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットレター情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetLetterLt()
        {
            int idx = 0;
            _letterLt.Clear();

            try
            {

                //foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select " +
                //                                                        "strftime('%m/%d', EventDate, 'localtime'), " +
                //                                                        "Title " +
                //                                                    "From T_LETTER " +
                //                                                    "ORDER BY EventDate DESC, EventTime DESC"))
                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "ORDER BY EventDate DESC, EventTime DESC"))
                {
                    _letterLt.Add(new CHomeTopLetter(row.EventDate, row.Title, row.DataNo));
                    if (idx == 0)
                    {
                        // 月日設定
                        LetterDate0.Text = row.EventDate.Substring(5, 5);
                        // タイトル設定
                        LetterTitle0.Text = row.Title;
                        // 画面遷移記号設定
                        LetterMark0.Text = "〉";

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                        tgr0.Tapped += (s, e) => { Letter_Label_Tap(s, e, 0); };
                        LetterDate0.GestureRecognizers.Add(tgr0);
                        LetterTitle0.GestureRecognizers.Add(tgr0);
                        LetterMark0.GestureRecognizers.Add(tgr0);

                        idx++;
                    }
                    else if (idx == 1)
                    {
                        // 月日設定
                        LetterDate1.Text = row.EventDate.Substring(5, 5);
                        // タイトル設定
                        LetterTitle1.Text = row.Title;
                        // 画面遷移記号設定
                        LetterMark1.Text = "〉";

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr1 = new TapGestureRecognizer();
                        tgr1.Tapped += (s, e) => { Letter_Label_Tap(s, e, 1); };
                        LetterDate1.GestureRecognizers.Add(tgr1);
                        LetterTitle1.GestureRecognizers.Add(tgr1);
                        LetterMark1.GestureRecognizers.Add(tgr1);
                        idx++;
                    }
                    else if (idx == 2)
                    {
                        // 月日設定
                        LetterDate2.Text = row.EventDate.Substring(5, 5);
                        // タイトル設定
                        LetterTitle2.Text = row.Title;
                        // 画面遷移記号設定
                        LetterMark2.Text = "〉";

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr2 = new TapGestureRecognizer();
                        tgr2.Tapped += (s, e) => { Letter_Label_Tap(s, e, 2); };
                        LetterDate2.GestureRecognizers.Add(tgr2);
                        LetterTitle2.GestureRecognizers.Add(tgr2);
                        LetterMark2.GestureRecognizers.Add(tgr2);
                        idx++;
                    }
                    else if (idx == 3)
                    {
                        // 月日設定
                        LetterDate3.Text = row.EventDate.Substring(5, 5);
                        // タイトル設定
                        LetterTitle3.Text = row.Title;
                        // 画面遷移記号設定
                        LetterMark3.Text = "〉";

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr3 = new TapGestureRecognizer();
                        tgr3.Tapped += (s, e) => { Letter_Label_Tap(s, e, 3); };
                        LetterDate3.GestureRecognizers.Add(tgr3);
                        LetterTitle3.GestureRecognizers.Add(tgr3);
                        LetterMark3.GestureRecognizers.Add(tgr3);
                        idx++;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 参加予定一覧情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetEventLt()
        {
            int idx = 0;
            //DateTime dt = DateTime.Now;
            //DateTime wdt;
            //TimeSpan countTSpn;
            //string _nowDate = dt.ToString("yyyy/MM/dd");
            int intDataNo = 0;                              // データNo.設定用
            int intEventDataNo = 0;                         // イベントデータNo.設定用
            string strTitle = "";                           // タイトル設定用文字列
            string strDate = "";                            // 月日設定用文字列
            string strCount = "";                           // 日数設定用文字列
            string strCancel = "";                          // 中止表示用文字列
            string strAnswer = "";                          // 出欠設定用文字列

            _eventLt.Clear();
            try
            {
                foreach (Table.HOME_EVENT row in _sqlite.Get_HOME_EVENT(
                                        "SELECT " +
                                            "t1.DataNo, " +
                                            "t1.EventClass, " +
                                            "t1.EventDataNo, " +
                                            "t1.EventDate, " +
                                            "t1.ClubCode, " +
                                            "t1.MemberCode, " +
                                            "t1.Answer, " +
                                            "t1.CancelFlg, " +
                                            "t2.Title, " +
                                            "t3.MeetingName " +
                                        "FROM " +
                                            "T_EVENTRET t1 " +
                                        "LEFT OUTER JOIN " +
                                            "T_EVENT t2 " +
                                        "ON " +
                                            "t1.EventClass = '1' and " +
                                            "t1.EventDataNo = t2.DataNo " +
                                        "LEFT OUTER JOIN " +
                                            "T_MEETINGSCHEDULE t3 " +
                                        "ON " +
                                            "t1.EventClass = '2' and " +
                                            "t1.EventDataNo = t3.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "'"))
                                            //"t1.MemberCode = '" + _a_account.MemberCode + "'"))
                                            //"t1.MemberCode = '" + _a_account.MemberCode + "' AND " +
                                            //"t1.EventDate >= '" + _nowDate + "' AND " +
                                            //"(t1.Answer <> '2')"))
                {
                    intDataNo = 0;                           // データNo.設定用
                    intEventDataNo = 0;                      // イベントデータNo.設定用
                    strTitle = "";                           // タイトル設定用文字列
                    strDate = "";                            // 月日設定用文字列
                    strCount = "";                           // 日数設定用文字列
                    strCancel = "";                          // 中止表示用文字列
                    strAnswer = "";                          // 出欠設定用文字列

                    // イベントリストの各項目値を取得する
                    GetEventListData(row, 
                                     ref intDataNo, 
                                     ref intEventDataNo,
                                     ref strDate, 
                                     ref strTitle, 
                                     ref strCount, 
                                     ref strCancel, 
                                     ref strAnswer);

                    if (strAnswer == "")
                    {
                        continue;
                    }

                    if (idx == 0)
                    {
                        // 月日設定
                        EventDate0.Text = strDate;
                        // タイトル設定
                        EventTitle0.Text = strTitle;
                        // 日数設定
                        EventCount0.Text = strCount;
                        // 中止設定
                        EventMsg0.Text = strCancel;
                        // 画面遷移記号設定
                        EventMark0.Text = "〉";

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                        tgr0.Tapped += (s, e) => { Event_Label_Tap(s, e, 0); };
                        EventDate0.GestureRecognizers.Add(tgr0);
                        EventTitle0.GestureRecognizers.Add(tgr0);
                        EventMsg0.GestureRecognizers.Add(tgr0);
                        EventMark0.GestureRecognizers.Add(tgr0);

                        idx++;
                    }
                    else if (idx == 1)
                    {
                        // 月日設定
                        EventDate1.Text = strDate;
                        // タイトル設定
                        EventTitle1.Text = strTitle;
                        // 日数設定
                        EventCount1.Text = strCount;
                        // 中止設定
                        EventMsg1.Text = strCancel;
                        // 画面遷移記号設定
                        EventMark1.Text = "〉";

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                        tgr0.Tapped += (s, e) => { Event_Label_Tap(s, e, 0); };
                        EventDate1.GestureRecognizers.Add(tgr0);
                        EventTitle1.GestureRecognizers.Add(tgr0);
                        EventMsg1.GestureRecognizers.Add(tgr0);
                        EventMark1.GestureRecognizers.Add(tgr0);

                        idx++;
                    }
                    else if (idx == 2)
                    {
                        // 月日設定
                        EventDate2.Text = strDate;
                        // タイトル設定
                        EventTitle2.Text = strTitle;
                        // 日数設定
                        EventCount2.Text = strCount;
                        // 中止設定
                        EventMsg2.Text = strCancel;
                        // 画面遷移記号設定
                        EventMark2.Text = "〉";
                        idx++;

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                        tgr0.Tapped += (s, e) => { Event_Label_Tap(s, e, 0); };
                        EventDate2.GestureRecognizers.Add(tgr0);
                        EventTitle2.GestureRecognizers.Add(tgr0);
                        EventMsg2.GestureRecognizers.Add(tgr0);
                        EventMark2.GestureRecognizers.Add(tgr0);
                    }
                    else if (idx == 3)
                    {
                        // 月日設定
                        EventDate3.Text = strDate;
                        // タイトル設定
                        EventTitle3.Text = strTitle;
                        // 日数設定
                        EventCount3.Text = strCount;
                        // 中止設定
                        EventMsg3.Text = strCancel;
                        // 画面遷移記号設定
                        EventMark3.Text = "〉";
                        idx++;

                        // Labelタップ時の処理追加
                        TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                        tgr0.Tapped += (s, e) => { Event_Label_Tap(s, e, 0); };
                        EventDate3.GestureRecognizers.Add(tgr0);
                        EventTitle3.GestureRecognizers.Add(tgr0);
                        EventMsg3.GestureRecognizers.Add(tgr0);
                        EventMark3.GestureRecognizers.Add(tgr0);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }
        }

        private void GetEventListData(Table.HOME_EVENT row, 
                                        ref int intDataNo,
                                        ref int intEventDataNo,
                                        ref string strDate, 
                                        ref string strTitle, 
                                        ref string strCount, 
                                        ref string strCancel, 
                                        ref string strAnswer)
        {
            DateTime dt = DateTime.Now;
            DateTime wdt;
            string wstrDate = "";
            TimeSpan countTSpn;

            // データNo.設定
            if (row.DataNo == 0)
            {
                return;
            }
            else
            {
                intDataNo = row.DataNo;
            }

            // イベントNo.設定
            if (row.EventDataNo != 0)
            {
                intEventDataNo = row.EventDataNo;
            }

            // 日時・日数設定
            if (row.EventDate != null)
            {
                wstrDate = row.EventDate;
                wdt = DateTime.Parse(wstrDate);

                // 日時設定
                strDate = wstrDate.Substring(5, 5);

                // 日数設定
                countTSpn = wdt - dt;

                if (countTSpn.Days == 0)
                {
                    strCount = "本日";
                }
                else if (countTSpn.Days > 0)
                {
                    strCount = countTSpn.Days.ToString() + "日前";
                }
                else
                {
                    strCount = countTSpn.Days.ToString() + "日前";
                    //strCount = "日時エラー";
                }
            }

            // タイトル設定
            if (row.EventClass != null)
            {
                // 1:キャビネット
                if (row.EventClass.Equals("1"))
                {
                    if (row.Title != null)
                    {
                        strTitle = row.Title;
                    }
                }
                // 2:クラブ（例会）
                else if (row.EventClass.Equals("2"))
                {
                    if (row.MeetingName != null)
                    {
                        strTitle = row.MeetingName;
                    }
                }
                // 3:クラブ（理事・委員会）
                else if (row.EventClass.Equals("3"))
                {
                    if (row.Title != null)
                    {
                        strTitle = row.Title;
                    }
                }
            }

            // 中止設定
            if (row.CancelFlg != null)
            {
                if (row.CancelFlg.Equals("1"))
                {
                    strCancel = "中止";
                }
            }

            // 出欠設定
            if (row.Answer != null)
            {
                strAnswer = row.Answer;

                // Null以外の時のみ表示
                _eventLt.Add(new CHomeTopEvent(intDataNo, intEventDataNo, strDate, strTitle, strCount, strCancel, strAnswer));
            }


        }

    }

    public sealed class CHomeTopSlogan
    {
        public CHomeTopSlogan(string slogan, string districtGovernor)
        {
            Slogan = slogan;
            DistrictGovernor = districtGovernor;
        }
        public string Slogan { get; set; }
        public string DistrictGovernor { get; set; }
    }

    public sealed class CHomeTopLetter
    {
        public CHomeTopLetter(string letterDate, string letterTitle, int letterDataNo)
        {
            LetterDate = letterDate;
            LetterTitle = letterTitle;
            LetterDataNo = letterDataNo;
        }
        public string LetterDate { get; set; }
        public string LetterTitle { get; set; }
        public int LetterDataNo { get; set; }
    }

    public sealed class CHomeTopEvent
    {
        public CHomeTopEvent(int dataNo, int eventDataNo, string eventDate, string eventTitle, string eventCount, string eventCancel, string answer)
        {
            DataNo = dataNo;
            EventDataNo = eventDataNo;
            EventDate = eventDate;
            EventTitle = eventTitle;
            EventCount = eventCount;
            EventCancel = eventCancel;
            Answer = answer;
        }
        public int DataNo { get; set; }
        public int EventDataNo { get; set; }
        public string EventDate { get; set; }
        public string EventTitle { get; set; }
        public string EventCount { get; set; }
        public string EventCancel { get; set; }
        public string Answer { get; set; }
    }

}