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
    public partial class EventList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // リストビュー設定内容
        public List<EventRow> Items { get; set; }

        // 文字列
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

        public EventList()
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

            // イベント情報データ取得
            //GetEventData();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventData()
        {
            int idx = 0;
            int intDataNo = 0;                              // データNo.設定用
            int intEventDataNo = 0;                         // イベントデータNo.設定用
            string strDate = "";                            // 月日設定用文字列
            string strEveClass = "";                        // イベントクラス設定用文字列
            string strClubEveClass = "";                    // クラブイベントクラス設定用文字列
            string strTitle = "";                           // タイトル設定用文字列
            string strCancel = "";                          // 中止表示用文字列
            string strAnswer = "";                          // 出欠設定用文字列
            List<EventRow> items = new List<EventRow>();


            try
            {
                foreach (Table.EVENT_LIST row in _sqlite.Get_EVENT_LIST(
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
                                            "t4.Subject," +
                                            "t4.EventClass " +
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
                                        "ORDER BY t1.EventDate ASC"))
                    //"WHERE " +
                    //    "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' " +


                {
                    intDataNo = 0;                           // データNo.設定用
                    intEventDataNo = 0;                      // イベントデータNo.設定用
                    strDate = "";                            // 月日設定用文字列
                    strEveClass = "";                        // イベントクラス設定用文字列
                    strClubEveClass = "";                    // クラブイベントクラス設定用文字列
                    strTitle = "";                           // タイトル設定用文字列
                    strCancel = "";                          // 中止表示用文字列
                    strAnswer = "";                          // 出欠設定用文字列

                    // イベントリストの各項目値を取得する
                    GetEventListData(row,
                                     ref intDataNo,
                                     ref intEventDataNo,
                                     ref strDate,
                                     ref strEveClass,
                                     ref strClubEveClass,
                                     ref strTitle,
                                     ref strCancel,
                                     ref strAnswer);

                    items.Add(new EventRow(intDataNo, intEventDataNo, strDate, strEveClass, strClubEveClass, strTitle, strCancel, strAnswer));

                }
                EventListView.ItemsSource = items;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

        /// <summary>
        /// イベントリストの各項目値を取得するして表示用変数に設定する。
        /// </summary>
        /// <param name="row">SQLiteから取得したイベントデータ</param>
        /// <param name="intDataNo">表示対象データのDataNo</param>
        /// <param name="intEventDataNo">表示対象データのEventDataNo</param>
        /// <param name="strDate">日付（表示用変数）</param>
        /// <param name="strEveClass">イベント区分（表示用変数）</param>
        /// <param name="strClubEveClass">クラブイベント区分（表示用変数）</param>
        /// <param name="strTitle">タイトル（表示用変数）</param>
        /// <param name="strCancel">キャンセル（表示用変数）</param>
        /// <param name="strAnswer">回答（表示用変数）</param>
        private void GetEventListData(Table.EVENT_LIST row,
                                        ref int intDataNo,
                                        ref int intEventDataNo,
                                        ref string strDate,
                                        ref string strEveClass,
                                        ref string strClubEveClass,
                                        ref string strTitle,
                                        ref string strCancel,
                                        ref string strAnswer)
        {
            string wkAnswer = string.Empty;
            string wkClass = string.Empty;

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
            strEveClass = _utl.GetString(row.EventClass);
            // 1:キャビネット
            if (strEveClass.Equals(_utl.EVENTCLASS_CV))
            {
                strDate = strDate + " " + ST_EVENT_1;
            }
            // 2:クラブ（例会）
            else if (strEveClass.Equals(_utl.EVENTCLASS_CL))
            {
                strDate = strDate + " " + ST_EVENT_2;
            }
            // 3:クラブ（理事・委員会）
            else if (strEveClass.Equals(_utl.EVENTCLASS_DR))
            {
                strDate = strDate + " " + ST_EVENT_3;
            }

            // クラブイベント区分
            strClubEveClass = _utl.GetString(row.ClubEventClass);

            // タイトル設定
            // 1:キャビネット
            if (strEveClass.Equals(_utl.EVENTCLASS_CV))
            {
                strTitle = _utl.GetString(row.Title);
            }
            // 2:クラブ（例会）
            else if (strEveClass.Equals(_utl.EVENTCLASS_CL))
            {
                strTitle = _utl.GetString(row.MeetingName);
            }
            // 3:クラブ（理事・委員会）
            else if (strEveClass.Equals(_utl.EVENTCLASS_DR))
            {
                // 理事会
                if (strClubEveClass.Equals(_utl.CLUBEVENTCLASS_RI))
                {
                    strTitle = ST_CLUSEVENT_1 + " " + _utl.GetString(row.Subject);
                }
                // 委員会
                else if (strClubEveClass.Equals(_utl.CLUBEVENTCLASS_IN))
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
            if (string.IsNullOrEmpty(item.EventClass))
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 出欠確認画面へ
            //Navigation.PushAsync(new EventPage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    public sealed class EventRow
    {
        public EventRow(int dataNo, int eventDataNo, string eventDate, string eventClass, string clubEventClass, string title, string eventPlace, string answer)
        {
            DataNo = dataNo;
            EventDataNo = eventDataNo;
            EventDate = eventDate;
            EventClass = eventClass;
            ClubEventClass = clubEventClass;
            Title = title;
            EventPlace = eventPlace;
            Answer = answer;
        }
        public int DataNo { get; set; }
        public int EventDataNo { get; set; }
        public string EventDate { get; set; }
        public string EventClass { get; set; }
        public string ClubEventClass { get; set; }
        public string Title { get; set; }
        public string EventPlace { get; set; }
        public string Answer { get; set; }
    }

    public class MyEventSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (EventRow)item;
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