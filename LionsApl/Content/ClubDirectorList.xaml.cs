using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：理事・委員会一覧クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubDirectorList : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // 情報通信マネージャクラス
        private IComManager _icom;

        // T_BADGE登録クラス
        private CBADGE _cbadge;

        // リストビュー設定内容
        private ObservableCollection<ClubDirectorRow> Items;


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubDirectorList()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // Content Utilクラス生成
            _utl = new LAUtility();

            // 情報通信マネージャー生成
            _icom = IComManager.GetInstance(_sqlite.dbFile);

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // 未読情報クラス生成
            _cbadge = new CBADGE("", 0, "", "");

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
            int intDataNo = 0;                              // データNo.設定用
            string strEventClass = "";                      // 区分設定用文字列
            string strEventDate = "";                       // 開催日設定用文字列
            string strEventTime = "";                       // 開催時刻設定用文字列
            string strSubject = "";                         // 件名設定用文字列
            string strAnswer = "";                          // 回答設定用文字列
            string strCancel = "";                          // 中止表示用文字列
            Color colAnswer = Color.Default;                // 回答設定文字色用値
            string strBadge = "";                           // 未読設定用文字列
            Items = new ObservableCollection<ClubDirectorRow>();

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
                                                                "TE.Answer, " +
                                                                "TB.DataNo AS Badge " +
                                                                "FROM T_DIRECTOR TD " +
                                                                "LEFT OUTER JOIN T_EVENTRET TE " +
                                                                "ON TD.DataNo = TE.EventDataNo AND TE.EventClass = '3' " +
                                                                "LEFT OUTER JOIN T_BADGE TB " +
                                                                "ON TD.DataNo = TB.DataNo " +
                                                                "AND TB.DataClass = '2' " +
                                                                "ORDER BY TD.EventDate DESC "))
                {
                    intDataNo = 0;                              // データNo.設定用
                    strEventClass = "";                         // 区分設定用文字列
                    strEventDate = "";                          // 開催日設定用文字列
                    strEventTime = "";                          // 開催時刻設定用文字列
                    strSubject = "";                            // 件名設定用文字列
                    strAnswer = "";                             // 回答設定用文字列
                    strCancel = "";                             // 中止表示用文字列
                    colAnswer = Color.Default;                  // 回答設定文字色用値
                    strBadge = "";                              // 未読設定用文字列

                    // イベントリストの各項目値を取得する
                    GetDirectorListData(row,
                                        ref intDataNo,
                                        ref strEventClass,
                                        ref strEventDate,
                                        ref strEventTime,
                                        ref strSubject,
                                        ref strAnswer,
                                        ref strCancel,
                                        ref colAnswer,
                                        ref strBadge);

                    // List DataSet
                    Items.Add(new ClubDirectorRow(intDataNo, strEventClass, strEventDate, strSubject, strAnswer, strCancel, colAnswer, strBadge));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new ClubDirectorRow(0, strEventClass, strEventDate, strSubject, strAnswer, strCancel, colAnswer, strBadge));
                }
                ClubDirectorListView.ItemsSource = Items;
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : {ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント情報をSQLiteファイルから取得して画面に設定する。(更新用)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void UpdBadgeData()
        {
            int intDataNo = 0;                              // データNo.設定用
            string strEventClass = "";                      // 区分設定用文字列
            string strEventDate = "";                       // 開催日設定用文字列
            string strEventTime = "";                       // 開催時刻設定用文字列
            string strSubject = "";                         // 件名設定用文字列
            string strAnswer = "";                          // 回答設定用文字列
            string strCancel = "";                          // 中止表示用文字列
            Color colAnswer = Color.Default;                // 回答設定文字色用値
            string strBadge = "";                           // 未読設定用文字列
            int idx = 0;

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
                                                                "TE.Answer, " +
                                                                "TB.DataNo AS Badge " +
                                                                "FROM T_DIRECTOR TD " +
                                                                "LEFT OUTER JOIN T_EVENTRET TE " +
                                                                "ON TD.DataNo = TE.EventDataNo AND TE.EventClass = '3' " +
                                                                "LEFT OUTER JOIN T_BADGE TB " +
                                                                "ON TD.DataNo = TB.DataNo " +
                                                                "AND TB.DataClass = '2' " +
                                                                "ORDER BY TD.EventDate DESC "))

                {
                    intDataNo = 0;                              // データNo.設定用
                    strEventClass = "";                         // 区分設定用文字列
                    strEventDate = "";                          // 開催日設定用文字列
                    strEventTime = "";                          // 開催時刻設定用文字列
                    strSubject = "";                            // 件名設定用文字列
                    strAnswer = "";                             // 回答設定用文字列
                    strCancel = "";                             // 中止表示用文字列
                    colAnswer = Color.Default;                  // 回答設定文字色用値
                    strBadge = "";                              // 未読設定用文字列

                    // イベントリストの各項目値を取得する
                    GetDirectorListData(row,
                                        ref intDataNo,
                                        ref strEventClass,
                                        ref strEventDate,
                                        ref strEventTime,
                                        ref strSubject,
                                        ref strAnswer,
                                        ref strCancel,
                                        ref colAnswer,
                                        ref strBadge);

                    // 未読を設定
                    Items[idx].Badge = strBadge;

                    idx++;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"UpdateSQLite検索エラー(T_EVENTRET/T_EVENT/T_MEETINGSCHEDULE/T_DIRECTOR) : {ex.Message}", "OK");
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベントリストの各項目値を取得するして表示用変数に設定する。
        /// </summary>
        /// <param name="row">SQLiteから取得したイベントデータ</param>
        /// <param name="intDataNo">表示対象データのDataNo</param>
        /// <param name="intEventDataNo">表示対象データのEventDataNo</param>
        /// <param name="strDate">日付（表示用変数）</param>
        /// <param name="strCancel">キャンセル（表示用変数）</param>
        /// <param name="strTitle">タイトル（表示用変数）</param>
        /// <param name="strAnswer">回答（表示用変数）</param>
        /// <param name="colAnswer">回答文字色（表示用変数）</param>
        /// <param name="strBadge">未読（表示用変数）</param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetDirectorListData(Table.DIRECTOR_LIST row,
                                        ref int intDataNo,
                                        ref string strEventClass,
                                        ref string strEventDate,
                                        ref string strEventTime,
                                        ref string strSubject,
                                        ref string strAnswer,
                                        ref string strCancel,
                                        ref Color colAnswer,
                                        ref string strBadge)
        {
            string flgEventClass = string.Empty;
            string wkAnsFlg = string.Empty;
            bool wkTargetFlg = false;
            string wkBdgFlg = string.Empty;

            // DataNo
            intDataNo = row.DataNo;

            // 区分
            flgEventClass = _utl.GetString(row.EventClass);
            if (flgEventClass == LADef.CLUBEVENTCLASS_RI)
            {
                strEventClass = LADef.ST_BOARD;
            }
            else if (flgEventClass == LADef.CLUBEVENTCLASS_IN)
            {
                strEventClass = LADef.ST_COMM;
            }
            else
            {
                strEventClass = LADef.ST_ETC;
            }

            // 開催日時
            strEventTime = _utl.GetTimeString(row.EventTime);
            if (strEventTime == "00:00")
            {
                strEventDate = _utl.GetString(row.EventDate).Substring(0, 10);
            }
            else
            {
                strEventDate = _utl.GetString(row.EventDate).Substring(0, 10) + " " + strEventTime;
            }

            // 件名
            strSubject = _utl.GetString(row.Subject);

            // 中止
            strCancel = "";
            if (_utl.GetString(row.CancelFlg) == "1")
            {
                strCancel = LADef.ST_CANCEL;
            }

            // ログインユーザーが対象か判定
            wkTargetFlg = false;
            ChkMember(row.Member, _sqlite.Db_A_Account.MemberCode, ref wkTargetFlg);
            ChkMember(row.MemberAdd, _sqlite.Db_A_Account.MemberCode, ref wkTargetFlg);

            // 回答
            wkAnsFlg = _utl.GetString(row.Answer);
            strAnswer = "";
            if (wkTargetFlg)
            {
                // 対象者の場合は回答をセット
                if (wkAnsFlg.Equals(LADef.ANSWER_PRE))
                {
                    // 出席
                    strAnswer = LADef.ST_ANSWER_PRE;
                    colAnswer = Color.FromHex(LADef.STRCOL_STRDEF);
                }
                else if (wkAnsFlg.Equals(LADef.ANSWER_ABS))
                {
                    // 欠席
                    strAnswer = LADef.ST_ANSWER_ABS;
                    colAnswer = Color.FromHex(LADef.STRCOL_STRDEF);
                }
                else
                {
                    // 未回答
                    strAnswer = LADef.ST_ANSWER_NO;
                    colAnswer = Color.FromHex(LADef.STRCOL_RED);
                }
            }

            // 未読
            wkBdgFlg = _utl.GetString(row.Badge);
            strBadge = "";
            if (!wkBdgFlg.Equals(string.Empty))
            {
                strBadge = LADef.ST_BADGE;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Memberに自分が含まれるかチェックする。
        /// </summary>
        /// <param name="members"></param>
        /// <param name="memberCode"></param>
        /// <param name="targetFlg"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void ChkMember(string members, string memberCode, ref bool targetFlg)
        {
            string[] wkUserList = null;

            // 値がない場合は終了
            if (members == null)
            {
                return;
            }
            if (members.Equals(string.Empty))
            {
                return;
            }

            // ','で文字列を分割する
            wkUserList = _utl.GetString(members).Split(',');
            foreach (string code in wkUserList)
            {
                string chkCode = string.Empty;
                // コードに"_XX"が含まれるか確認する
                if (code.IndexOf('_', 0) > 0)
                {
                    // コードに"_XX"が含まれる場合

                    // コードから"_XX"を除く
                    string[] codes = code.Split('_');
                    string code_x = codes[0];
                    chkCode = code_x;
                }
                else
                {
                    // コードに"_XX"が含まれない場合
                    chkCode = code;
                }
                // [メンバー]を検索
                if (memberCode.Equals(chkCode))
                {
                    targetFlg = true;
                    break;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // 未読情報データ更新
            UpdBadgeData();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// タップ処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
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

            // 未読の場合は既読にする
            if (item.Badge.Equals(LADef.ST_BADGE))
            {
                // 未読情報のキーを設定
                _cbadge.DataClass = _utl.DATACLASS_DI;
                _cbadge.DataNo = item.DataNo;
                _cbadge.ClubCode = _sqlite.Db_A_Account.ClubCode;
                _cbadge.MemberCode = _sqlite.Db_A_Account.MemberCode;

                // 未読情報をコンテンツに設定
                _icom.SetContentToBADGE(_cbadge);
                try
                {
                    // SQLServerへ削除
                    Task<HttpResponseMessage> response = _icom.AsyncPostTextForWebAPI();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"SQLServer 未読情報削除エラー : {ex.Message}", "OK");
                }

                try
                {
                    // SQLiteへ削除
                    SetBadgeSQlite();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", $"SQLite 未読情報削除エラー : {ex.Message}", "OK");
                }

                // タブページのバッジ更新
                ((MainTabPage)((App)Application.Current).TabPage).SetBadgeInfo();
            }

            // 理事・委員会画面へ
            await Navigation.PushAsync(new ClubDirectorPage(item.DataNo, item.Answer));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 未読情報削除（SQLite）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetBadgeSQlite()
        {
            string _dataClass = _utl.GetSQLString(_cbadge.DataClass);
            string _dataNo = _cbadge.DataNo.ToString();
            string _clubCode = _utl.GetSQLString(_cbadge.ClubCode);
            string _memberCode = _utl.GetSQLString(_cbadge.MemberCode);

            foreach (Table.T_BADGE row in _sqlite.Del_T_BADGE("DELETE FROM T_BADGE " +
                                                              "WHERE DataClass = " + _dataClass + " " +
                                                              "AND DataNo = " + _dataNo + " " +
                                                              "AND ClubCode = " + _clubCode + " " +
                                                              "AND MemberCode = " + _memberCode + " "))
            { }
        }

    }

    public sealed class ClubDirectorRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _dataNo = 0;
        private string _eventclass = string.Empty;
        private string _eventdate = string.Empty;
        private string _subject = string.Empty;
        private string _answer = string.Empty;
        private string _cancelflg = string.Empty;
        private Color _answercolor = Color.Default;
        private string _badge = string.Empty;

        public ClubDirectorRow(int datano, 
                               string eventclass, 
                               string eventdate, 
                               string subject, 
                               string answer, 
                               string cancelflg,
                               Color answerColor,
                               string badge)
        {
            DataNo = datano;
            EventClass = eventclass;
            EventDate = eventdate;
            Subject = subject;
            Answer = answer;
            CancelFlg = cancelflg;
            AnswerColor = answerColor;
            Badge = badge;
        }
        public int DataNo
        {
            get { return _dataNo; }
            set
            {
                if (_dataNo != value)
                {
                    _dataNo = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(DataNo)));
                    }
                }
            }
        }
        public string EventClass
        {
            get { return _eventclass; }
            set
            {
                if (_eventclass != value)
                {
                    _eventclass = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(EventClass)));
                    }
                }
            }
        }
        public string EventDate
        {
            get { return _eventdate; }
            set
            {
                if (_eventdate != value)
                {
                    _eventdate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(EventDate)));
                    }
                }
            }
        }
        public string Subject
        {
            get { return _subject; }
            set
            {
                if (_subject != value)
                {
                    _subject = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Subject)));
                    }
                }
            }
        }
        public string CancelFlg
        {
            get { return _cancelflg; }
            set
            {
                if (_cancelflg != value)
                {
                    _cancelflg = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(CancelFlg)));
                    }
                }
            }
        }
        public string Answer
        {
            get { return _answer; }
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Answer)));
                    }
                }
            }
        }
        public Color AnswerColor
        {
            get { return _answercolor; }
            set
            {
                if (_answercolor != value)
                {
                    _answercolor = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(AnswerColor)));
                    }
                }
            }
        }
        public string Badge
        {
            get { return _badge; }
            set
            {
                if (_badge != value)
                {
                    _badge = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Badge)));
                    }
                }
            }
        }
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