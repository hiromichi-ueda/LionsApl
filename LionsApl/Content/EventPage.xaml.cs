using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 出欠確認画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // T_EVENTRETテーブルクラス
        private Table.T_EVENTRET _eventret;
        // T_EVENTテーブルクラス
        private Table.T_EVENT _event;
        // T_MEETINGSCHEDULEテーブルクラス
        private Table.T_MEETINGSCHEDULE _meetingschedule;
        // T_DIRECTORテーブルクラス
        private Table.T_DIRECTOR _director;

        // キャビネットイベント表示用クラス
        //private CEventPageEvent _event;
        // 年間例会スケジュール表示用クラス
        //private CEventPageMeeting _meeting;
        // 理事・委員会表示用クラス
        //private CEventPageDirect _direct;

        // 前画面からの取得情報
        private int _dataNo;                                 // データNo.
        private int _eventDataNo;                            // イベントNo.

        // 文字列定数
        private string ST_EVENT_1 = "キャビネット出欠の確認";
        private string ST_EVENT_2 = "例会出欠の確認";
        private string ST_EVENT_3_1 = "理事会出欠の確認";
        private string ST_EVENT_3_2 = "委員会出欠の確認";

        private string ST_CANCEL = "中止";

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public EventPage(int dataNo, int eventDataNo)
        {
            InitializeComponent();

            _dataNo = dataNo;
            _eventDataNo = eventDataNo;

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
            GetEventRet();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出席ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Present_Button_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Disp", $"出席", "OK");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 欠席ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Adsent_Button_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Disp", $"欠席", "OK");
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント出欠情報を取得して画面に設定する
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventRet()
        {
            // 変数
            string wkEventClass = string.Empty;
            string wkDrEventclass = string.Empty;

            try
            {
                foreach (Table.T_EVENTRET row in _sqlite.Get_T_EVENTRET("Select * " +
                                                                    "From T_EVENTRET " +
                                                                    "Where DataNo='" + _dataNo + "'"))
                {
                    _eventret = row;
                }


                wkEventClass = _utl.GetString(_eventret.EventClass);

                // イベントクラス別のデータ読み込み
                if (wkEventClass.Equals(_utl.EVENTCLASS_CV))
                {
                    foreach (Table.T_EVENT row in _sqlite.Get_T_EVENT("Select * " +
                                                                      "From T_EVENT " +
                                                                      "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _event = row;
                    }
                }
                else if (wkEventClass.Equals(_utl.EVENTCLASS_CL))
                {
                    foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                                          "From T_MEETINGSCHEDULE " +
                                                                                          "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _meetingschedule = row;
                    }
                }
                else if (wkEventClass.Equals(_utl.EVENTCLASS_DR))
                {
                    foreach (Table.T_DIRECTOR row in _sqlite.Get_T_DIRECTOR("Select * " +
                                                                            "From T_DIRECTOR " +
                                                                            "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _director = row;
                    }
                }

                // タイトル文字列の設定
                if (wkEventClass.Equals(_utl.EVENTCLASS_CV))
                {
                    BodyTitle.Text = ST_EVENT_1;


                }
                else if (wkEventClass.Equals(_utl.EVENTCLASS_CL))
                {
                    BodyTitle.Text = ST_EVENT_2;
                }
                else if (wkEventClass.Equals(_utl.EVENTCLASS_DR))
                {
                    wkDrEventclass = _utl.GetString(_director.EventClass);
                    if (wkDrEventclass.Equals(_utl.CLUBEVENTCLASS_RI))
                    {
                        BodyTitle.Text = ST_EVENT_3_1;
                    }
                    else if (wkDrEventclass.Equals(_utl.CLUBEVENTCLASS_IN))
                    {
                        BodyTitle.Text = ST_EVENT_3_2;
                    }

                    SetCEventPageDirect();
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }


        private void SetCEventPageDirect()
        {
            string wkDate = string.Empty;
            string wkCancel = string.Empty;
            string wkSeason = string.Empty;
            string wkPlace = string.Empty;
            string wkAgenda = string.Empty;
            string wkAnsDate = string.Empty;

            // 開催日
            wkDate = _utl.GetString(_director.EventDate).Substring(0, 10) + " " +
                     _utl.GetString(_director.EventTime).Substring(0, 5) + "～";

            // 中止
            wkCancel = _utl.StrCancel(_director.CancelFlg);

            // 区分
            wkSeason = _utl.StrSeason(_director.Season);

            // 開催場所
            wkPlace = _utl.GetString(_director.EventPlace);

            // 議題・内容
            wkAgenda = _utl.GetString(_director.Agenda, _utl.NLC_ON);

            // 回答期限
            wkAnsDate = _utl.GetString(_director.AnswerDate).Substring(0, 10);

            // コントロールテンプレートを作成
            EventPageDirect direct = new EventPageDirect(_director.DataNo,
                                                            wkDate,
                                                            wkCancel,
                                                            wkSeason,
                                                            wkPlace,
                                                            wkAgenda,
                                                            wkAnsDate,
                                                            Device.GetNamedSize(NamedSize.Default, typeof(Label)));

            // StackLayoutにコントロールテンプレートを追加
            EventStackLayout.Children.Add(direct);

        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// キャビネットイベント表示用クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class CEventPageEvent
    {
        public CEventPageEvent(string date,
                                  string cancel,
                                  string stime,
                                  string etime,
                                  string rectime,
                                  string place,
                                  string title,
                                  string body,
                                  string sake,
                                  string meeting,
                                  string murl,
                                  string mid,
                                  string mpw,
                                  string mother,
                                  string fname)
        {
            Data = date;
            Cancel = cancel;
            STime = stime;
            ETime = etime;
            RecTime = rectime;
            Place = place;
            Title = title;
            Body = body;
            Sake = sake;
            Meeting = meeting;
            MUrl = murl;
            MId = mid;
            MPw = mpw;
            MOther = mother;
            FName = fname;
        }
        public string Data { get; set; }
        public string Cancel { get; set; }
        public string STime { get; set; }
        public string ETime { get; set; }
        public string RecTime { get; set; }
        public string Place { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Sake { get; set; }
        public string Meeting { get; set; }
        public string MUrl { get; set; }
        public string MId { get; set; }
        public string MPw { get; set; }
        public string MOther { get; set; }
        public string FName { get; set; }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 年間例会スケジュール表示用クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class CEventPageMeeting
    {
        public CEventPageMeeting(string date,
                                string cancel,
                                string place,
                                int count,
                                string name,
                                string online,
                                string sake)
        {
            Data = date;
            Cancel = cancel;
            Place = place;
            Count = count;
            Name = name;
            Online = online;
            Sake = sake;
        }
        public string Data { get; set; }
        public string Cancel { get; set; }
        public string Place { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Online { get; set; }
        public string Sake { get; set; }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 理事・委員会表示用クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    public sealed class CEventPageDirect
    {
        public CEventPageDirect(string date, 
                                string cancel, 
                                string season, 
                                string place, 
                                string agenda, 
                                string ansdate)
        {
            Data = date;
            Cancel = cancel;
            Season = season;
            Place = place;
            Agenda = agenda;
            AnsDate = ansdate;
        }
        public string Data { get; set; }
        public string Cancel { get; set; }
        public string Season { get; set; }
        public string Place { get; set; }
        public string Agenda { get; set; }
        public string AnsDate { get; set; }
    }

}