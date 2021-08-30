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


    }

}