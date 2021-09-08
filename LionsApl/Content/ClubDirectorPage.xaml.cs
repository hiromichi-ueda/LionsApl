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
        private int _DataNo;

        public ClubDirectorPage(int dataNo)
        {
            InitializeComponent();

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

        // 理事・委員会情報を取得する。
        private void GetDirectorInfo()
        {
            Table.TableUtil Util = new Table.TableUtil();

            // 会員情報取得
            try
            {
                foreach (Table.T_DIRECTOR row in _sqlite.Get_T_DIRECTOR("Select * " +
                                                                        "From T_DIRECTOR " +
                                                                        "Where DataNo = '" + _DataNo + "'"))
                {
                    // 出欠タイトル
                    if (row.EventClass == "1")
                    {
                        DirectorTitle.Text = "理事会出欠の確認";
                    }
                    else
                    {
                        DirectorTitle.Text = "委員会出欠の確認";
                    }

                    // 開催日
                    EventDate.Text = row.EventDate.Substring(0, 10) + " " + row.EventTime;

                    // 区分
                    if (row.Season == "1")
                    {
                        Season.Text = "今期";
                    }
                    else
                    {
                        Season.Text = "次期";
                    }

                    // 開催場所
                    EventPlace.Text = row.EventPlace;

                    // 議題・内容
                    Agenda.Text = row.Agenda;

                    // 回答期限
                    AnswerDate.Text = row.AnswerDate.Substring(0, 10);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(T_DIRECTOR) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出席ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Attendance_Button_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Alert", "出席", "OK");
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 欠席ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Absence_Button_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Alert", "欠席", "OK");
        }


    }
}