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
    public partial class ClubDirectorList : ContentPage
    {
        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // リストビュー設定内容
        public List<ClubDirectorRow> Items { get; set; }

        public ClubDirectorList()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 理事・委員会データ取得
            GetClubDirector();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 理事・委員会情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetClubDirector()
        {
            int wkDataNo = 0;
            string wkEventClass = string.Empty;
            string wkEventDate = string.Empty;
            string wkEventTime = string.Empty;
            string wkSubject = string.Empty;
            string wkCancel = string.Empty;
            string wkAnswer = string.Empty;
            string[] wkUserList = null;
            bool wkTargetFlg = false;
            Items = new List<ClubDirectorRow>();

            try
            {

                foreach (Table.DIRECTOR_LIST row in _sqlite.Get_DIRECTOR_LIST(
                                                                "SELECT " +
                                                                "TD.DataNo, " +
                                                                "TD.EventClass, " +
                                                                "TD.EventDate, " +
                                                                "TD.EventTime, " +
                                                                "TD.Subject, " +
                                                                "TD.Member, " +
                                                                "TD.MemberAdd, " +
                                                                "TD.CancelFlg, " +
                                                                "TE.Answer " +
                                                                "FROM T_DIRECTOR TD " +
                                                                "LEFT OUTER JOIN T_EVENTRET TE " +
                                                                "ON TD.DataNo = TE.EventDataNo AND TE.EventClass = '3'" +
                                                                "ORDER BY TD.EventDate DESC"))
                {
                    // DataNo
                    wkDataNo = row.DataNo;

                    // 区分
                    if (_utl.GetString(row.EventClass) == "1")
                    {
                        wkEventClass = _utl.ST_BOARD;
                    }
                    else
                    {
                        wkEventClass = _utl.ST_COMM;
                    }

                    // 開催日時
                    wkEventTime = _utl.GetTimeString(row.EventTime);
                    if (wkEventTime == "00:00")
                    {
                        wkEventDate = _utl.GetString(row.EventDate).Substring(0, 10);
                    }
                    else
                    {
                        wkEventDate = _utl.GetString(row.EventDate).Substring(0, 10) + " " + wkEventTime;
                    }

                    // 件名
                    wkSubject = _utl.GetString(row.Subject);

                    // 中止
                    wkCancel = "";
                    if(_utl.GetString(row.CancelFlg) == "1")
                    {
                        wkCancel = LADef.ST_CANCEL;
                    }

                    // ログインユーザーが対象か判定
                    wkTargetFlg = false;
                    wkUserList = _utl.GetString(row.Member).Split(',');
                    foreach (string code in wkUserList)
                    {
                        // [メンバー]を検索
                        if (_sqlite.Db_A_Account.MemberCode.Equals(code))
                        {
                            wkTargetFlg = true;
                            break;
                        }
                    }

                    wkUserList = _utl.GetString(row.MemberAdd).Split(',');
                    foreach (string code in wkUserList)
                    {
                        // [メンバー追加]を検索
                        if (_sqlite.Db_A_Account.MemberCode.Equals(code))
                        {
                            wkTargetFlg = true;
                            break;
                        }
                    }

                    // 回答
                    wkAnswer = "";
                    if (wkTargetFlg)
                    {
                        // 対象者の場合は回答をセット
                        if (_utl.GetString(row.Answer).Equals(LADef.ANSWER_PRE))
                        {
                            // 出席
                            wkAnswer = LADef.ST_ANSWER_PRE;
                        }
                        else if (_utl.GetString(row.Answer).Equals(LADef.ANSWER_ABS))
                        {
                            // 欠席
                            wkAnswer = LADef.ST_ANSWER_ABS;
                        }
                        else
                        {
                            // 未回答
                            wkAnswer = LADef.ST_ANSWER_NO;
                        }
                    }

                    // List DataSet
                    Items.Add(new ClubDirectorRow(wkDataNo, wkEventClass, wkEventDate, wkSubject, wkAnswer, wkCancel));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubDirectorRow(0, wkEventClass, wkEventDate, wkSubject, wkAnswer, wkCancel));
                }
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
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

            ClubDirectorRow item = e.Item as ClubDirectorRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (item.DataNo == 0)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 理事・委員会画面へ
            Navigation.PushAsync(new ClubDirectorPage(item.DataNo));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }

    public sealed class ClubDirectorRow
    {
        public ClubDirectorRow(int datano, 
                               string eventclass, 
                               string eventdate, 
                               string subject, 
                               string answer, 
                               string cancelflg)
        {
            DataNo = datano;
            EventClass = eventclass;
            EventDate = eventdate;
            Subject = subject;
            Answer = answer;
            CancelFlg = cancelflg;
            if (Answer.Equals(LADef.ST_ANSWER_NO))
            {
                AnswerColor = LADef.STRCOL_RED;
            }
            else
            {
                AnswerColor = LADef.STRCOL_STRDEF;
            }
        }
        public int DataNo { get; set; }
        public string EventClass { get; set; }
        public string EventDate { get; set; }
        public string Subject { get; set; }
        public string CancelFlg { get; set; }
        public string Answer { get; set; }
        public string AnswerColor { get; set; }
    }

    public class MyClubDirectorSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubDirectorRow)item;
            if (info.DataNo != 0)
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