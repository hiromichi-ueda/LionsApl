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
        private string _DataNo;

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
            Table.TableUtil Util = new Table.TableUtil();
            ContentUtil CUtil = new ContentUtil();

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
                    cancelFlg = Util.GetString(row.CancelFlg);
                    if (cancelFlg == "1")
                    {
                        Cancel.Text = "中止";
                    }

                    // 開催日
                    eventDate = Util.GetString(row.EventDate).Substring(0, 10) + " " + Util.GetString(row.EventTime);
                    EventDate.Text = eventDate;

                    // シーズン区分
                    seasonFlg = Util.GetString(row.Season);
                    if (seasonFlg == "1")
                    {
                        Season.Text = "今期";
                    }
                    else if (seasonFlg == "2")
                    {
                        Season.Text = "次期";
                    }

                    // 区分
                    eventFlg = Util.GetString(row.EventClass);
                    if (eventFlg == "1")
                    {
                        EventClass.Text = "理事会";
                    }
                    else if (eventFlg == "2")
                    {
                        EventClass.Text = Util.GetString(row.CommitteeName);
                    }

                    // 開催場所
                    EventPlace.Text = Util.GetString(row.EventPlace);

                    // 件名
                    Subject.Text = Util.GetString(row.Subject);

                    // 議題・内容
                    //agendaStr = $"テスト用文字列\rテスト用文字列\nテスト用文字列\r\nテスト用文字列{Environment.NewLine}テスト用文字列テスト用文字列テスト用文字列テスト用文字列テスト用文字列テスト用文字列テスト用文字列";
                    agendaStr = Util.GetString(row.Agenda);
                    agendaStr = CUtil.DelNewLine(agendaStr);
                    Agenda.Text = agendaStr;

                    // 回答期限
                    AnswerDate.Text = Util.GetString(row.AnswerDate).Substring(0, 10);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

    }
}