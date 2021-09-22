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

        // リストビュー設定内容
        public List<EventRow> Items { get; set; }

        // 文字列定数
        private string ST_EVENT_1 = "[キャビネット]";
        private string ST_EVENT_2 = "[クラブ]";
        private string ST_EVENT_3 = "[クラブ]";

        //private string ANSWER_NO = "未登録";
        private string ST_ANSWER_NO = "未回答";
        private string ST_ANSWER_1 = "出席";
        private string ST_ANSWER_2 = "欠席";

        private string ST_CLUSEVENT_1 = "[理事会]";
        private string ST_CLUSEVENT_2 = "[委員会]";

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
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            EventRow item = e.Item as EventRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (string.IsNullOrEmpty(item.Title))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 出欠確認画面へ
            Navigation.PushAsync(new EventPage(item.DataNo, item.EventDataNo));

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
            List<EventRow> items = new List<EventRow>();


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
                                            "t4.EventPlace AS ClubEventPlace " +
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
                                        "ORDER BY t1.EventDate DESC, t1.DataNo DESC"))


                {
                    intDataNo = 0;                           // データNo.設定用
                    intEventDataNo = 0;                      // イベントデータNo.設定用
                    strDate = "";                            // 月日設定用文字列
                    strCancel = "";                          // 中止表示用文字列
                    strTitle = "";                           // タイトル設定用文字列
                    strAnswer = "";                          // 出欠設定用文字列


                    // イベントリストの各項目値を取得する
                    GetEventListData(row,
                                     ref intDataNo,
                                     ref intEventDataNo,
                                     ref strDate,
                                     ref strCancel,
                                     ref strTitle,
                                     ref strAnswer);

                    // イベントリスト行クラスを作成する。
                    EventRow eventRow = new EventRow(intDataNo, intEventDataNo, strDate, strCancel, strTitle, strAnswer);
                    items.Add(eventRow);
                }
                EventListView.ItemsSource = items;
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
        /// <param name="strCancel">キャンセル（表示用変数）</param>
        /// <param name="strTitle">タイトル（表示用変数）</param>
        /// <param name="strAnswer">回答（表示用変数）</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventListData(Table.EVENT_LIST row,
                                        ref int intDataNo,
                                        ref int intEventDataNo,
                                        ref string strDate,
                                        ref string strCancel,
                                        ref string strTitle,
                                        ref string strAnswer)
        {
            string wkEveClass = string.Empty;
            string wkClubEveClass = string.Empty;
            string wkAnswer = string.Empty;

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
            if (wkEveClass.Equals(_utl.EVENTCLASS_CV))
            {
                strDate = strDate + " " + ST_EVENT_1;
            }
            // 2:クラブ（例会）
            else if (wkEveClass.Equals(_utl.EVENTCLASS_CL))
            {
                strDate = strDate + " " + ST_EVENT_2;
            }
            // 3:クラブ（理事・委員会）
            else if (wkEveClass.Equals(_utl.EVENTCLASS_DR))
            {
                strDate = strDate + " " + ST_EVENT_3;
            }

            // タイトル設定
            // 1:キャビネット
            if (wkEveClass.Equals(_utl.EVENTCLASS_CV))
            {
                strTitle = _utl.GetString(row.Title);
            }
            // 2:クラブ（例会）
            else if (wkEveClass.Equals(_utl.EVENTCLASS_CL))
            {
                strTitle = _utl.GetString(row.MeetingName);
            }
            // 3:クラブ（理事・委員会）
            else if (wkEveClass.Equals(_utl.EVENTCLASS_DR))
            {
                // クラブイベント区分
                wkClubEveClass = _utl.GetString(row.ClubEventClass);

                // 理事会
                if (wkClubEveClass.Equals(_utl.CLUBEVENTCLASS_RI))
                {
                    strTitle = ST_CLUSEVENT_1 + " " + _utl.GetString(row.Subject);
                }
                // 委員会
                else if (wkClubEveClass.Equals(_utl.CLUBEVENTCLASS_IN))
                {
                    strTitle = ST_CLUSEVENT_2 + " " + _utl.GetString(row.Subject);
                }
            }

            // 中止設定
            if (_utl.GetString(row.CancelFlg).Equals(_utl.CANCELFLG))
            {
                strCancel = ST_CANCEL;
            }

            // 出欠設定
            wkAnswer = _utl.GetString(row.Answer);
            if (wkAnswer.Equals(_utl.ANSWER_PRE))
            {
                strAnswer = ST_ANSWER_1;
            }
            else if (wkAnswer.Equals(_utl.ANSWER_AB))
            {
                strAnswer = ST_ANSWER_2;
            }
            else if (wkAnswer.Equals(_utl.ANSWER_NO))
            {
                strAnswer = ST_ANSWER_NO;
            }

            //DisplayAlert("Disp", $" DataNo : {intDataNo}\r\n" +
            //         $" Class : {wkEveClass}\r\n" +
            //         $" EventDataNo : {intEventDataNo}\r\n" +
            //         $" Date : {strDate}\r\n" +
            //         $" Cancel : {strCancel}\r\n" +
            //         $" Title : {strTitle}\r\n" +
            //         $" ClubClass : {wkClubEveClass}\r\n" +
            //         $" Answer : {strAnswer}", "OK");

        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// イベントリスト行クラス
    /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class EventRow
    {
        public EventRow(int dataNo, int eventDataNo, string eventDate, string eventCancel, string title, string answer)
        {
            DataNo = dataNo;
            EventDataNo = eventDataNo;
            EventDate = eventDate;
            EventCancel = eventCancel;
            Title = title;
            Answer = answer;
        }
        public int DataNo { get; set; }
        public int EventDataNo { get; set; }
        public string EventDate { get; set; }
        public string EventCancel { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
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
            if (!String.IsNullOrEmpty(info.EventDate))
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