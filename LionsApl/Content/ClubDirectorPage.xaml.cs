using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubDirectorPage : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // 対象データNo.
        private int _DataNo;
        // 対象EbentRetデータ
        private Table.T_EVENTRET _EventRet = null;

        public ClubDirectorPage(int dataNo)
        {
            InitializeComponent();

            // font-size
            DirectorTitle.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            Cancel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_Season.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            Season.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_EventPlace.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            EventPlace.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_Agenda.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            Agenda.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_AnswerDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            AnswerDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 対象データNo.設定
            _DataNo = dataNo;

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

            // 理事・委員会情報を取得する。
            GetDirectorInfo();

            // イベント出欠情報を取得する。
            GetEventRetInfo();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会情報を取得する。 
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetDirectorInfo()
        {
            Table.TableUtil TUtl = new Table.TableUtil();
            ContentUtil CUtl = new ContentUtil();

            string eventDate = string.Empty;        // 開催日
            string seasonFlg = string.Empty;        // シーズン区分
            string cancelFlg = string.Empty;        // 中止フラグ
            string agendaStr = string.Empty;        // 議題・内容

            // 会員情報取得
            try
            {
                foreach (Table.T_DIRECTOR row in _sqlite.Get_T_DIRECTOR("Select * " +
                                                                        "From T_DIRECTOR " +
                                                                        "Where DataNo = '" + _DataNo + "'"))
                {
                    // 出欠タイトル
                    if (TUtl.GetString(row.EventClass) == "1")
                    {
                        DirectorTitle.Text = "理事会出欠の確認";
                    }
                    else
                    {
                        DirectorTitle.Text = "委員会出欠の確認";
                    }

                    // 中止
                    cancelFlg = TUtl.GetString(row.CancelFlg);
                    //cancelFlg = "1";
                    if (cancelFlg == "1")
                    {
                        Cancel.Text = "中止";
                    }

                    // 開催日
                    eventDate = TUtl.GetString(row.EventDate).Substring(0, 10) + " " + TUtl.GetString(row.EventTime);
                    EventDate.Text = eventDate;

                    // 区分
                    seasonFlg = TUtl.GetString(row.Season);
                    if (seasonFlg == "1")
                    {
                        Season.Text = "今期";
                    }
                    else if (seasonFlg == "2")
                    {
                        Season.Text = "次期";
                    }

                    // 開催場所
                    EventPlace.Text = TUtl.GetString(row.EventPlace);

                    // 議題・内容
                    //agendaStr = $"テスト用文字列\rテスト用文字列\nテスト用文字列\r\nテスト用文字列{Environment.NewLine}テスト用文字列テスト用文字列テスト用文字列テスト用文字列テスト用文字列テスト用文字列テスト用文字列";
                    agendaStr = TUtl.GetString(row.Agenda);
                    agendaStr = CUtl.DelNewLine(agendaStr);
                    Agenda.Text = agendaStr;

                    // 回答期限
                    AnswerDate.Text = TUtl.GetString(row.AnswerDate).Substring(0, 10);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント出欠を取得する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetEventRetInfo()
        {
            //Table.TableUtil Util = new Table.TableUtil();
            int wDataNo;

            // 会員情報取得
            try
            {
                foreach (Table.T_EVENTRET row in _sqlite.Get_T_EVENTRET("Select * " +
                                                                        "From T_EVENTRET " +
                                                                        "Where " +
                                                                        "EventClass = '3' AND " +
                                                                        "EventDataNo = '" + _DataNo + "'"))
                {
                    _EventRet = row;
                    wDataNo = _EventRet.DataNo;
                }
                if (_EventRet == null)
                {
                    DisplayAlert("理事・委員会", "イベント情報がありません。", "OK");
                    btn_attendance.IsEnabled = false;
                    btn_absence.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_EVENTRET) : &{ex.Message}", "OK");
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出席ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Attendance_Button_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Alert", "出席", "OK");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 欠席ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Absence_Button_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Alert", "欠席", "OK");
        }


    }
}