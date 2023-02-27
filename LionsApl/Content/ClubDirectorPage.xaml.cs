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
    /// クラブ：理事・委員会ページクラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubDirectorPage : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private int _dataNo;                                // 対象データNo.
        private string _answer;                             // 出欠

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo"></param>
        /// <param name="answer"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubDirectorPage(int dataNo, string answer)
        {
            InitializeComponent();

            // font-size
            lbl_EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            EventDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            Cancel.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_Season.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            Season.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_EventClass.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            EventClass.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_EventPlace.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            EventPlace.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_Agenda.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            Agenda.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            lbl_AnswerDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            AnswerDate.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));

            // 前画面からの取得情報
            _dataNo = dataNo;           // データNo.
            _answer = answer;           // 出欠

            // Content Utilクラス生成
            _utl = new LAUtility();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 理事・委員会情報を取得する。
            GetDirectorInfo();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会情報を取得する。 
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetDirectorInfo()
        {
            string eventDate = string.Empty;        // 開催日
            string seasonFlg = string.Empty;        // シーズン区分
            string eventFlg = string.Empty;         // 区分
            string cancelFlg = string.Empty;        // 中止フラグ
            string agendaStr = string.Empty;        // 議題・内容

            // 会員情報取得
            try
            {
                foreach (Table.T_DIRECTOR row in _sqlite.Get_T_DIRECTOR("Select * " +
                                                                        "From T_DIRECTOR " +
                                                                        "Where DataNo = '" + _dataNo + "'"))
                {

                    // 中止
                    Cancel.Text = _utl.StrCancel(row.CancelFlg);
                    //cancelFlg = _utl.GetString(row.CancelFlg);
                    //if (cancelFlg == "1")
                    //{
                    //    Cancel.Text = "中止";
                    //}

                    // 開催日
                    EventDate.Text = _utl.GetDateString(row.EventDate) + " " + _utl.GetTimeString(row.EventTime);
                    //eventDate = _utl.GetDateString(row.EventDate) + " " + _utl.GetTimeString(row.EventTime);
                    //EventDate.Text = eventDate;

                    // シーズン区分
                    Season.Text = _utl.StrSeason(row.Season);
                    //seasonFlg = _utl.GetString(row.Season);
                    //if (seasonFlg == "1")
                    //{
                    //    Season.Text = "今期";
                    //}
                    //else if (seasonFlg == "2")
                    //{
                    //    Season.Text = "次期";
                    //}

                    // 区分
                    eventFlg = _utl.GetString(row.EventClass);
                    if (eventFlg == LADef.CLUBEVENTCLASS_RI)
                    {
                        EventClass.Text = "理事会";
                    }
                    else if (eventFlg == LADef.CLUBEVENTCLASS_IN)
                    {
                        EventClass.Text = _utl.GetString(row.CommitteeName);
                    }
                    else
                    {
                        EventClass.Text = "その他";
                    }

                    // 開催場所
                    EventPlace.Text = _utl.GetString(row.EventPlace);

                    // 件名
                    Subject.Text = _utl.GetString(row.Subject);

                    // 議題・内容
                    agendaStr = _utl.GetString(row.Agenda);
                    Agenda.Text = agendaStr;

                    // 回答期限
                    if (_answer == string.Empty)
                    {
                        AnswerDate.Text = _utl.GetDateString(row.AnswerDate);
                    }
                    else
                    {
                        AnswerDate.Text = _utl.GetDateString(row.AnswerDate) + " (" + _answer + ")";
                    }

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

    }
}