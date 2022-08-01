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
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブ：例会プログラム一覧クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMeetingProgramList : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // Utilityクラス
        private LAUtility _utl;

        // リストビュー設定内容
        public List<MeetingProgramRow> Items { get; set; }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubMeetingProgramList()
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

            // 例会プログラムデータ取得
            GetMeetingProgram();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 例会プログラム情報をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void GetMeetingProgram()
        {
            int wkDataNo = 0;
            string wkMeeting = string.Empty;
            string wkMeetingDate = string.Empty;
            string wkMeetingName = string.Empty;
            string wkCancel = string.Empty;
            Items = new List<MeetingProgramRow>();

            //Table.TableUtil Util = new Table.TableUtil();

            try
            {
                foreach (Table.CLUB_MPROG row in _sqlite.Get_CLUB_MPROG(
                                                                "SELECT " +
                                                                    "t1.DataNo, " +
                                                                    "t1.ScheduleDataNo, " +
                                                                    "t1.ClubCode, " +
                                                                    "t1.ClubNameShort, " +
                                                                    "t1.Meeting, " +
                                                                    "t2.MeetingDate, " +
                                                                    "t2.MeetingTime, " +
                                                                    "t2.MeetingPlace, " +
                                                                    "t2.MeetingName, " +
                                                                    "t2.CancelFlg " +
                                                                "FROM " +
                                                                    "T_MEETINGPROGRAM t1 " +
                                                                "LEFT OUTER JOIN " +
                                                                    "T_MEETINGSCHEDULE t2 " +
                                                                "ON " +
                                                                    "t1.ScheduleDataNo = t2.DataNo " +
                                                                "ORDER BY t2.MeetingDate DESC"))
                {
                    wkDataNo = row.DataNo;
                    wkMeeting = "";
                    if (_utl.GetString(row.Meeting) == "2")
                    {
                        wkMeeting = "[オンライン]";
                    }
                    wkMeetingDate = _utl.GetDateString(row.MeetingDate) + "  " + wkMeeting;
                    wkMeetingName = _utl.GetString(row.MeetingName);
                    wkCancel = _utl.StrCancel(row.CancelFlg);
                    Items.Add(new MeetingProgramRow(wkDataNo, wkMeetingDate, wkMeetingName, wkCancel));
                }
                if (Items.Count == 0)
                {
                    // メッセージ表示のため空行を追加
                    Items.Add(new MeetingProgramRow(0, wkMeetingDate, wkMeetingName, wkCancel));
                }
                this.BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(CLUB_MPROG) : {ex.Message}", "OK");
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

            MeetingProgramRow item = e.Item as MeetingProgramRow;

            // 1件もない(メッセージ行のみ表示している)場合は処理しない
            if (item.DataNo == 0)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            // iOSの場合
            if (Device.RuntimePlatform == Device.iOS)
            {
                // 例会プログラム画面へ
                Navigation.PushAsync(new ClubMeetingProgramPage(item.DataNo));
            }
            // Androidの場合
            else if (Device.RuntimePlatform == Device.Android)
            {
                // 例会プログラム画面(Android用)へ
                Navigation.PushAsync(new ClubMeetingProgramPageAndroid(item.DataNo));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

    }

    public sealed class MeetingProgramRow
    {
        public MeetingProgramRow(int dataNo, string meetingDate, string meetingName, string cancel)
        {
            DataNo = dataNo;
            MeetingDate = meetingDate;
            MeetingName = meetingName;
            Cancel = cancel;
        }
        public int DataNo { get; set; }
        public string MeetingDate { get; set; }
        public string MeetingName { get; set; }
        public string Cancel { get; set; }
    }

    public class MyClubMeetingProgramSelector : DataTemplateSelector
    {
        //切り替えるテンプレートを保持するプロパティを用意する
        public DataTemplate ExistDataTemplate { get; set; }
        public DataTemplate NoDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // 条件より該当するテンプレートを返す
            var info = (MeetingProgramRow)item;
            if (info.DataNo > 0)
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