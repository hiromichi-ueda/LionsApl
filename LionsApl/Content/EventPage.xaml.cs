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
        private string ST_EVENT_3 = "理事・委員会出欠の確認";

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public EventPage(int dataNo, int eventDataNo)
        {
            InitializeComponent();

            // font-size
            SetFontSize();

            // 前画面からの取得情報
            _dataNo = dataNo;                           // データNo.
            _eventDataNo = eventDataNo;                 // イベントデータNo.

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


        private void SetFontSize()
        {
            double wfFontSizse = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            ////////////////////////
            // 表示項目

            // キャビネットイベント表示

            // 年間例会スケジュール表示
            this.lbl_MD_Date.FontSize = wfFontSizse;
            this.MD_Date.FontSize = wfFontSizse;
            this.MD_Cancel.FontSize = wfFontSizse;
            this.lbl_MD_Place.FontSize = wfFontSizse;
            this.MD_Place.FontSize = wfFontSizse;
            this.lbl_MD_Count.FontSize = wfFontSizse;
            this.MD_Count.FontSize = wfFontSizse;
            this.lbl_MD_Name.FontSize = wfFontSizse;
            this.MD_Name.FontSize = wfFontSizse;
            this.lbl_MD_Online.FontSize = wfFontSizse;
            this.MD_Online.FontSize = wfFontSizse;
            this.lbl_MD_Sake.FontSize = wfFontSizse;
            this.MD_Sake.FontSize = wfFontSizse;

            // 理事・委員会表示
            this.lbl_DD_Date.FontSize = wfFontSizse;
            this.DD_Date.FontSize = wfFontSizse;
            this.DD_Cancel.FontSize = wfFontSizse;
            this.lbl_DD_Season.FontSize = wfFontSizse;
            this.DD_Season.FontSize = wfFontSizse;
            this.lbl_DD_Place.FontSize = wfFontSizse;
            this.DD_Place.FontSize = wfFontSizse;
            this.lbl_DD_Agenda.FontSize = wfFontSizse;
            this.DD_Agenda.FontSize = wfFontSizse;
            this.lbl_DD_AnsDate.FontSize = wfFontSizse;
            this.DD_AnsDate.FontSize = wfFontSizse;


            ////////////////////////
            // 入力項目

            // 年間例会スケジュール入力
            this.lbl_MI_Header.FontSize = wfFontSizse;
            this.lbl_MI_Opt1Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt2Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt3Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt4Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt5Name.FontSize = wfFontSizse;
            this.lbl_MI_OtherUser.FontSize = wfFontSizse;
            this.MI_OtherUser.FontSize = wfFontSizse;
            this.lbl_MI_AnsDate.FontSize = wfFontSizse;
            this.MI_AnsDate.FontSize = wfFontSizse;
            this.lbl_MI_footer.FontSize = wfFontSizse;

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

                // イベントクラス別のデータ読み込み・タイトル文字列の設定・各画面の設定
                // キャビネットイベント
                if (wkEventClass.Equals(_utl.EVENTCLASS_CV))
                {
                    // キャビネットイベントのデータ読み込み
                    foreach (Table.T_EVENT row in _sqlite.Get_T_EVENT("Select * " +
                                                                      "From T_EVENT " +
                                                                      "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _event = row;
                    }
                    // タイトル文字列の設定
                    BodyTitle.Text = ST_EVENT_1;

                    MeetingDspSL.HeightRequest = 0.0;
                    MeetingInpSL.HeightRequest = 0.0;
                    DirectDspSL.HeightRequest = 0.0;

                }
                // 年間例会スケジュール
                else if (wkEventClass.Equals(_utl.EVENTCLASS_CL))
                {
                    // 年間例会スケジュールのデータ読み込み
                    foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                                          "From T_MEETINGSCHEDULE " +
                                                                                          "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _meetingschedule = row;
                    }
                    // タイトル文字列の設定
                    BodyTitle.Text = ST_EVENT_2;
                    // 年間例会スケジュール画面の設定
                    SetCEventPageMeeting();

                }
                // 理事・委員会
                else if (wkEventClass.Equals(_utl.EVENTCLASS_DR))
                {
                    // 理事・委員会のデータ読み込み
                    foreach (Table.T_DIRECTOR row in _sqlite.Get_T_DIRECTOR("Select * " +
                                                                            "From T_DIRECTOR " +
                                                                            "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _director = row;
                    }
                    // タイトル文字列の設定
                    BodyTitle.Text = ST_EVENT_3;
                    // 理事・委員会画面の設定
                    SetCEventPageDirect();

                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 年間例会スケジュール画面の設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetCEventPageMeeting()
        {
            string wkDate = string.Empty;
            string wkCancel = string.Empty;
            int wkCount = 0;
            string wkPlace = string.Empty;
            string wkName = string.Empty;
            string wkOnline = string.Empty;
            string wkSake = string.Empty;
            bool wkOpt1flg = false;
            string wkOpt1Name = string.Empty;
            string wkOpt1Value = string.Empty;
            bool wkOpt2flg = false;
            string wkOpt2Name = string.Empty;
            string wkOpt2Value = string.Empty;
            bool wkOpt3flg = false;
            string wkOpt3Name = string.Empty;
            string wkOpt3Value = string.Empty;
            bool wkOpt4flg = false;
            string wkOpt4Name = string.Empty;
            string wkOpt4Value = string.Empty;
            bool wkOpt5flg = false;
            string wkOpt5Name = string.Empty;
            string wkOpt5Value = string.Empty;

            string wkAnsDate = string.Empty;


            // データNo.
            //DataNo = _eventret.DataNo;

            // イベントデータNo.
            //EventDataNo = _eventret.EventDataNo;

            // 例会日
            wkDate = _utl.GetString(_meetingschedule.MeetingDate).Substring(0, 10) + " " +
                     _utl.GetString(_meetingschedule.MeetingTime).Substring(0, 5) + "～";

            // 中止
            wkCancel = _utl.StrCancel(_meetingschedule.CancelFlg);

            // 例会場所
            wkPlace = _utl.GetString(_meetingschedule.MeetingPlace);

            // 例会回数
            wkCount = _meetingschedule.MeetingCount;

            // 例会名
            wkName = _utl.GetString(_meetingschedule.MeetingName);

            // 会議方法
            wkOnline = _utl.StrOnline(_meetingschedule.Online);

            // お酒
            wkSake = _utl.StrOnOff(_meetingschedule.Sake);

            // オプション1
            wkOpt1flg = _utl.GetString(_meetingschedule.OptionRadio1).Equals(_utl.ONFLG);
            if (wkOpt1flg)
            {
                // オプション1（項目名）
                wkOpt1Name = _utl.GetString(_meetingschedule.OptionName1);

                // オプション1（入力値）
                wkOpt1Value = _utl.GetString(_eventret.Option1);
            }
            // オプション2
            wkOpt2flg = _utl.GetString(_meetingschedule.OptionRadio2).Equals(_utl.ONFLG);
            if (wkOpt2flg)
            {
                // オプション2（項目名）
                wkOpt2Name = _utl.GetString(_meetingschedule.OptionName2);

                // オプション2（入力値）
                wkOpt2Value = _utl.GetString(_eventret.Option2);
            }
            // オプション3
            wkOpt3flg = _utl.GetString(_meetingschedule.OptionRadio3).Equals(_utl.ONFLG);
            if (wkOpt3flg)
            {
                // オプション3（項目名）
                wkOpt3Name = _utl.GetString(_meetingschedule.OptionName3);

                // オプション3（入力値）
                wkOpt3Value = _utl.GetString(_eventret.Option3);
            }
            // オプション4
            wkOpt4flg = _utl.GetString(_meetingschedule.OptionRadio4).Equals(_utl.ONFLG);
            if (wkOpt4flg)
            {
                // オプション4（項目名）
                wkOpt4Name = _utl.GetString(_meetingschedule.OptionName4);

                // オプション4（入力値）
                wkOpt4Value = _utl.GetString(_eventret.Option4);
            }
            // オプション5
            wkOpt5flg = _utl.GetString(_meetingschedule.OptionRadio5).Equals(_utl.ONFLG);
            if (wkOpt5flg)
            {
                // オプション5（項目名）
                wkOpt5Name = _utl.GetString(_meetingschedule.OptionName5);

                // オプション5（入力値）
                wkOpt5Value = _utl.GetString(_eventret.Option5);
            }

            // 回答期限
            wkAnsDate = _utl.GetString(_meetingschedule.AnswerDate).Substring(0, 10);

            // 表示項目設定
            MD_Date.Text = wkDate;
            MD_Cancel.Text = wkCancel;
            MD_Place.Text = wkPlace;
            MD_Count.Text = wkCount.ToString();
            MD_Name.Text = wkName;
            // 会議方法の値が無い場合
            if (wkOnline == string.Empty)
            {
                // 会議方法項目非表示
                MD_Online.IsVisible = false;
                lbl_MD_Online.IsVisible = false;
            }
            else
            {
                MD_Online.Text = wkOnline;
            }
            MD_Sake.Text = wkSake;

            // 入力項目設定

            // オプション1
            SetOptionItem(wkOpt1flg, wkOpt1Value, wkOpt1Name, ref lbl_MI_Opt1Name, ref MI_Opt1Switch);

            // オプション2
            SetOptionItem(wkOpt2flg, wkOpt2Value, wkOpt2Name, ref lbl_MI_Opt2Name, ref MI_Opt2Switch);

            // オプション3
            SetOptionItem(wkOpt3flg, wkOpt3Value, wkOpt3Name, ref lbl_MI_Opt3Name, ref MI_Opt3Switch);

            // オプション4
            SetOptionItem(wkOpt4flg, wkOpt4Value, wkOpt4Name, ref lbl_MI_Opt4Name, ref MI_Opt4Switch);

            // オプション5
            SetOptionItem(wkOpt5flg, wkOpt5Value, wkOpt5Name, ref lbl_MI_Opt5Name, ref MI_Opt5Switch);

            for (int idx = 0 ; idx <= 15; idx++)
            {
                MI_OtherUser.Items.Add(idx.ToString());
            }

            // 回答期限
            MI_AnsDate.Text = wkAnsDate;


            EventDspSL.IsVisible = false;
            EventInpSL.IsVisible = false;
            DirectDspSL.IsVisible = false;


            //// コントロールテンプレートを作成
            //EventPageMeeting meeting = new EventPageMeeting(ref _eventret,
            //                                                ref _meetingschedule,
            //                                                Device.GetNamedSize(NamedSize.Default, typeof(Label)));

            //// StackLayoutにコントロールテンプレートを追加
            //EventStackLayout.Children.Add(meeting);

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会画面の設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
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


            DD_Date.Text = wkDate;
            DD_Cancel.Text = wkCancel;
            DD_Season.Text = wkSeason;
            DD_Place.Text = wkPlace;
            DD_Agenda.Text = wkAgenda;
            DD_AnsDate.Text = wkAnsDate;


            // キャビネットイベント項目非表示
            EventDspSL.IsVisible = false;
            EventInpSL.IsVisible = false;

            // 年間例会スケジュール項目非表示
            MeetingDspSL.IsVisible = false;
            MeetingInpSL.IsVisible = false;


            //// コントロールテンプレートを作成
            //EventPageDirect direct = new EventPageDirect(_director.DataNo,
            //                                                wkDate,
            //                                                wkCancel,
            //                                                wkSeason,
            //                                                wkPlace,
            //                                                wkAgenda,
            //                                                wkAnsDate,
            //                                                Device.GetNamedSize(NamedSize.Default, typeof(Label)));

            //// StackLayoutにコントロールテンプレートを追加
            //EventStackLayout.Children.Add(direct);

        }


        private void SetOptionItem(bool flg, 
                                   string radio, 
                                   string name, 
                                   ref Label lavelItem, 
                                   ref Switch switchItem)
        {
            // 入力項目設定
            if (flg)
            {
                lavelItem.Text = name;
                if (radio.Equals(_utl.ONFLG))
                {
                    switchItem.IsToggled = true;
                }
                else
                {
                    switchItem.IsToggled = false;
                }
            }
            else
            {
                lavelItem.IsVisible = false;
                switchItem.IsVisible = false;
            }
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