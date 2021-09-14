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

        // リストビュー設定内容
        public List<ClubDirectorRow> Items { get; set; }

        public ClubDirectorList()
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
            string wkDataNo = string.Empty;
            string wkEventClass = string.Empty;
            string wkEventDate = string.Empty;
            string wkEventTime = string.Empty;
            string wkSubject = string.Empty;
            string wkCancel = string.Empty;
            string wkAnswer = string.Empty;
            string[] wkUserList = null;
            bool wkTargetFlg = false;
            Items = new List<ClubDirectorRow>();

            Table.TableUtil Util = new Table.TableUtil();

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
                    wkDataNo = row.DataNo.ToString();

                    // 区分
                    if (Util.GetString(row.EventClass) == "1")
                    {
                        wkEventClass = "理事会";
                    }
                    else
                    {
                        wkEventClass = "委員会";
                    }

                    // 開催日時
                    wkEventTime = Util.GetString(row.EventTime).Substring(0, 5);
                    if (wkEventTime == "00:00")
                    {
                        wkEventDate = Util.GetString(row.EventDate).Substring(0, 10);
                    }
                    else
                    {
                        wkEventDate = Util.GetString(row.EventDate).Substring(0, 10) + " " + wkEventTime;
                    }

                    // 件名
                    wkSubject = Util.GetString(row.Subject);

                    // 中止
                    wkCancel = "";
                    if(Util.GetString(row.CancelFlg) == "1")
                    {
                        wkCancel = "中止";
                    }

                    // ログインユーザーが対象か判定
                    wkTargetFlg = false;
                    wkUserList = Util.GetString(row.Member).Split(',');
                    foreach (string code in wkUserList)
                    {
                        // [メンバー]を検索
                        if (_sqlite.Db_A_Account.MemberCode.Equals(code))
                        {
                            wkTargetFlg = true;
                            break;
                        }
                    }
                    wkUserList = Util.GetString(row.MemberAdd).Split(',');
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
                        if (Util.GetString(row.Answer) == "1")
                        {
                            wkAnswer = "出席";
                        }
                        else if (Util.GetString(row.Answer) == "2")
                        {
                            wkAnswer = "欠席";
                        }
                        else
                        {
                            wkAnswer = "未回答";
                        }
                    }

                    // List DataSet
                    Items.Add(new ClubDirectorRow(wkDataNo, wkEventClass, wkEventDate, wkSubject, wkAnswer, wkCancel));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubDirectorRow(wkDataNo, wkEventClass, wkEventDate, wkSubject, wkAnswer, wkCancel));
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
            if (string.IsNullOrEmpty(item.DataNo))
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
        public ClubDirectorRow(string datano, string eventclass, string eventdate, string subject, string answer, string cancelflg)
        {
            DataNo = datano;
            EventClass = eventclass;
            EventDate = eventdate;
            Subject = subject;
            CancelFlg = cancelflg;
            Answer = answer;
        }
        public string DataNo { get; set; }
        public string EventClass { get; set; }
        public string EventDate { get; set; }
        public string Subject { get; set; }
        public string CancelFlg { get; set; }
        public string Answer { get; set; }
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
            if (!String.IsNullOrEmpty(info.DataNo))
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