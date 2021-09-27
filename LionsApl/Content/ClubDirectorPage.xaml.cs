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
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 前画面からの取得情報
        private string _DataNo;         // 対象データNo.


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNo"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubDirectorPage(string dataNo)
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

            // 対象データNo.設定
            _DataNo = dataNo;

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
                                                                        "Where DataNo = '" + _DataNo + "'"))
                {

                    // 中止
                    cancelFlg = _utl.GetString(row.CancelFlg);
                    if (cancelFlg == "1")
                    {
                        Cancel.Text = "中止";
                    }

                    // 開催日
                    eventDate = _utl.GetString(row.EventDate).Substring(0, 10) + " " + _utl.GetString(row.EventTime);
                    EventDate.Text = eventDate;

                    // シーズン区分
                    seasonFlg = _utl.GetString(row.Season);
                    if (seasonFlg == "1")
                    {
                        Season.Text = "今期";
                    }
                    else if (seasonFlg == "2")
                    {
                        Season.Text = "次期";
                    }

                    // 区分
                    eventFlg = _utl.GetString(row.EventClass);
                    if (eventFlg == "1")
                    {
                        EventClass.Text = "理事会";
                    }
                    else if (eventFlg == "2")
                    {
                        EventClass.Text = _utl.GetString(row.CommitteeName);
                    }

                    // 開催場所
                    EventPlace.Text = _utl.GetString(row.EventPlace);

                    // 件名
                    Subject.Text = _utl.GetString(row.Subject);

                    // 議題・内容
                    agendaStr = _utl.GetString(row.Agenda, _utl.NLC_ON);
                    Agenda.Text = agendaStr;

                    // 回答期限
                    AnswerDate.Text = _utl.GetString(row.AnswerDate).Substring(0, 10);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

    }
}