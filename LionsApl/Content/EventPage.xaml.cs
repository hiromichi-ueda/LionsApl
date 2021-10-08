using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

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

        // 情報通信マネージャクラス
        private IComManager _icom;

        // T_EVENTRETテーブルクラス
        private Table.T_EVENTRET _eventret;
        // T_EVENTテーブルクラス
        private Table.T_EVENT _event;
        // T_MEETINGSCHEDULEテーブルクラス
        private Table.T_MEETINGSCHEDULE _meetingschedule;
        // T_DIRECTORテーブルクラス
        private Table.T_DIRECTOR _director;

        // T_EVENTRET登録クラス
        private CEVENTRET _ceventret;

        // キャビネットイベント表示用クラス
        //private CEventPageEvent _event;
        // 年間例会スケジュール表示用クラス
        //private CEventPageMeeting _meeting;
        // 理事・委員会表示用クラス
        //private CEventPageDirect _direct;

        // 前画面からの取得情報
        private int _dataNo;                                // データNo.
        private int _eventDataNo;                           // イベントNo.

        private string _eventClass;                         // イベントクラス
        private string _onlineFlg;                          // オンライン参加フラグ
        private string _opt1Flg;                            // オプション1フラグ
        private string _opt2Flg;                            // オプション2フラグ
        private string _opt3Flg;                            // オプション3フラグ
        private string _opt4Flg;                            // オプション4フラグ
        private string _opt5Flg;                            // オプション5フラグ
        private string _oCntFlg;                            // 本人以外の参加人数フラグ

        // ファイルパス
        private string _fileName;                           // ファイル名

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

            // 情報通信マネージャー生成
            _icom = IComManager.GetInstance();

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

            // 出欠情報クラス設定
            string emp = string.Empty;
            _ceventret = new CEVENTRET(0, emp, emp, emp, emp, emp, emp, emp, emp, emp, 0);

            // データNo.
            _ceventret.DataNo = _dataNo;

            // イベント情報データ取得
            GetEventRet();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 詳細を見るボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void File_Button_Clicked(object sender, System.EventArgs e)
        {
            //Navigation.PushModalAsync(new EventPageFile(_dataNo, _fileName));
            Navigation.PushAsync(new EventPageFile(_dataNo, _fileName));
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

            // 回答（出席）
            _ceventret.Answer = _utl.ANSWER_PRE;

            // キャビネットイベント
            if (_eventClass.Equals(_utl.EVENTCLASS_CV))
            {
                Present_Cabinet();
            }
            // 年間例会スケジュール
            else if (_eventClass.Equals(_utl.EVENTCLASS_CL))
            {
                Present_Meeting();
            }
            // 理事・委員会
            else if (_eventClass.Equals(_utl.EVENTCLASS_DR))
            {
                //Present_Director();
            }

            // 出欠情報をコンテンツに設定
            _icom.SetContentToEVENTRET(_ceventret);
            try
            {
                // SQLServerへ登録
                Task<HttpResponseMessage> response = _icom.AsyncPostTextForWebAPI();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLServer 出席情報登録エラー : {ex.Message}", "OK");
            }

            try
            {
                // SQLiteへ登録
                SetEventRetSQlite();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite 出席情報登録エラー : {ex.Message}", "OK");
            }

            DisplayAlert("出欠確認", $"出席情報を登録しました。", "OK");

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出席：キャビネットイベント
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Present_Cabinet()
        {
            //string recInfo = string.Empty;

            // オプション1
            if (_opt1Flg.Equals(_utl.ONFLG))
            {
                if (EI_Opt1Switch.IsToggled)
                {
                    _ceventret.Option1 = _utl.ONFLG;
                    //recInfo += "オプション1=ON\r\n";
                }
                else
                {
                    _ceventret.Option1 = _utl.OFFFLG;
                    //recInfo += "オプション1=OFF\r\n";
                }
            }

            // オプション2
            if (_opt2Flg.Equals(_utl.ONFLG))
            {
                if (EI_Opt2Switch.IsToggled)
                {
                    _ceventret.Option2 = _utl.ONFLG;
                    //recInfo += "オプション2=ON\r\n";
                }
                else
                {
                    _ceventret.Option2 = _utl.OFFFLG;
                    //recInfo += "オプション2=OFF\r\n";
                }
            }

            // オプション3
            if (_opt3Flg.Equals(_utl.ONFLG))
            {
                if (EI_Opt3Switch.IsToggled)
                {
                    _ceventret.Option3 = _utl.ONFLG;
                    //recInfo += "オプション3=ON\r\n";
                }
                else
                {
                    _ceventret.Option3 = _utl.OFFFLG;
                    //recInfo += "オプション3=OFF\r\n";
                }
            }

            // オプション4
            if (_opt4Flg.Equals(_utl.ONFLG))
            {
                if (EI_Opt4Switch.IsToggled)
                {
                    _ceventret.Option4 = _utl.ONFLG;
                    //recInfo += "オプション4=ON\r\n";
                }
                else
                {
                    _ceventret.Option4 = _utl.OFFFLG;
                    //recInfo += "オプション4=OFF\r\n";
                }
            }

            // オプション5
            if (_opt5Flg.Equals(_utl.ONFLG))
            {
                if (EI_Opt5Switch.IsToggled)
                {
                    _ceventret.Option5 = _utl.ONFLG;
                    //recInfo += "オプション5=ON\r\n";
                }
                else
                {
                    _ceventret.Option5 = _utl.OFFFLG;
                    //recInfo += "オプション5=OFF\r\n";
                }
            }

            // 遅刻
            if (EI_Late.IsToggled == true)
            {
                _ceventret.AnswerLate = _utl.ONFLG;
                //recInfo += "遅刻=ON\r\n";
            }
            else
            {
                _ceventret.AnswerLate = string.Empty;
                //recInfo += "遅刻=OFF\r\n";
            }

            // 早退
            if (EI_Early.IsToggled == true)
            {
                _ceventret.AnswerEarly = _utl.ONFLG;
                //recInfo += "早退=ON\r\n";
            }
            else
            {
                _ceventret.AnswerEarly = string.Empty;
                //recInfo += "早退=OFF\r\n";
            }

            // オンライン参加
            if (_onlineFlg.Equals(_utl.ONFLG))
            {
                if (EI_Online.IsToggled)
                {
                    _ceventret.Online = _utl.ONFLG;
                    //recInfo += "オンライン参加=ON\r\n";
                }
                else
                {
                    _ceventret.Online = string.Empty;
                    //recInfo += "オンライン参加=OFF\r\n";
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出席：年間例会スケジュール
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Present_Meeting()
        {
            //string recInfo = string.Empty;

            // オプション1
            if (_opt1Flg.Equals(_utl.ONFLG))
            {
                if (MI_Opt1Switch.IsToggled)
                {
                    _ceventret.Option1 = _utl.ONFLG;
                    //recInfo += "オプション1=ON\r\n";
                }
                else
                {
                    _ceventret.Option1 = _utl.OFFFLG;
                    //recInfo += "オプション1=OFF\r\n";
                }
            }

            // オプション2
            if (_opt2Flg.Equals(_utl.ONFLG))
            {
                if (MI_Opt2Switch.IsToggled)
                {
                    _ceventret.Option2 = _utl.ONFLG;
                    //recInfo += "オプション2=ON\r\n";
                }
                else
                {
                    _ceventret.Option2 = _utl.OFFFLG;
                    //recInfo += "オプション2=OFF\r\n";
                }
            }

            // オプション3
            if (_opt3Flg.Equals(_utl.ONFLG))
            {
                if (MI_Opt3Switch.IsToggled)
                {
                    _ceventret.Option3 = _utl.ONFLG;
                    //recInfo += "オプション3=ON\r\n";
                }
                else
                {
                    _ceventret.Option3 = _utl.OFFFLG;
                    //recInfo += "オプション3=OFF\r\n";
                }
            }

            // オプション4
            if (_opt4Flg.Equals(_utl.ONFLG))
            {
                if (MI_Opt4Switch.IsToggled)
                {
                    _ceventret.Option4 = _utl.ONFLG;
                    //recInfo += "オプション4=ON\r\n";
                }
                else
                {
                    _ceventret.Option4 = _utl.OFFFLG;
                    //recInfo += "オプション4=OFF\r\n";
                }
            }

            // オプション5
            if (_opt5Flg.Equals(_utl.ONFLG))
            {
                if (MI_Opt5Switch.IsToggled)
                {
                    _ceventret.Option5 = _utl.ONFLG;
                    //recInfo += "オプション5=ON\r\n";
                }
                else
                {
                    _ceventret.Option5 = _utl.OFFFLG;
                    //recInfo += "オプション5=OFF\r\n";
                }
            }

            // 遅刻
            if (MI_Late.IsToggled)
            {
                _ceventret.AnswerLate = _utl.ONFLG;
                //recInfo += "遅刻=ON\r\n";
            }
            else
            {
                _ceventret.AnswerLate = string.Empty;
                //recInfo += "遅刻=OFF\r\n";
            }

            // 早退
            if (MI_Early.IsToggled)
            {
                _ceventret.AnswerEarly = _utl.ONFLG;
                //recInfo += "早退=ON\r\n";
            }
            else
            {
                _ceventret.AnswerEarly = string.Empty;
                //recInfo += "早退=OFF\r\n";
            }

            // オンライン参加
            if (_onlineFlg.Equals(_utl.ONFLG))
            {
                if (MI_Online.IsToggled)
                {
                    _ceventret.Online = _utl.ONFLG;
                    //recInfo += "オンライン参加=ON\r\n";
                }
                else
                {
                    _ceventret.Online = string.Empty;
                    //recInfo += "オンライン参加=OFF\r\n";
                }
            }

            // 本人以外の参加数
            if (_oCntFlg.Equals(_utl.ONFLG))
            {
                _ceventret.OtherCount = int.Parse(MI_OtherUser.Items[MI_OtherUser.SelectedIndex]);
                //recInfo += "人数=" + MI_OtherUser.Items[MI_OtherUser.SelectedIndex] + "\r\n";
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Present_Direct()
        {

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

            // 回答（欠席）
            _ceventret.Answer = _utl.ANSWER_AB;

            // 欠席のため各値に初期値を設定
            _ceventret.AnswerLate = string.Empty;
            _ceventret.AnswerEarly = string.Empty;
            _ceventret.Online = string.Empty;
            _ceventret.Option1= string.Empty;
            _ceventret.Option2 = string.Empty;
            _ceventret.Option3 = string.Empty;
            _ceventret.Option4 = string.Empty;
            _ceventret.Option5 = string.Empty;
            _ceventret.OtherCount = 0;

            //DisplayAlert("Disp", $"Answer : {_ceventret.Answer}" + Environment.NewLine +
            //                     $"AnswerLate : {_ceventret.AnswerLate}" + Environment.NewLine +
            //                     $"AnswerEarly : {_ceventret.AnswerEarly}" + Environment.NewLine +
            //                     $"Online : {_ceventret.Online}" + Environment.NewLine +
            //                     $"Option1 : {_ceventret.Option1}" + Environment.NewLine +
            //                     $"Option2 : {_ceventret.Option2}" + Environment.NewLine +
            //                     $"Option3 : {_ceventret.Option3}" + Environment.NewLine +
            //                     $"Option4 : {_ceventret.Option4}" + Environment.NewLine +
            //                     $"Option5 : {_ceventret.Option5}" + Environment.NewLine +
            //                     $"OtherCount : {_ceventret.OtherCount}" + Environment.NewLine
            //             , "OK");

            // 出欠情報をコンテンツに設定
            _icom.SetContentToEVENTRET(_ceventret);
            try
            {
                // SQLServerへ登録
                Task<HttpResponseMessage> response = _icom.AsyncPostTextForWebAPI();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLServer 欠席情報登録エラー : {ex.Message}", "OK");
            }

            try
            {
                // SQLiteへ登録
                SetEventRetSQlite();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite 欠席情報登録エラー : {ex.Message}", "OK");
            }

            DisplayAlert("出欠確認", $"欠席情報を登録しました。", "OK");

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// フォントサイズ設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetFontSize()
        {
            double wfFontSizse = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            ////////////////////////
            // 表示項目

            // キャビネットイベント表示
            this.lbl_ED_EventDate.FontSize = wfFontSizse;
            this.ED_EventDate.FontSize = wfFontSizse;
            this.lbl_ED_EventTime.FontSize = wfFontSizse;
            this.ED_EventTime.FontSize = wfFontSizse;
            this.lbl_ED_EventPlace.FontSize = wfFontSizse;
            this.ED_EventPlace.FontSize = wfFontSizse;
            this.lbl_ED_Title.FontSize = wfFontSizse;
            this.ED_Title.FontSize = wfFontSizse;
            this.lbl_ED_Body.FontSize = wfFontSizse;
            this.ED_Body.FontSize = wfFontSizse;
            this.lbl_ED_Sake.FontSize = wfFontSizse;
            this.ED_Sake.FontSize = wfFontSizse;
            this.lbl_ED_Meeting.FontSize = wfFontSizse;
            this.ED_Meeting.FontSize = wfFontSizse;
            this.lbl_ED_MeetingUrl.FontSize = wfFontSizse;
            this.ED_MeetingUrl.FontSize = wfFontSizse;
            this.lbl_ED_MeetingID.FontSize = wfFontSizse;
            this.ED_MeetingID.FontSize = wfFontSizse;
            this.lbl_ED_Meeting.FontSize = wfFontSizse;
            this.ED_Meeting.FontSize = wfFontSizse;
            this.lbl_ED_MeetingPW.FontSize = wfFontSizse;
            this.ED_MeetingPW.FontSize = wfFontSizse;
            this.lbl_ED_MeetingOther.FontSize = wfFontSizse;
            this.ED_MeetingOther.FontSize = wfFontSizse;
            this.lbl_ED_File.FontSize = wfFontSizse;
            this.ED_File.FontSize = wfFontSizse;

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

            // キャビネットイベント入力
            this.lbl_EI_Header.FontSize = wfFontSizse;
            this.lbl_EI_Opt1Name.FontSize = wfFontSizse;
            this.lbl_EI_Opt2Name.FontSize = wfFontSizse;
            this.lbl_EI_Opt3Name.FontSize = wfFontSizse;
            this.lbl_EI_Opt4Name.FontSize = wfFontSizse;
            this.lbl_EI_Opt5Name.FontSize = wfFontSizse;
            this.lbl_EI_Late.FontSize = wfFontSizse;
            this.lbl_EI_Early.FontSize = wfFontSizse;
            this.lbl_EI_Online.FontSize = wfFontSizse;
            this.lbl_EI_AnsDate.FontSize = wfFontSizse;
            this.EI_AnsDate.FontSize = wfFontSizse;
            this.lbl_EI_footer.FontSize = wfFontSizse;

            // 年間例会スケジュール入力
            this.lbl_MI_Header.FontSize = wfFontSizse;
            this.lbl_MI_Opt1Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt2Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt3Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt4Name.FontSize = wfFontSizse;
            this.lbl_MI_Opt5Name.FontSize = wfFontSizse;
            this.lbl_MI_Late.FontSize = wfFontSizse;
            this.lbl_MI_Early.FontSize = wfFontSizse;
            this.lbl_MI_Online.FontSize = wfFontSizse;
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
            string wkDrEventclass = string.Empty;

            try
            {
                ////////////////////////////////////////////////////////////////
                // 指定データNoのイベント出欠情報（T_EVENTRET）を取得する
                foreach (Table.T_EVENTRET row in _sqlite.Get_T_EVENTRET("Select * " +
                                                                    "From T_EVENTRET " +
                                                                    "Where DataNo='" + _dataNo + "'"))
                {
                    _eventret = row;
                }

                // イベント区分を取得する
                _eventClass = _utl.GetString(_eventret.EventClass);

                ////////////////////////////////////////////////////////////////
                // イベント別のデータ読み込み・タイトル文字列の設定・各画面の設定を行う

                // キャビネットイベント
                if (_eventClass.Equals(_utl.EVENTCLASS_CV))
                {
                    // キャビネットイベント情報（T_EVENT）を取得する
                    foreach (Table.T_EVENT row in _sqlite.Get_T_EVENT("Select * " +
                                                                      "From T_EVENT " +
                                                                      "Where DataNo='" + _eventDataNo + "'"))
                    {
                        _event = row;
                    }
                    // タイトル文字列の設定
                    BodyTitle.Text = ST_EVENT_1;

                    // キャビネットイベント画面の設定
                    SetCEventPageCabinet();

                }
                // 年間例会スケジュール
                else if (_eventClass.Equals(_utl.EVENTCLASS_CL))
                {
                    // 年間例会スケジュール情報（T_MEETINGSCHEDULE）を取得する
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
                else if (_eventClass.Equals(_utl.EVENTCLASS_DR))
                {
                    // 理事・委員会情報（T_DRECTOR）を取得する
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
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// キャビネットイベント項目の設定
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetCEventPageCabinet()
        {
            string wkDate = string.Empty;
            string wkCancel = string.Empty;
            string wkTime = string.Empty;
            string wkRecTime = string.Empty;
            string wkPlace = string.Empty;
            string wkTitle = string.Empty;
            string wkBody = string.Empty;
            string wkSake = string.Empty;
            string wkMeeting = string.Empty;
            string wkUrl = string.Empty;
            string wkID = string.Empty;
            string wkPW = string.Empty;
            string wkMtOther = string.Empty;
            string wkOpt1Name = string.Empty;
            string wkOpt2Name = string.Empty;
            string wkOpt3Name = string.Empty;
            string wkOpt4Name = string.Empty;
            string wkOpt5Name = string.Empty;
            string wkAnsDate = string.Empty;

            ////////////////////////////////////////////////////////////////
            // 表示項目

            // 開催日
            wkDate = _utl.GetString(_event.EventDate).Substring(0, 10);

            // 中止
            wkCancel = _utl.StrCancel(_eventret.CancelFlg);

            // 開催日時
            wkTime = _utl.GetString(_event.EventTimeStart) + "～" +
                     _utl.GetString(_event.EventTimeEnd);

            // 受付時間
            wkRecTime = _utl.GetString(_event.ReceptionTime);

            // 開催場所
            wkPlace = _utl.GetString(_event.EventPlace);

            // 件名
            wkTitle = _utl.GetString(_event.Title);

            // 内容
            wkBody = _utl.GetString(_event.Body, _utl.NLC_ON);

            // お酒
            wkSake = _utl.StrOnOff(_event.Sake);

            // 会議方法
            wkMeeting = _utl.StrOnline(_event.Meeting);

            // URL
            wkUrl = _utl.GetString(_event.MeetingUrl);

            // MT ID
            wkID = _utl.GetString(_event.MeetingID);

            // パスワード
            wkPW = _utl.GetString(_event.MeetingPW);

            // 備考
            wkMtOther = _utl.GetString(_event.MeetingOther, _utl.NLC_ON);

            // 詳細資料
            _fileName = _utl.GetString(_event.FileName);


            ////////////////////////////////////////////////////////////////
            // 入力項目

            // オプション1
            _opt1Flg = _utl.GetString(_event.OptionRadio1);
            if (_opt1Flg.Equals(_utl.ONFLG))
            {
                // オプション1（項目名）
                wkOpt1Name = _utl.GetString(_event.OptionName1);

                // オプション1（入力値）
                _ceventret.Option1 = _utl.GetString(_eventret.Option1);
            }
            // オプション2
            _opt2Flg = _utl.GetString(_event.OptionRadio2);
            if (_opt2Flg.Equals(_utl.ONFLG))
            {
                // オプション2（項目名）
                wkOpt2Name = _utl.GetString(_event.OptionName2);

                // オプション2（入力値）
                _ceventret.Option2 = _utl.GetString(_eventret.Option2);
            }
            // オプション3
            _opt3Flg = _utl.GetString(_event.OptionRadio3);
            if (_opt3Flg.Equals(_utl.ONFLG))
            {
                // オプション3（項目名）
                wkOpt3Name = _utl.GetString(_event.OptionName3);

                // オプション3（入力値）
                _ceventret.Option3 = _utl.GetString(_eventret.Option3);
            }
            // オプション4
            _opt4Flg = _utl.GetString(_event.OptionRadio4);
            if (_opt4Flg.Equals(_utl.ONFLG))
            {
                // オプション4（項目名）
                wkOpt4Name = _utl.GetString(_event.OptionName4);

                // オプション4（入力値）
                _ceventret.Option4 = _utl.GetString(_eventret.Option4);
            }
            // オプション5
            _opt5Flg = _utl.GetString(_event.OptionRadio5);
            if (_opt5Flg.Equals(_utl.ONFLG))
            {
                // オプション5（項目名）
                wkOpt5Name = _utl.GetString(_event.OptionName5);

                // オプション5（入力値）
                _ceventret.Option5 = _utl.GetString(_eventret.Option5);
            }

            // 遅刻
            _ceventret.AnswerLate = _utl.GetString(_eventret.AnswerLate);

            // 早退
            _ceventret.AnswerEarly = _utl.GetString(_eventret.AnswerEarly);

            // オンライン参加
            _onlineFlg = _utl.GetString(_event.Meeting);
            _ceventret.Online = _utl.GetString(_eventret.Online);

            // 回答期限
            wkAnsDate = _utl.GetString(_event.AnswerDate).Substring(0, 10);


            ////////////////////////////////////////////////////////////////
            // 表示項目設定

            ED_EventDate.Text = wkDate;
            ED_Cancel.Text = wkCancel;
            ED_EventTime.Text = wkTime;
            ED_ReceptionTime.Text = wkRecTime;
            ED_EventPlace.Text = wkPlace;
            ED_Title.Text = wkTitle;
            ED_Body.Text = wkBody;
            ED_Sake.Text = wkSake;

            // 会議方法～備考
            if (wkMeeting.Equals(_utl.ST_MEETING_ONLINE))
            {
                ED_Meeting.Text = wkMeeting;
                ED_MeetingUrl.Text = wkUrl;
                ED_MeetingID.Text = wkID;
                ED_MeetingPW.Text = wkPW;
                ED_MeetingOther.Text = wkMtOther;
            }
            else
            {
                // 会議方法～備考項目非表示
                lbl_ED_Meeting.IsVisible = false;
                ED_Meeting.IsVisible = false;
                lbl_ED_MeetingUrl.IsVisible = false;
                ED_MeetingUrl.IsVisible = false;
                lbl_ED_MeetingID.IsVisible = false;
                ED_MeetingID.IsVisible = false;
                lbl_ED_MeetingPW.IsVisible = false;
                ED_MeetingPW.IsVisible = false;
                lbl_ED_MeetingOther.IsVisible = false;
                ED_MeetingOther.IsVisible = false;
            }


            ////////////////////////////////////////////////////////////////
            // 入力項目設定

            // オプション1
            SetOptionItem(_opt1Flg, _ceventret.Option1, wkOpt1Name, ref lbl_EI_Opt1Name, ref EI_Opt1Switch);

            // オプション2
            SetOptionItem(_opt2Flg, _ceventret.Option2, wkOpt2Name, ref lbl_EI_Opt2Name, ref EI_Opt2Switch);

            // オプション3
            SetOptionItem(_opt3Flg, _ceventret.Option3, wkOpt3Name, ref lbl_EI_Opt3Name, ref EI_Opt3Switch);

            // オプション4
            SetOptionItem(_opt4Flg, _ceventret.Option4, wkOpt4Name, ref lbl_EI_Opt4Name, ref EI_Opt4Switch);

            // オプション5
            SetOptionItem(_opt5Flg, _ceventret.Option5, wkOpt5Name, ref lbl_EI_Opt5Name, ref EI_Opt5Switch);


            // 遅刻
            EI_Late.IsToggled = _ceventret.AnswerLate.Equals(_utl.ONFLG);

            // 早退
            EI_Early.IsToggled = _ceventret.AnswerEarly.Equals(_utl.ONFLG);

            // オンライン参加
            if (_onlineFlg.Equals(_utl.ONFLG))
            {
                EI_Online.IsToggled = _ceventret.Online.Equals(_utl.ONFLG);
            }
            else
            {
                // オンライン参加項目非表示
                lbl_EI_Online.IsVisible = false;
                EI_Online.IsVisible = false;
            }

            // 回答期限
            EI_AnsDate.Text = wkAnsDate;

            // 年間例会スケジュール項目非表示
            MeetingDspSL.IsVisible = false;
            MeetingInpSL.IsVisible = false;

            // 理事・委員会項目非表示
            DirectDspSL.IsVisible = false;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 年間例会スケジュール項目の設定
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
            string wkOpt1Name = string.Empty;
            string wkOpt2Name = string.Empty;
            string wkOpt3Name = string.Empty;
            string wkOpt4Name = string.Empty;
            string wkOpt5Name = string.Empty;
            string wkAnsDate = string.Empty;

            ////////////////////////////////////////////////////////////////
            // 表示項目

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

            ////////////////////////////////////////////////////////////////
            // 入力項目

            // オプション1
            _opt1Flg = _utl.GetString(_meetingschedule.OptionRadio1);
            if (_opt1Flg.Equals(_utl.ONFLG))
            {
                // オプション1（項目名）
                wkOpt1Name = _utl.GetString(_meetingschedule.OptionName1);

                // オプション1（入力値）
                _ceventret.Option1 = _utl.GetString(_eventret.Option1);
            }
            // オプション2
            _opt2Flg = _utl.GetString(_meetingschedule.OptionRadio2);
            if (_opt2Flg.Equals(_utl.ONFLG))
            {
                // オプション2（項目名）
                wkOpt2Name = _utl.GetString(_meetingschedule.OptionName2);

                // オプション2（入力値）
                _ceventret.Option2 = _utl.GetString(_eventret.Option2);
            }
            // オプション3
            _opt3Flg = _utl.GetString(_meetingschedule.OptionRadio3);
            if (_opt3Flg.Equals(_utl.ONFLG))
            {
                // オプション3（項目名）
                wkOpt3Name = _utl.GetString(_meetingschedule.OptionName3);

                // オプション3（入力値）
                _ceventret.Option3 = _utl.GetString(_eventret.Option3);
            }
            // オプション4
            _opt4Flg = _utl.GetString(_meetingschedule.OptionRadio4);
            if (_opt4Flg.Equals(_utl.ONFLG))
            {
                // オプション4（項目名）
                wkOpt4Name = _utl.GetString(_meetingschedule.OptionName4);

                // オプション4（入力値）
                _ceventret.Option4 = _utl.GetString(_eventret.Option4);
            }
            // オプション5
            _opt5Flg = _utl.GetString(_meetingschedule.OptionRadio5);
            if (_opt5Flg.Equals(_utl.ONFLG))
            {
                // オプション5（項目名）
                wkOpt5Name = _utl.GetString(_meetingschedule.OptionName5);

                // オプション5（入力値）
                _ceventret.Option5 = _utl.GetString(_eventret.Option5);
            }

            // 遅刻
            _ceventret.AnswerLate = _utl.GetString(_eventret.AnswerLate);

            // 早退
            _ceventret.AnswerEarly = _utl.GetString(_eventret.AnswerEarly);

            // オンライン参加
            _onlineFlg = _utl.GetString(_meetingschedule.Online);
            _ceventret.Online = _utl.GetString(_eventret.Online);

            // 本人以外の参加数
            _oCntFlg = _utl.GetString(_meetingschedule.OtherUser);
            _ceventret.OtherCount = _eventret.OtherCount;

            // 回答期限
            wkAnsDate = _utl.GetString(_meetingschedule.AnswerDate).Substring(0, 10);


            ////////////////////////////////////////////////////////////////
            // 表示項目設定

            MD_Date.Text = wkDate;
            MD_Cancel.Text = wkCancel;
            MD_Place.Text = wkPlace;
            MD_Count.Text = wkCount.ToString();
            MD_Name.Text = wkName;

            // 会議方法
            if (wkOnline.Equals(_utl.ST_MEETING_ONLINE))
            {
                MD_Online.Text = wkOnline;
            }
            else
            {
                // 会議方法項目非表示
                lbl_MD_Online.IsVisible = false;
                MD_Online.IsVisible = false;
            }

            MD_Sake.Text = wkSake;

            ////////////////////////////////////////////////////////////////
            // 入力項目設定

            // オプション1
            SetOptionItem(_opt1Flg, _ceventret.Option1, wkOpt1Name, ref lbl_MI_Opt1Name, ref MI_Opt1Switch);

            // オプション2
            SetOptionItem(_opt2Flg, _ceventret.Option2, wkOpt2Name, ref lbl_MI_Opt2Name, ref MI_Opt2Switch);

            // オプション3
            SetOptionItem(_opt3Flg, _ceventret.Option3, wkOpt3Name, ref lbl_MI_Opt3Name, ref MI_Opt3Switch);

            // オプション4
            SetOptionItem(_opt4Flg, _ceventret.Option4, wkOpt4Name, ref lbl_MI_Opt4Name, ref MI_Opt4Switch);

            // オプション5
            SetOptionItem(_opt5Flg, _ceventret.Option5, wkOpt5Name, ref lbl_MI_Opt5Name, ref MI_Opt5Switch);

            // 遅刻
            MI_Late.IsToggled = _ceventret.AnswerLate.Equals(_utl.ONFLG);

            // 早退
            MI_Early.IsToggled = _ceventret.AnswerEarly.Equals(_utl.ONFLG);

            // オンライン参加
            if (_onlineFlg.Equals(_utl.ONFLG))
            {
                MI_Online.IsToggled = _ceventret.Online.Equals(_utl.ONFLG); 
            }
            else
            {
                // オンライン参加項目非表示
                lbl_MI_Online.IsVisible = false;
                MI_Online.IsVisible = false;
            }

            // 本人以外の参加数
            if (_oCntFlg.Equals(_utl.ONFLG))
            {
                for (int idx = 0; idx <= 15; idx++)
                {
                    MI_OtherUser.Items.Add(idx.ToString());
                }
                MI_OtherUser.SelectedIndex = _ceventret.OtherCount;
            }
            else
            {
                // 本人以外の参加数項目非表示
                lbl_MI_OtherUser.IsVisible = false;
                MI_OtherUser.IsVisible = false;
            }

            // 回答期限
            MI_AnsDate.Text = wkAnsDate;

            // キャビネットイベント項目非表示
            EventDspSL.IsVisible = false;
            EventInpSL.IsVisible = false;

            // 理事・委員会項目非表示
            DirectDspSL.IsVisible = false;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会項目の設定
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

            ////////////////////////////////////////////////////////////////
            // 表示項目

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


            ////////////////////////////////////////////////////////////////
            // 表示項目設定

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

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// オプション項目の設定
        /// </summary>
        /// <param name="flg"></param>
        /// <param name="radio"></param>
        /// <param name="name"></param>
        /// <param name="labelItem"></param>
        /// <param name="switchItem"></param>
        /// <param name="rowdef"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetOptionItem(string flg, 
                                   string radio, 
                                   string name, 
                                   ref Label labelItem, 
                                   ref Switch switchItem)
        {
            // 入力項目設定
            // オプションが有効である場合
            if (flg.Equals(_utl.ONFLG))
            {
                labelItem.Text = name;
                // オプションがONに設定されている、もしくは設定がない場合
                if (radio.Equals(_utl.ONFLG) || radio.Equals(_utl.NOFLG))
                {
                    // SwitchをONで表示
                    switchItem.IsToggled = true;
                }
                // オプションがOFFに設定されている場合
                else if (radio.Equals(_utl.OFFFLG))
                {
                    // SwitchをOFFで表示
                    switchItem.IsToggled = false;
                }
            }
            // オプションが有効でない場合
            else
            {
                // オプション項目非表示
                labelItem.IsVisible = false;
                switchItem.IsVisible = false;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出欠情報登録（SQLite）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetEventRetSQlite()
        {
            string _answer = _utl.GetSQLString(_ceventret.Answer);
            string _answerLate = _utl.GetSQLString(_ceventret.AnswerLate);
            string _answerEarly = _utl.GetSQLString(_ceventret.AnswerEarly);
            string _online = _utl.GetSQLString(_ceventret.Online);
            string _option1 = _utl.GetSQLString(_ceventret.Option1);
            string _option2 = _utl.GetSQLString(_ceventret.Option2);
            string _option3 = _utl.GetSQLString(_ceventret.Option3);
            string _option4 = _utl.GetSQLString(_ceventret.Option4);
            string _option5 = _utl.GetSQLString(_ceventret.Option5);
            string _otherCount = _ceventret.OtherCount.ToString();

            foreach (Table.T_EVENTRET row in _sqlite.Set_T_EVENTRET("UPDATE T_EVENTRET SET" +
                                                                    " Answer = " + _answer + ", " +
                                                                    " AnswerLate = " + _answerLate + ", " +
                                                                    " AnswerEarly = " + _answerEarly + ", " +
                                                                    " Online = " + _online + ", " +
                                                                    " Option1 = " + _option1 + ", " +
                                                                    " Option2 = " + _option2 + ", " +
                                                                    " Option3 = " + _option3 + ", " +
                                                                    " Option4 = " + _option4 + ", " +
                                                                    " Option5 = " + _option5 + ", " +
                                                                    " OtherCount = " + _otherCount + " " +
                                                                    "WHERE DataNo = '" + _dataNo + "'"))
            {
                _eventret = row;
            }
        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// キャビネットイベント表示用クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    //public sealed class CEventPageEvent
    //{
    //    public CEventPageEvent(string date,
    //                              string cancel,
    //                              string stime,
    //                              string etime,
    //                              string rectime,
    //                              string place,
    //                              string title,
    //                              string body,
    //                              string sake,
    //                              string meeting,
    //                              string murl,
    //                              string mid,
    //                              string mpw,
    //                              string mother,
    //                              string fname)
    //    {
    //        Data = date;
    //        Cancel = cancel;
    //        STime = stime;
    //        ETime = etime;
    //        RecTime = rectime;
    //        Place = place;
    //        Title = title;
    //        Body = body;
    //        Sake = sake;
    //        Meeting = meeting;
    //        MUrl = murl;
    //        MId = mid;
    //        MPw = mpw;
    //        MOther = mother;
    //        FName = fname;
    //    }
    //    public string Data { get; set; }
    //    public string Cancel { get; set; }
    //    public string STime { get; set; }
    //    public string ETime { get; set; }
    //    public string RecTime { get; set; }
    //    public string Place { get; set; }
    //    public string Title { get; set; }
    //    public string Body { get; set; }
    //    public string Sake { get; set; }
    //    public string Meeting { get; set; }
    //    public string MUrl { get; set; }
    //    public string MId { get; set; }
    //    public string MPw { get; set; }
    //    public string MOther { get; set; }
    //    public string FName { get; set; }
    //}


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 年間例会スケジュール表示用クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    //public sealed class CEventPageMeeting
    //{
    //    public CEventPageMeeting(string date,
    //                            string cancel,
    //                            string place,
    //                            int count,
    //                            string name,
    //                            string online,
    //                            string sake)
    //    {
    //        Data = date;
    //        Cancel = cancel;
    //        Place = place;
    //        Count = count;
    //        Name = name;
    //        Online = online;
    //        Sake = sake;
    //    }
    //    public string Data { get; set; }
    //    public string Cancel { get; set; }
    //    public string Place { get; set; }
    //    public int Count { get; set; }
    //    public string Name { get; set; }
    //    public string Online { get; set; }
    //    public string Sake { get; set; }
    //}


    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 理事・委員会表示用クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    //public sealed class CEventPageDirect
    //{
    //    public CEventPageDirect(string date, 
    //                            string cancel, 
    //                            string season, 
    //                            string place, 
    //                            string agenda, 
    //                            string ansdate)
    //    {
    //        Data = date;
    //        Cancel = cancel;
    //        Season = season;
    //        Place = place;
    //        Agenda = agenda;
    //        AnsDate = ansdate;
    //    }
    //    public string Data { get; set; }
    //    public string Cancel { get; set; }
    //    public string Season { get; set; }
    //    public string Place { get; set; }
    //    public string Agenda { get; set; }
    //    public string AnsDate { get; set; }
    //}

}