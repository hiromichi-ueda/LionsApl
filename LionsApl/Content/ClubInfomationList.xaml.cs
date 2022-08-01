using System;
using System.Collections.Generic;
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
    /// クラブ：連絡事項一覧クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubInfomationList : ContentPage
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
        private ObservableCollection<ClubInfomationRow> Items;


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubInfomationList()
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

            // 連絡事項データ取得
            GetClubInfomation();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 連絡事項情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetClubInfomation()
        {
            int intDataNo = 0;
            string strWorkTypeCode = string.Empty;
            string strWorkClubCode = string.Empty;
            string strWorkDate = string.Empty;
            string strWorkSubject = string.Empty;
            string WorkFlg = string.Empty;
            bool bolAddListFlg = false;
            string strWorkBadge = string.Empty;
            Items = new ObservableCollection<ClubInfomationRow>();

            try
            {
                // 会員マスタよりログインユーザーの会員情報を取得
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER(
                                                                 "SELECT * " +
                                                                 "FROM M_MEMBER " +
                                                                 "WHERE MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' "))
                {
                    // 会員種別を保持
                    strWorkTypeCode = _utl.GetString(row.TypeCode);
                }

                // 連絡事項(クラブ)のデータを全件取得
                foreach (Table.T_INFOMATION_CLUB row in _sqlite.Get_T_INFOMATION_CLUB(
                                                                 "SELECT * " +
                                                                 "FROM T_INFOMATION_CLUB " +
                                                                 "ORDER BY AddDate DESC"))
                {
                    //AddListFlg = false;
                    //WorkFlg = _utl.GetString(row.InfoFlg);

                    //// データ№
                    //wkDataNo = row.DataNo;

                    //// 未読(連絡事項)のデータをデータNoを条件に取得
                    //BadgeFlg = false;
                    //foreach (Table.T_BADGE bRow in _sqlite.Get_T_BADGE(
                    //                                                 "SELECT * " +
                    //                                                 "FROM T_BADGE " +
                    //                                                 "WHERE DataClass = '3' " +
                    //                                                 "AND DataNo = " + wkDataNo))
                    //{
                    //    // 未読のデータが取得できた場合はフラグを立てる
                    //    BadgeFlg = true;
                    //    break;
                    //}

                    //// 全会員の場合
                    //if (WorkFlg == _utl.INFOFLG_ALL)
                    //{
                    //    WorkCodeList = _utl.GetString(row.TypeCode).Split(',');
                    //    foreach (string code in WorkCodeList)
                    //    {
                    //        // 会員種別を条件にログインユーザーが対象か判定
                    //        if (WorkTypeCode.Equals(code))
                    //        {
                    //            AddListFlg = true;
                    //            break;
                    //        }
                    //    }
                    //}

                    //// 個別設定の場合
                    //else if (WorkFlg == _utl.INFOFLG_PRIV)
                    //{
                    //    WorkCodeList = _utl.GetString(row.InfoUser).Split(',');
                    //    foreach (string code in WorkCodeList)
                    //    {
                    //        // 連絡者(会員番号)を条件にログインユーザーが対象か判定
                    //        if (_sqlite.Db_A_Account.MemberCode.Equals(code))
                    //        {
                    //            AddListFlg = true;
                    //            break;
                    //        }
                    //    }
                    //}

                    //// ログインユーザーが対象の連絡事項を設定
                    //if (AddListFlg)
                    //{
                    //    if (BadgeFlg)
                    //    {
                    //        WorkBadge = LADef.ST_BADGE;
                    //    }
                    //    else
                    //    {
                    //        WorkBadge = string.Empty;
                    //    }

                    //    WorkClubCode = _utl.GetString(row.ClubCode);
                    //    WorkDate = _utl.GetString(row.AddDate).Substring(0, 10);
                    //    WorkSubject = _utl.GetString(row.Subject);
                    //    Items.Add(new ClubInfomationRow(wkDataNo, WorkClubCode, WorkDate, WorkSubject, WorkBadge));

                    //}
                    GetClubInfomationListData(row,
                                              ref intDataNo,
                                              ref strWorkClubCode,
                                              ref strWorkDate,
                                              ref strWorkSubject,
                                              ref strWorkBadge,
                                              ref strWorkTypeCode,
                                              ref bolAddListFlg);

                    if (bolAddListFlg)
                    {
                        Items.Add(new ClubInfomationRow(intDataNo, strWorkClubCode, strWorkDate, strWorkSubject, strWorkBadge));
                    }
                }

                // ログインユーザーが対象の連絡事項が1件もない場合
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    //Items.Add(new ClubInfomationRow(wkDataNo, WorkClubCode, WorkDate, WorkSubject));
                    Items.Add(new ClubInfomationRow(0, strWorkClubCode, strWorkDate, strWorkSubject, strWorkBadge));
                }
                ClubInfomationListView.ItemsSource = Items;
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_INFOMATION_CLUB) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// イベント情報をSQLiteファイルから取得して画面に設定する。(更新用)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void UpdBadgeData()
        {
            int intDataNo = 0;
            string strWorkTypeCode = string.Empty;
            string strWorkClubCode = string.Empty;
            string strWorkDate = string.Empty;
            string strWorkSubject = string.Empty;
            string WorkFlg = string.Empty;
            bool bolAddListFlg = false;
            string strWorkBadge = string.Empty;
            int idx = 0;

            try
            {
                // 会員マスタよりログインユーザーの会員情報を取得
                foreach (Table.M_MEMBER row in _sqlite.Get_M_MEMBER(
                                                                 "SELECT * " +
                                                                 "FROM M_MEMBER " +
                                                                 "WHERE MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' "))
                {
                    // 会員種別を保持
                    strWorkTypeCode = _utl.GetString(row.TypeCode);
                }

                // 連絡事項(クラブ)のデータを全件取得
                foreach (Table.T_INFOMATION_CLUB row in _sqlite.Get_T_INFOMATION_CLUB(
                                                                 "SELECT * " +
                                                                 "FROM T_INFOMATION_CLUB " +
                                                                 "ORDER BY AddDate DESC"))
                {
                    GetClubInfomationListData(row,
                                              ref intDataNo,
                                              ref strWorkClubCode,
                                              ref strWorkDate,
                                              ref strWorkSubject,
                                              ref strWorkBadge,
                                              ref strWorkTypeCode,
                                              ref bolAddListFlg);

                    if (bolAddListFlg)
                    {
                        // 未読を設定
                        Items[idx].Badge = strWorkBadge;
                    }

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
        private void GetClubInfomationListData(Table.T_INFOMATION_CLUB row,
                                         ref int intDataNo,
                                         ref string strWorkClubCode,
                                         ref string strWorkDate,
                                         ref string strWorkSubject,
                                         ref string strWorkBadge,
                                         ref string strWorkTypeCode,
                                         ref bool bolAddListFlg)
        {
            string WorkFlg = string.Empty;
            string[] WorkCodeList = null;
            bool BadgeFlg = false;

            bolAddListFlg = false;
            WorkFlg = _utl.GetString(row.InfoFlg);

            // データ№
            intDataNo = row.DataNo;

            // 未読(連絡事項)のデータをデータNoを条件に取得
            BadgeFlg = false;
            foreach (Table.T_BADGE bRow in _sqlite.Get_T_BADGE(
                                                             "SELECT * " +
                                                             "FROM T_BADGE " +
                                                             "WHERE DataClass = '3' " +
                                                             "AND DataNo = " + intDataNo))
            {
                // 未読のデータが取得できた場合はフラグを立てる
                BadgeFlg = true;
                break;
            }

            // 全会員の場合
            if (WorkFlg == _utl.INFOFLG_ALL)
            {
                WorkCodeList = _utl.GetString(row.TypeCode).Split(',');
                foreach (string code in WorkCodeList)
                {
                    // 会員種別を条件にログインユーザーが対象か判定
                    if (strWorkTypeCode.Equals(code))
                    {
                        bolAddListFlg = true;
                        break;
                    }
                }
            }

            // 個別設定の場合
            else if (WorkFlg == _utl.INFOFLG_PRIV)
            {
                WorkCodeList = _utl.GetString(row.InfoUser).Split(',');
                foreach (string code in WorkCodeList)
                {
                    // 連絡者(会員番号)を条件にログインユーザーが対象か判定
                    if (_sqlite.Db_A_Account.MemberCode.Equals(code))
                    {
                        bolAddListFlg = true;
                        break;
                    }
                }
            }

            // ログインユーザーが対象の連絡事項を設定
            if (bolAddListFlg)
            {
                if (BadgeFlg)
                {
                    strWorkBadge = LADef.ST_BADGE;
                }
                else
                {
                    strWorkBadge = string.Empty;
                }

                strWorkClubCode = _utl.GetString(row.ClubCode);
                strWorkDate = _utl.GetString(row.AddDate).Substring(0, 10);
                strWorkSubject = _utl.GetString(row.Subject);

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

            ClubInfomationRow item = e.Item as ClubInfomationRow;

            // 連絡事項が1件もない(メッセージ行のみ表示している)場合は処理しない
            if (item.DataNo == 0)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // 未読の場合は既読にする
            if (item.Badge.Equals(LADef.ST_BADGE))
            {
                // 未読情報のキーを設定
                _cbadge.DataClass = _utl.DATACLASS_IN;
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

            // 連絡事項(クラブ)画面へ
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Navigation.PushAsync(new ClubInfomationPage(item.DataNo));
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Navigation.PushAsync(new ClubInfomationPageAndroid(item.DataNo));
            }

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

    public sealed class ClubInfomationRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _dataNo = 0;
        private string _clubCode = string.Empty;
        private string _addDate = string.Empty;
        private string _subject = string.Empty;
        private string _badge = string.Empty;

        public ClubInfomationRow(int dataNo, string clubCode, string addDate, string subject, string badge)
        {
            DataNo = dataNo;
            ClubCode = clubCode;
            AddDate = addDate;
            Subject = subject;
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
        public string ClubCode
        {
            get { return _clubCode; }
            set
            {
                if (_clubCode != value)
                {
                    _clubCode = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(ClubCode)));
                    }
                }
            }
        }
        public string AddDate
        {
            get { return _addDate; }
            set
            {
                if (_addDate != value)
                {
                    _addDate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(AddDate)));
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

    public class MyClubInfomationSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (ClubInfomationRow)item;
            if (info.DataNo > 0){
                return ExistDataTemplate;
            }
            else
            {
                return NoDataTemplate;
            }
        }
    }
}