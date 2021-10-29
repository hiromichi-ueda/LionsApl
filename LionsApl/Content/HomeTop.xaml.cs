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
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ホームTOP画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTop : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // キャビネットレターリスト
        public ObservableCollection<CHomeTopLetter> _letterLt = new ObservableCollection<CHomeTopLetter>();
        // 参加予定リスト
        public ObservableCollection<CHomeTopEvent> _eventLt = new ObservableCollection<CHomeTopEvent>();

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 現在日時
        private DateTime _nowDt;

        // 表示用文字列
        string ST_NOSLOGAN = "地区スローガン情報はありません。";
        string ST_NOLETTER = "キャビネットレター情報はありません。";
        string ST_NOEVENT = "出席予定のイベントはありません。";

        // 定数
        int LETTER_MAXROW = 5;
        int EVENT_MAXROW = 4;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

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

            // 処理日時取得
            _nowDt = DateTime.Now;

            // 画面表示の初期化
            // Slogan
            LabelSlogun.Text = ST_NOSLOGAN;
            LabelDistrictGovernor.Text = "";

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

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
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // 処理日時取得
            _nowDt = DateTime.Now;

            // 参加予定一覧情報をSQLiteファイルから取得して画面に設定する。
            SetEventLt();

            //DisplayAlert("Disp", $"経過時間(分) : {((App)Application.Current).ElapsedTime.TotalMinutes}", "OK");

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
                DisplayAlert("Alert", $"SQLite検索エラー(スローガン) : {ex.Message}", "OK");
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
            //LetterStackLayout.Children.Clear();

            try
            {

                foreach (Table.T_LETTER row in _sqlite.Get_T_LETTER("Select * " +
                                                                    "From T_LETTER " +
                                                                    "ORDER BY EventDate DESC, EventTime DESC, DataNo DESC"))
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
                    HomeTopNoData letterRow = new HomeTopNoData(ST_NOLETTER,
                                                                Device.GetNamedSize(NamedSize.Caption, typeof(Label)));
                    // StackLayoutにコントロールテンプレートを追加
                    LetterStackLayout.Children.Add(letterRow);

                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_LETTER) : {ex.Message}", "OK");
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
            string strClass = string.Empty;                 // イベントクラス用文字列
            string strTitle = string.Empty;                 // タイトル設定用文字列
            string strDate = string.Empty;                  // 月日設定用文字列
            string strCount = string.Empty;                 // 日数設定用文字列
            string strCancel = string.Empty;                // 中止表示用文字列
            string strAnswer = string.Empty;                // 出欠設定用文字列
            string strAnswerDate = string.Empty;            // 回答期限日用文字列
            string strAnswerTime = string.Empty;            // 回答期限時間用文字列

            _eventLt.Clear();
            EventStackLayout.Children.Clear();

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
                                            "t4.Subject, " +
                                            "t2.AnswerDate AS AnswerDateEv, " +
                                            "t3.AnswerDate AS AnswerDateMe, " +
                                            "t3.AnswerTime, " +
                                            "t4.AnswerDate AS AnswerDateDi " +
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
                    strClass = string.Empty;                 // イベントクラス用文字列
                    strTitle = string.Empty;                 // タイトル設定用文字列
                    strDate = string.Empty;                  // 月日設定用文字列
                    strCount = string.Empty;                 // 日数設定用文字列
                    strCancel = string.Empty;                // 中止表示用文字列
                    strAnswer = string.Empty;                // 出欠設定用文字列

                    // イベントリストの各項目値を取得する
                    GetEventListData(row, 
                                     ref intDataNo, 
                                     ref intEventDataNo,
                                     ref strClass, 
                                     ref strDate, 
                                     ref strTitle, 
                                     ref strCount, 
                                     ref strCancel, 
                                     ref strAnswer,
                                     ref strAnswerDate,
                                     ref strAnswerTime);

                    // イベント出欠に回答していない、もしくは欠席の場合
                    if (strAnswer.Equals(_utl.ANSWER_NO) || 
                        strAnswer.Equals(_utl.ANSWER_AB))
                    {
                        // 対象外として次のデータへ
                        continue;
                    }

                    // 回答期限チェック
                    if (_utl.ChkDate(strDate, string.Empty, _nowDt))
                    {
                        // 対象外として次のデータへ
                        continue;
                    }

                    // コントロールテンプレートを作成
                    HomeTopEvent eventRow = new HomeTopEvent(intDataNo,
                                                            strDate.Substring(5, 5),
                                                            strTitle,
                                                            strCount,
                                                            strCancel,
                                                            Device.GetNamedSize(NamedSize.Caption, typeof(Label)));
                    // Labelタップ時の処理追加
                    // ※intDataN･intEventDataNoを指定すると、最後に設定した値しか渡らないので注意！
                    TapGestureRecognizer tgr0 = new TapGestureRecognizer();
                    tgr0.Tapped += (s, e) => { EventRow_Tap(s, e, row.DataNo, row.EventDataNo); };
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
                    HomeTopNoData eventRow = new HomeTopNoData(ST_NOEVENT,
                                                               Device.GetNamedSize(NamedSize.Caption, typeof(Label)));
                    // StackLayoutにコントロールテンプレートを追加
                    EventStackLayout.Children.Add(eventRow);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベントリストの各項目値を取得するして表示用変数に設定する。
        /// </summary>
        /// <param name="row">SQLiteから取得したイベントデータ</param>
        /// <param name="intDataNo">表示対象データのDataNo</param>
        /// <param name="intEventDataNo">表示対象データのEventDataNo</param>
        /// <param name="strDate">日付（表示用変数）</param>
        /// <param name="strTitle">タイトル（表示用変数）</param>
        /// <param name="strCount">日数（表示用変数）</param>
        /// <param name="strCancel">中止</param>
        /// <param name="strAnswer">出欠</param>
        /// <param name="strAnswerDate">回答期限日</param>
        /// <param name="strAnswerTime">回答期限時刻</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventListData(Table.HOME_EVENT row,
                                        ref int intDataNo,
                                        ref int intEventDataNo,
                                        ref string strClass,
                                        ref string strDate,
                                        ref string strTitle,
                                        ref string strCount,
                                        ref string strCancel,
                                        ref string strAnswer,
                                        ref string strAnswerDate,
                                        ref string strAnswerTime)
        {
            DateTime dt = DateTime.Now.Date;
            DateTime wdt;
            string wkDate = string.Empty;
            TimeSpan countTSpn;

            try
            {
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
                    strDate = wkDate.Substring(0, 10);

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
                strClass = _utl.GetString(row.EventClass);
                if (strClass != string.Empty)
                {
                    // 1:キャビネット
                    if (strClass.Equals(_utl.EVENTCLASS_EV))
                    {
                        strTitle = _utl.GetString(row.Title);
                    }
                    // 2:クラブ（例会）
                    else if (strClass.Equals(_utl.EVENTCLASS_ME))
                    {
                        strTitle = _utl.GetString(row.MeetingName);
                    }
                    // 3:クラブ（理事・委員会）
                    else if (strClass.Equals(_utl.EVENTCLASS_DI))
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

                // 回答期限日設定
                if (strClass != string.Empty)
                {
                    // 1:キャビネット
                    if (strClass.Equals(_utl.EVENTCLASS_EV))
                    {
                        strAnswerDate = _utl.GetString(row.AnswerDateEv).Substring(0, 10);
                    }
                    // 2:クラブ（例会）
                    else if (strClass.Equals(_utl.EVENTCLASS_ME))
                    {
                        strAnswerDate = _utl.GetString(row.AnswerDateMe).Substring(0, 10);
                    }
                    // 3:クラブ（理事・委員会）
                    else if (strClass.Equals(_utl.EVENTCLASS_DI))
                    {
                        strAnswerDate = _utl.GetString(row.AnswerDateDi).Substring(0, 10);
                    }
                }

                // 回答期限時間設定（年間例会スケジュールのみ）
                if (strClass.Equals(_utl.EVENTCLASS_ME))
                {
                    strAnswerTime = _utl.GetString(row.AnswerTime);
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"イベント情報取得エラー : {ex.Message}", "OK");
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