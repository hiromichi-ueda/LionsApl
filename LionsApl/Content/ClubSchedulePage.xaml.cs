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
    public partial class ClubSchedulePage : ContentPage
    {

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        // 
        private int _DataNo;

        public ClubSchedulePage(string title, int dataNo)
        {
            InitializeComponent();

            // 一覧から取得
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
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

        }

        private void GetMeetingScheduleInfo()
        {
            string CancelFlg = string.Empty;
            string CancelStr = string.Empty;
            string Online = string.Empty;
            string OnlineStr = string.Empty;

            Table.TableUtil Util = new Table.TableUtil();

            // 会員情報取得
            try
            {
                foreach (Table.T_MEETINGSCHEDULE row in _sqlite.Get_T_MEETINGSCHEDULE("Select * " +
                                                                    "From T_MEETINGSCHEDULE " +
                                                                    "Where DataNo = '" + _DataNo + "'"))
                {
                    // 例会日
                    MeetingDate.Text = Util.GetString(row.MeetingDate).Substring(0, 10);

                    // 中止
                    if (Util.GetString(row.CancelFlg) == "1")
                    {
                        CancelStr = "中止";
                    }

                    // オンライン
                    if (Util.GetString(row.Online) == "1")
                    {
                        OnlineStr = "　※オンライン例会";
                    }

                    // 時間
                    MeetingTime.Text = Util.GetString(row.MeetingTime) + "～" + OnlineStr;


                    // 例会名
                    MeetingName.Text = Util.GetString(row.MeetingName);

                    // 備考

                    // --------------------------
                    // 例会オプション
                    // --------------------------

                    // Option1

                    // Option2

                    // Option3

                    // Option4

                    // Option5

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_MEETINGSCHEDULE) : &{ex.Message}", "OK");
            }

        }

    }

}
