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
        private Table.A_SETTING _a_setting;                 // A_SETTINGテーブルクラス
        private Table.A_ACCOUNT _a_account;                 // A_ACCOUNTテーブルクラス
        private Table.T_SLOGAN _t_slogan;                   // T_SLOGANテーブルクラス
        private Table.T_LETTER _t_letter;                   // T_LETTERテーブルクラス
        private Table.T_EVENTRET _t_eventret;               // T_EVENTRETテーブルクラス
        private Table.T_EVENT _t_event;                     // T_EVENTテーブルクラス


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HomeTop()
        {
            InitializeComponent();

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

            // 設定ファイル情報取得
            SetSetting();

            // アカウント情報取得
            SetAccount();

            // スローガン情報取得
            SetSlogan();

            // キャビネットレター情報取得
            SetLetterLt();

            // 参加予定一覧情報取得
            SetEventLt();

            // Letter
            //LetterDate0.Text = "01/01";
            //LetterTitle0.Text = "レター１";
            //LetterDate1.Text = "02/02";
            //LetterTitle1.Text = "レター２";
            //LetterDate2.Text = "03/03";
            //LetterTitle2.Text = "レター３";
            //LetterDate3.Text = "04/04";
            //LetterTitle3.Text = "レター４";
            // Event
            //EventDate0.Text = "01/01";
            //EventTitle0.Text = "イベント１";
            //EventCount0.Text = "(本日)";
            //EventMsg0.Text = "    ";
            //EventDate1.Text = "02/02";
            //EventTitle1.Text = "イベント２";
            //EventCount1.Text = "(7日前)";
            //EventMsg1.Text = "    ";
            //EventDate2.Text = "03/03";
            //EventTitle2.Text = "イベント３";
            //EventCount2.Text = "(30日前)";
            //EventMsg2.Text = "    ";
            //EventDate3.Text = "04/04";
            //EventTitle3.Text = "イベント４";
            //EventCount3.Text = "(120日前)";
            //EventMsg3.Text = "    ";

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 参加・不参加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Eventret_Button_Clicked(object sender, System.EventArgs e)
        {

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
        /// Activityボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Activity_Button_Clicked(object sender, System.EventArgs e)
        {

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Activityボタン押下
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
        /// 設定ファイル情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetSetting()
        {
            _a_setting = null;

            // データ取得
            try
            {
                foreach (Table.A_SETTING row in _sqlite.Get_A_SETTING("SELECT * FROM A_SETTING"))
                {
                    _a_setting = new Table.A_SETTING
                    {
                        DistrictCode = row.DistrictCode,
                        DistrictName = row.DistrictName,
                        CabinetName = row.CabinetName,
                        PeriodStart = row.PeriodStart,
                        PeriodEnd = row.PeriodEnd,
                        DistrictID = row.DistrictID,
                        MagazineMoney = row.MagazineMoney,
                        EventDataDay = row.EventDataDay
                    };

                }
                Title = _a_setting.CabinetName;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(セッティング) : &{ex.Message}", "OK");
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetAccount()
        {
            _a_account = null;

            // データ取得
            try
            {
                foreach (Table.A_ACCOUNT row in _sqlite.Get_A_ACCOUNT("SELECT * FROM A_ACCOUNT"))
                {
                    _a_account = new Table.A_ACCOUNT
                    {
                        Region = row.Region,
                        Zone = row.Zone,
                        ClubCode = row.ClubCode,
                        ClubName = row.ClubName,
                        MemberCode = row.MemberCode,
                        MemberFirstName = row.MemberFirstName,
                        MemberLastName = row.MemberLastName,
                        AccountDate = row.AccountDate
                    };

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(アカウント) : &{ex.Message}", "OK");
            }
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
                    _t_slogan = new Table.T_SLOGAN
                    {
                        DataNo = row.DataNo,
                        FiscalStart = row.FiscalStart,
                        FiscalEnd = row.FiscalEnd,
                        Slogan = row.Slogan,
                        DistrictGovernor = row.DistrictGovernor
                    };

                }
                LabelSlogun.Text = _t_slogan.Slogan;
                LabelDistrictGovernor.Text = "地区ガバナー " + _t_slogan.DistrictGovernor;
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
            DateTime dt = new DateTime(2003,1,1,0,0,0);     
            DateTime wdt;
            TimeSpan countTSpn;
            string _nowDate = dt.ToString("yyyy/MM/dd");
            string strTitle = "";                           // タイトル設定用文字列
            string strDate = "";                            // 月日設定用文字列
            string strCount = "";                           // 日数設定用文字列
            string strCancel = "";                          // 中止表示用文字列

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
                                            "t1.MemberCode = '" + _a_account.MemberCode + "'"))
                                            //"t1.MemberCode = '" + _a_account.MemberCode + "' AND " +
                                            //"t1.EventDate >= '" + _nowDate + "' AND " +
                                            //"(t1.Answer <> '2')"))
                {
                    // タイトル設定
                    // 1:キャビネット
                    if (row.EventClass.Equals("1"))
                    {
                        _eventLt.Add(new CHomeTopEvent(row.EventDate, row.Title, "0"));
                        strDate = row.EventDate;
                        strTitle = row.Title;
                        //strCount = row.CountDate.ToString();
                    }
                    // 2:クラブ（例会）
                    else if (row.EventClass.Equals("2"))
                    {
                        _eventLt.Add(new CHomeTopEvent(row.EventDate, row.Title, "0"));
                        strDate = row.EventDate;
                        strTitle = row.MeetingName;
                        //strCount = row.CountDate.ToString();
                    }
                    // 3:クラブ（理事・委員会）
                    else if (row.EventClass.Equals("3"))
                    {
                        _eventLt.Add(new CHomeTopEvent(row.EventDate, row.Title, "0"));
                        strDate = row.EventDate;
                        strTitle = row.Title;
                        //strCount = row.CountDate.ToString();
                    }

                    // 日数設定
                    wdt = DateTime.Parse(strDate);
                    countTSpn = wdt - dt;
                    strCount = countTSpn.Days.ToString();
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
                        strCount = "日時エラー";
                    }

                    // 中止設定
                    if (row.CancelFlg.Equals("1"))
                    {
                        strCancel = "中止";
                    }

                    if (idx == 0)
                    {
                        EventDate0.Text = strDate.Substring(5, 5);
                        EventTitle0.Text = strTitle;
                        EventCount0.Text = strCount;
                        EventMsg0.Text = strCancel;
                        EventMark0.Text = ">";
                        idx++;
                    }
                    else if (idx == 1)
                    {
                        EventDate1.Text = strDate.Substring(5, 5);
                        EventTitle1.Text = strTitle;
                        EventCount1.Text = strCount;
                        EventMsg1.Text = strCancel;
                        EventMark1.Text = ">";
                        idx++;
                    }
                    else if (idx == 2)
                    {
                        EventDate2.Text = strDate.Substring(5, 5);
                        EventTitle2.Text = strTitle;
                        EventCount2.Text = strCount;
                        EventMsg2.Text = strCancel;
                        EventMark2.Text = ">";
                        idx++;
                    }
                    else if (idx == 3)
                    {
                        EventDate3.Text = strDate.Substring(5, 5);
                        EventTitle3.Text = strTitle;
                        EventCount3.Text = strCount;
                        EventMsg3.Text = strCancel;
                        EventMark3.Text = ">";
                        idx++;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
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
        public CHomeTopEvent(string eventDate, string eventTitle, string eventCount)
        {
            EventDate = eventDate;
            EventTitle = eventTitle;
            EventCount = eventCount;
        }
        public string EventDate { get; set; }
        public string EventTitle { get; set; }
        public string EventCount { get; set; }
    }

    //public class HomeTopTemplateSelector : DataTemplateSelector
    //{
    //    DataTemplate sloganTemp;
    //    DataTemplate letterTemp;
    //    DataTemplate eventTemp;

    //    public HomeTopTemplateSelector()
    //    {
    //        sloganTemp = new DataTemplate(typeof(SloganCell));
    //        letterTemp = new DataTemplate(typeof(LetterCell));
    //        eventTemp = new DataTemplate(typeof(EventCell));
    //    }

    //    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    //    {
    //        if (item is SloganCellClass)
    //            return sloganTemp;

    //        if (item is LetterCellClass)
    //            return letterTemp;

    //        if (item is EventCellClass)
    //            return eventTemp;

    //        //throw new NotImplementedException();
    //        throw new Exception("Could not find the Hometop.");
    //    }
    //}


}