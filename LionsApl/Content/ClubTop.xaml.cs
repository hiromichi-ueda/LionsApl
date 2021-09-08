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

    public partial class ClubTop : ContentPage
    {
        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ObservableCollection<string> Items { get; set; }

        public ClubTop()
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
            LoginInfo.Text = _sqlite.Db_A_Account.ClubName + " " + _sqlite.Db_A_Account.MemberFirstName + _sqlite.Db_A_Account.MemberLastName;

            // クラブスローガン設定
            Sel_T_CLUBSLOGAN();


        }

        //-------------------------------------------
        /// <summary>
        /// 年間例会スケジュール一覧画面へ
        /// </summary>
        //-------------------------------------------
        private void Label_ClubSchedule_Taped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClubScheduleList());
        }

        //-------------------------------------------
        /// <summary>
        /// 理事・委員会一覧画面へ
        /// </summary>
        //-------------------------------------------
        private void Label_Director_Taped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClubDirectorList());
        }

        //-------------------------------------------
        /// <summary>
        /// 例会プログラム一覧画面へ
        /// </summary>
        //-------------------------------------------
        private void Label_MeetingProgram_Taped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClubMeetingProgramList());
        }

        //-------------------------------------------
        /// <summary>
        /// 連絡事項（クラブ）一覧画面へ
        /// </summary>
        //-------------------------------------------
        private void Label_Infomation_Taped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClubInfomationList());
        }

        //-------------------------------------------
        /// <summary>
        /// 会員情報一覧画面へ
        /// </summary>
        //-------------------------------------------
        private void Label_Member_Taped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClubMemberList());
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 選択したテーブルの内容を表示する。(T_CLUBSLOGAN)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Sel_T_CLUBSLOGAN()
        {
            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(_sqlite.DbPath))
                {
                    foreach (var row in db.Query<Table.T_CLUBSLOGAN>("Select * From T_CLUBSLOGAN"))
                    {
                        // クラブスローガン設定
                        if (row.ClubSlogan != null)
                        {
                            ClubSlogan.Text = row.ClubSlogan;
                        }
                        // 会長名設定
                        if (row.ExecutiveName != null)
                        {
                            ExecutiveName.Text = "会長 " + row.ExecutiveName;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(クラブスローガン) : &{ex.Message}", "OK");
            }
        }

    }

}