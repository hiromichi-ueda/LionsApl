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
        // キャビネットレターリスト
        public ObservableCollection<CHomeTopLetter> _letterLt = new ObservableCollection<CHomeTopLetter>();
        // 参加予定リスト
        public ObservableCollection<CHomeTopEvent> _eventLt = new ObservableCollection<CHomeTopEvent>();

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 表示用文字列
        string NoSloganStr = "スローガン情報はありません。";
        string NoLetterStr = "キャビネットレター情報はありません。";
        string NoEventStr = "参加予定のイベントはありません。";

        // 定数
        int LETTER_MAXROW = 5;
        int EVENT_MAXROW = 4;
        string EVENT_CLUSS_CV = "1";
        string EVENT_CLUSS_CL = "2";
        string EVENT_CLUSS_DR = "3";

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public HomeTop()
        {
            InitializeComponent();

            // font-size
            this.LabelSlogun.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.LabelDistrictGovernor.FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label));
            //this.LetterDate0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterTitle0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterMark0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterDate1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterTitle1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterMark1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterDate2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterTitle2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterMark2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterDate3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterTitle3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.LetterMark3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventDate0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventTitle0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventCount0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMsg0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMark0.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventTitle1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventCount1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMsg1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMark1.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventTitle2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventCount2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMsg2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMark2.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventTitle3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventCount3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMsg3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            //this.EventMark3.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            

            // 画面表示の初期化
            // Slogan
            LabelSlogun.Text = NoSloganStr;
            LabelDistrictGovernor.Text = "";
            //// Letter
            //LetterDate0.Text = "";
            //LetterTitle0.Text = "キャビネットレター情報はありません。";
            //LetterDate1.Text = "";
            //LetterTitle1.Text = "";
            //LetterDate2.Text = "";
            //LetterTitle2.Text = "";
            //LetterDate3.Text = "";
            //LetterTitle3.Text = "";
            //// Event
            //EventDate0.Text = "";
            //EventTitle0.Text = "参加予定のイベントはありません。";
            //EventCount0.Text = "";
            //EventMsg0.Text = "";
            //EventDate1.Text = "";
            //EventTitle1.Text = "";
            //EventCount1.Text = "";
            //EventMsg1.Text = "";
            //EventDate2.Text = "";
            //EventTitle2.Text = "";
            //EventCount2.Text = "";
            //EventMsg2.Text = "";
            //EventDate3.Text = "";
            //EventTitle3.Text = "";
            //EventCount3.Text = "";
            //EventMsg3.Text = "";


            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // A_SETTINGデータ取得
            _sqlite.SetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.SetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;


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
        private void LetterRow_Tap(object sender, System.EventArgs e, int dataNo)
        {
            Navigation.PushAsync(new LetterPage(dataNo));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 参加予定一覧情報タップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void EventRow_Tap(object sender, System.EventArgs e, int dataNo, int eventDataNo)
        {
            Navigation.PushAsync(new EventPage(dataNo, eventDataNo));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// スローガン情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetSlogan()
        {

            // データ取得
            try
            {
                foreach (Table.T_SLOGAN row in _sqlite.Get_T_SLOGAN("SELECT * FROM T_SLOGAN"))
                {
                    // スローガン設定
                    if (row.Slogan != null)
                    {
                        LabelSlogun.Text = _utl.GetString(row.Slogan, _utl.NLC_ON);
                    }
                    // ガバナー設定
                    if (row.DistrictGovernor != null)
                    {
                        LabelDistrictGovernor.Text = "地区ガバナー " + _utl.GetString(row.DistrictGovernor);
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

                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "ORDER BY EventDate DESC, EventTime DESC"))
                {
                    // データをリストに追加
                    _letterLt.Add(new CHomeTopLetter(_utl.GetString(row.EventDate), _utl.GetString(row.Title), row.DataNo));

                    // コントロールテンプレートを作成
                    HomeTopLetter letterRow = new HomeTopLetter(row.DataNo,
                                                            _utl.GetString(row.EventDate).Substring(5, 5),
                                                            _utl.GetString(row.Title),
                                                            Device.GetNamedSize(NamedSize.Default, typeof(Label)));
                    // Labelタップ時の処理追加
                    TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                    tgr0.Tapped += (s, e) => { LetterRow_Tap(s, e, row.DataNo); };
                    letterRow.GestureRecognizers.Add(tgr0);

                    // StackLayoutにコントロールテンプレートを追加
                    LetterStackLayout.Children.Add(letterRow);

                    idx++;

                    // MAX行数にて処理終了
                    if (idx >= LETTER_MAXROW)
                    {
                        break;
                    }
                }

                // キャビネットレターがない場合
                if (idx == 0)
                {
                    HomeTopNoData letterRow = new HomeTopNoData(NoLetterStr,
                                                                Device.GetNamedSize(NamedSize.Caption, typeof(Label)));
                    // StackLayoutにコントロールテンプレートを追加
                    LetterStackLayout.Children.Add(letterRow);

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
                                            "t3.MeetingName," +
                                            "t4.Subject " +
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
                                        "LEFT OUTER JOIN " +
                                            "T_DIRECTOR t4 " +
                                        "ON " +
                                            "t1.EventClass = '3' and " +
                                            "t1.EventDataNo = t4.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' " +
                                        "ORDER BY t1.EventDate ASC"))
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

                    // イベント出欠に回答していない場合
                    if (strAnswer == "")
                    {
                        // 対象外として次のデータへ
                        continue;
                    }

                    // コントロールテンプレートを作成
                    HomeTopEvent eventRow = new HomeTopEvent(intDataNo,
                                                            strDate,
                                                            strTitle,
                                                            strCount,
                                                            strCancel,
                                                            Device.GetNamedSize(NamedSize.Caption, typeof(Label)));
                    // Labelタップ時の処理追加
                    TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                    tgr0.Tapped += (s, e) => { EventRow_Tap(s, e, intDataNo, intEventDataNo); };
                    eventRow.GestureRecognizers.Add(tgr0);

                    // StackLayoutにコントロールテンプレートを追加
                    EventStackLayout.Children.Add(eventRow);

                    idx++;

                    // MAX行数にて処理終了
                    if (idx >= EVENT_MAXROW)
                    {
                        break;
                    }

                }

                // 参加イベントがない場合
                if (idx == 0)
                {
                    HomeTopNoData eventRow = new HomeTopNoData(NoEventStr,
                                                               Device.GetNamedSize(NamedSize.Caption, typeof(Label)));
                    // StackLayoutにコントロールテンプレートを追加
                    EventStackLayout.Children.Add(eventRow);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }
        }

        /// <summary>
        /// イベントリストの各項目値を取得するして表示用変数に設定する。
        /// </summary>
        /// <param name="row">SQLiteから取得したイベントデータ</param>
        /// <param name="intDataNo">表示対象データのDataNo</param>
        /// <param name="intEventDataNo">表示対象データのEventDataNo</param>
        /// <param name="strDate">日付（表示用変数）</param>
        /// <param name="strTitle">タイトル（表示用変数）</param>
        /// <param name="strCount">日数（表示用変数）</param>
        /// <param name="strCancel"></param>
        /// <param name="strAnswer"></param>
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
            string wkDate = string.Empty;
            string wkClass = string.Empty;
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
            intEventDataNo = row.EventDataNo;

            // 日時・日数設定
            if (row.EventDate != null)
            {
                wkDate = row.EventDate;
                wdt = DateTime.Parse(wkDate);

                // 日時設定
                strDate = wkDate.Substring(5, 5);

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
            wkClass = _utl.GetString(row.EventClass);
            if (wkClass != string.Empty)
            {
                // 1:キャビネット
                if (wkClass.Equals(EVENT_CLUSS_CV))
                {
                    strTitle = _utl.GetString(row.Title);
                }
                // 2:クラブ（例会）
                else if (wkClass.Equals(EVENT_CLUSS_CL))
                {
                    strTitle = _utl.GetString(row.MeetingName);
                }
                // 3:クラブ（理事・委員会）
                else if (wkClass.Equals(EVENT_CLUSS_DR))
                {
                    strTitle = _utl.GetString(row.Subject);
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

        public sealed class CHomeTopNoData
    {
        public CHomeTopNoData(string labelText)
        {
            LabelText = labelText;
        }
        public string LabelText { get; set; }
    }
}