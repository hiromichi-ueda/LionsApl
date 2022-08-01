using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 出欠確認一覧クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 情報通信マネージャクラス
        private IComManager _icom;

        // T_BADGE登録クラス
        private CBADGE _cbadge;

        // リストビュー設定内容
        public ObservableCollection<EventRow> Items;

        // 文字列定数
        //private string ST_EVENT_1 = "[キャビネット]";
        private string ST_EVENT_1 = "[キャビ]";
        private string ST_EVENT_2 = "[クラブ]";
        private string ST_EVENT_3 = "[クラブ]";

        //private string ANSWER_NO = "未登録";
        //public const string ST_ANSWER_NO = "未回答";
        //public const string ST_ANSWER_1 = "出席";
        //public const string ST_ANSWER_2 = "欠席";

        private string ST_CLUSEVENT_1 = "[理事会]";
        private string ST_CLUSEVENT_2 = "[委員会]";
        private string ST_CLUSEVENT_3 = "[その他]";

        private string ST_CANCEL = "中止";


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public EventList()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // 情報通信マネージャー生成
            _icom = IComManager.GetInstance(_sqlite.dbFile);

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 未読情報クラス生成
            _cbadge = new CBADGE("", 0, "", "");

            // イベント情報データ取得
            GetEventData();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // イベント情報データ更新
            UpdEventData();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            EventRow item = e.Item as EventRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (item.DataNo == 0)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 未読の場合は既読にする
            if (item.Badge.Equals(LADef.ST_BADGE))
            {
                // 未読情報のキーを設定
                _cbadge.DataClass = _utl.DATACLASS_EV;
                _cbadge.DataNo = item.DataNo;
                _cbadge.ClubCode = _sqlite.Db_A_Account.ClubCode;
                _cbadge.MemberCode = _sqlite.Db_A_Account.MemberCode;

                // 未読情報をコンテンツに設定
                _icom.SetContentToBADGE(_cbadge);
                try
                {
                    // SQLServerへ削除
                    Task<HttpResponseMessage> response = _icom.AsyncPostTextForWebAPI();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"SQLServer 未読情報削除エラー : {ex.Message}", "OK");
                }

                try
                {
                    // SQLiteへ削除
                    SetBadgeSQlite();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"SQLite 未読情報削除エラー : {ex.Message}", "OK");
                }

                // タブページのバッジ更新
                ((MainTabPage)((App)Application.Current).TabPage).SetBadgeInfo();
            }

            // 出欠確認画面へ
            await Navigation.PushAsync(new EventPage(item.DataNo, item.EventDataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventData()
        {
            int intDataNo = 0;                              // データNo.設定用
            int intEventDataNo = 0;                         // イベントデータNo.設定用
            string strDate = "";                            // 月日設定用文字列
            string strCancel = "";                          // 中止表示用文字列
            string strTitle = "";                           // タイトル設定用文字列
            string strAnswer = "";                          // 出欠設定用文字列
            Color colAnswer = Color.Default;                // 出欠設定文字色用値
            string strBadge = "";                           // 未読設定用文字列
            Items = new ObservableCollection<EventRow>();

            try
            {
                foreach (Table.EVENT_LIST row in _sqlite.Get_EVENT_LIST(
                                        "SELECT " +
                                            "t1.DataNo AS DataNo, " +
                                            "t1.EventClass AS EventClass, " +
                                            "t1.EventDataNo AS EventDataNo, " +
                                            "t1.EventDate AS EventDate, " +
                                            "t1.ClubCode AS ClubCode, " +
                                            "t1.MemberCode AS MemberCode, " +
                                            "t1.Answer AS Answer, " +
                                            "t1.CancelFlg AS CancelFlg, " +
                                            "t2.EventPlace AS EventPlace, " +
                                            "t2.Title AS Title, " +
                                            "t3.MeetingName AS MeetingName," +
                                            "t3.MeetingPlace AS MeetingPlace," +
                                            "t4.Subject AS Subject," +
                                            "t4.EventClass AS ClubEventClass, " +
                                            "t4.EventPlace AS ClubEventPlace, " +
                                            "t5.DataNo AS Badge " +
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
                                        "LEFT OUTER JOIN " +
                                            "T_BADGE t5 " +
                                        "ON " +
                                            "t5.DataClass = '1' and " +
                                            "t1.EventDataNo = t5.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' " +
                                        "ORDER BY t1.EventDate DESC, t1.DataNo DESC"))


                {
                    intDataNo = 0;                           // データNo.設定用
                    intEventDataNo = 0;                      // イベントデータNo.設定用
                    strDate = "";                            // 月日設定用文字列
                    strCancel = "";                          // 中止表示用文字列
                    strTitle = "";                           // タイトル設定用文字列
                    strAnswer = "";                          // 出欠設定用文字列
                    colAnswer = Color.Default;               // 出欠設定文字色用値
                    strBadge = "";                           // 未読設定用文字列

                    // イベントリストの各項目値を取得する
                    GetEventListData(row,
                                     ref intDataNo,
                                     ref intEventDataNo,
                                     ref strDate,
                                     ref strCancel,
                                     ref strTitle,
                                     ref strAnswer,
                                     ref colAnswer,
                                     ref strBadge);

                    // イベントリスト行クラスを作成する。
                    EventRow eventRow = new EventRow(intDataNo, intEventDataNo, strDate, strCancel, strTitle, strAnswer, colAnswer, strBadge);
                    Items.Add(eventRow);
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new EventRow(0, intEventDataNo, strDate, strCancel, strTitle, strAnswer, colAnswer, strBadge));
                }
                EventListView.ItemsSource = Items;
                this.BindingContext = this;

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント情報をSQLiteファイルから取得して画面に設定する。(更新用)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public void UpdEventData()
        {
            int intDataNo = 0;                              // データNo.設定用
            int intEventDataNo = 0;                         // イベントデータNo.設定用
            string strDate = "";                            // 月日設定用文字列
            string strCancel = "";                          // 中止表示用文字列
            string strTitle = "";                           // タイトル設定用文字列
            string strAnswer = "";                          // 出欠設定用文字列
            Color colAnswer = Color.Default;                // 出欠設定文字色用値
            string strBadge = "";                           // 未読設定用文字列
            int idx = 0;

            try
            {
                foreach (Table.EVENT_LIST row in _sqlite.Get_EVENT_LIST(
                                        "SELECT " +
                                            "t1.DataNo AS DataNo, " +
                                            "t1.EventClass AS EventClass, " +
                                            "t1.EventDataNo AS EventDataNo, " +
                                            "t1.EventDate AS EventDate, " +
                                            "t1.ClubCode AS ClubCode, " +
                                            "t1.MemberCode AS MemberCode, " +
                                            "t1.Answer AS Answer, " +
                                            "t1.CancelFlg AS CancelFlg, " +
                                            "t2.EventPlace AS EventPlace, " +
                                            "t2.Title AS Title, " +
                                            "t3.MeetingName AS MeetingName," +
                                            "t3.MeetingPlace AS MeetingPlace," +
                                            "t4.Subject AS Subject," +
                                            "t4.EventClass AS ClubEventClass, " +
                                            "t4.EventPlace AS ClubEventPlace, " +
                                            "t5.DataNo AS Badge " +
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
                                        "LEFT OUTER JOIN " +
                                            "T_BADGE t5 " +
                                        "ON " +
                                            "t5.DataClass = '1' and " +
                                            "t1.DataNo = t5.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' " +
                                        "ORDER BY t1.EventDate DESC, t1.DataNo DESC"))


                {
                    intDataNo = 0;                           // データNo.設定用
                    intEventDataNo = 0;                      // イベントデータNo.設定用
                    strDate = "";                            // 月日設定用文字列
                    strCancel = "";                          // 中止表示用文字列
                    strTitle = "";                           // タイトル設定用文字列
                    strAnswer = "";                          // 出欠設定用文字列
                    strBadge = "";                           // 未読設定用文字列

                    // イベントリストの各項目値を取得する
                    GetEventListData(row,
                                     ref intDataNo,
                                     ref intEventDataNo,
                                     ref strDate,
                                     ref strCancel,
                                     ref strTitle,
                                     ref strAnswer,
                                     ref colAnswer,
                                     ref strBadge);

                    // 出欠を設定
                    Items[idx].Answer = strAnswer;
                    // 文字色を設定
                    Items[idx].AnswerColor = colAnswer;
                    // 未読を設定
                    Items[idx].Badge = strBadge;

                    idx++;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"UpdateSQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : {ex.Message}", "OK");
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
        /// <param name="strCancel">キャンセル（表示用変数）</param>
        /// <param name="strTitle">タイトル（表示用変数）</param>
        /// <param name="strAnswer">回答（表示用変数）</param>
        /// <param name="colAnswer">回答文字色（表示用変数）</param>
        /// <param name="strBadge">未読（表示用変数）</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventListData(Table.EVENT_LIST row,
                                        ref int intDataNo,
                                        ref int intEventDataNo,
                                        ref string strDate,
                                        ref string strCancel,
                                        ref string strTitle,
                                        ref string strAnswer,
                                        ref Color colAnswer,
                                        ref string strBadge)
        {
            string wkEveClass = string.Empty;
            string wkClubEveClass = string.Empty;
            string wkAnswer = string.Empty;
            string wkBadge = string.Empty;

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

            // 日時設定
            strDate = _utl.GetString(row.EventDate).Substring(0, 10);

            // イベント区分
            wkEveClass = _utl.GetString(row.EventClass);
            // 1:キャビネット
            if (wkEveClass.Equals(LADef.EVENTCLASS_EV))
            {
                strDate = strDate + " " + ST_EVENT_1;
            }
            // 2:クラブ（例会）
            else if (wkEveClass.Equals(LADef.EVENTCLASS_ME))
            {
                strDate = strDate + " " + ST_EVENT_2;
            }
            // 3:クラブ（理事・委員会）
            else if (wkEveClass.Equals(LADef.EVENTCLASS_DI))
            {
                strDate = strDate + " " + ST_EVENT_3;
            }

            // タイトル設定
            // 1:キャビネット
            if (wkEveClass.Equals(LADef.EVENTCLASS_EV))
            {
                strTitle = _utl.GetString(row.Title);
            }
            // 2:クラブ（例会）
            else if (wkEveClass.Equals(LADef.EVENTCLASS_ME))
            {
                strTitle = _utl.GetString(row.MeetingName);
            }
            // 3:クラブ（理事・委員会）
            else if (wkEveClass.Equals(LADef.EVENTCLASS_DI))
            {
                // クラブイベント区分
                wkClubEveClass = _utl.GetString(row.ClubEventClass);

                // 理事会
                if (wkClubEveClass.Equals(LADef.CLUBEVENTCLASS_RI))
                {
                    strTitle = ST_CLUSEVENT_1 + " " + _utl.GetString(row.Subject);
                }
                // 委員会
                else if (wkClubEveClass.Equals(LADef.CLUBEVENTCLASS_IN))
                {
                    strTitle = ST_CLUSEVENT_2 + " " + _utl.GetString(row.Subject);
                }
                // その他
                else if (wkClubEveClass.Equals(LADef.CLUBEVENTCLASS_ET))
                {
                    strTitle = ST_CLUSEVENT_3 + " " + _utl.GetString(row.Subject);
                }
            }

            // 中止設定
            if (_utl.GetString(row.CancelFlg).Equals(_utl.CANCELFLG))
            {
                strCancel = LADef.ST_CANCEL;
            }

            // 出欠設定
            wkAnswer = _utl.GetString(row.Answer);
            if (wkAnswer.Equals(LADef.ANSWER_PRE))
            {
                strAnswer = LADef.ST_ANSWER_PRE;
                colAnswer = Color.FromHex(LADef.STRCOL_STRDEF);
            }
            else if (wkAnswer.Equals(LADef.ANSWER_ABS))
            {
                strAnswer = LADef.ST_ANSWER_ABS;
                colAnswer = Color.FromHex(LADef.STRCOL_STRDEF);
            }
            else if (wkAnswer.Equals(LADef.ANSWER_NO))
            {
                strAnswer = LADef.ST_ANSWER_NO;
                colAnswer = Color.FromHex(LADef.STRCOL_RED);
            }

            // 未読設定
            wkBadge = _utl.GetString(row.Badge);
            if (!wkBadge.Equals(string.Empty))
            {
                strBadge = LADef.ST_BADGE;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 未読情報削除（SQLite）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetBadgeSQlite()
        {
            string _dataClass = _utl.GetSQLString(_cbadge.DataClass);
            string _dataNo = _cbadge.DataNo.ToString();
            string _clubCode = _utl.GetSQLString(_cbadge.ClubCode);
            string _memberCode = _utl.GetSQLString(_cbadge.MemberCode);

            foreach (Table.T_BADGE row in _sqlite.Del_T_BADGE("DELETE FROM T_BADGE " +
                                                              "WHERE DataClass = " + _dataClass + " " +
                                                              "AND DataNo = " + _dataNo + " " +
                                                              "AND ClubCode = " + _clubCode + " " +
                                                              "AND MemberCode = " + _memberCode + " "))
            { }
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// イベントリスト行クラス
    /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class EventRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _dataNo = 0;
        private int _eventDataNo = 0;
        private string _eventDate = string.Empty;
        private string _eventCancel = string.Empty;
        private string _title = string.Empty;
        private string _answer = string.Empty;
        private Color _answerColor = Color.Default;
        private string _badge = string.Empty;

        public EventRow(int dataNo, 
                        int eventDataNo, 
                        string eventDate, 
                        string eventCancel, 
                        string title, 
                        string answer,
                        Color answerColor,
                        string badge)
        {
            DataNo = dataNo;
            EventDataNo = eventDataNo;
            EventDate = eventDate;
            EventCancel = eventCancel;
            Title = title;
            Answer = answer;
            AnswerColor = answerColor;
            Badge = badge;
        }

        public int DataNo 
        {
            get { return _dataNo; } 
            set
            {
                if (_dataNo != value)
                {
                    _dataNo = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(DataNo)));
                    }
                }
            }
        }

        public int EventDataNo
        {
            get { return _eventDataNo; }
            set
            {
                if (_eventDataNo != value)
                {
                    _eventDataNo = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(EventDataNo)));
                    }
                }
            }
        }

        public string EventDate
        {
            get { return _eventDate; }
            set
            {
                if (_eventDate != value)
                {
                    _eventDate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(EventDate)));
                    }
                }
            }
        }

        public string EventCancel
        {
            get { return _eventCancel; }
            set
            {
                if (_eventCancel != value)
                {
                    _eventCancel = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(EventCancel)));
                    }
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Title)));
                    }
                }
            }
        }
        public string Answer
        {
            get { return _answer; }
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Answer)));
                    }
                }
            }
        }
        public Color AnswerColor
        {
            get { return _answerColor; }
            set
            {
                if (_answerColor != value)
                {
                    _answerColor = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(AnswerColor)));
                    }
                }
            }
        }
        public string Badge
        {
            get { return _badge; }
            set
            {
                if (_badge != value)
                {
                    _badge = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Badge)));
                    }
                }
            }
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// リスト用行クラス切り替え
    /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
    public class MyEventSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (EventRow)item;
            if (info.DataNo > 0)
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