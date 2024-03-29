﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// クラブTOP画面クラス
    /// </summary>
    ///////////////////////////////////////////////////////////////////////////////////////////
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubTop : ContentPage
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// プロパティ

        private SQLiteManager _sqlite;                      // SQLiteマネージャークラス

        public ObservableCollection<string> Items { get; set; }

        // 表示用文字列
        string ST_NOCLUBSLOGAN = "クラブスローガン情報はありません。";


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// メソッド

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public ClubTop()
        {
            InitializeComponent();

            // font-size
            this.ClubSlogan.FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
            this.ExecutiveName.FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label));
            this.ExecutiveName.FontSize = Device.GetNamedSize(NamedSize.Caption, typeof(Label));

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // A_SETTINGデータ取得
            _sqlite.GetSetting();

            // タイトル設定
            Title = _sqlite.Db_A_Setting.CabinetName;

            // A_ACCOUNTデータ取得
            _sqlite.GetAccount();

            // ログイン情報設定
            LoginInfo.Text = _sqlite.LoginInfo;

            // クラブスローガン設定
            Sel_T_CLUBSLOGAN();

            // 未読情報設定
            SetBadgeInfo();


        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // 未読情報設定
            SetBadgeInfo();

        }

        //-------------------------------------------
        /// <summary>
        /// 年間例会スケジュール一覧画面へ
        /// </summary>
        //-------------------------------------------
        private async void Label_ClubSchedule_Taped(object sender, EventArgs e)
        {
            // 処理中ダイアログ表示
            await((App)Application.Current).DispLoadingDialog();

            await Navigation.PushAsync(new ClubScheduleList());
        }

        //-------------------------------------------
        /// <summary>
        /// 理事・委員会一覧画面へ
        /// </summary>
        //-------------------------------------------
        private async void Label_Director_Taped(object sender, EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            // DB情報取得処理
            try
            {
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_BADGEUPD());

                // タブページのバッジ更新
                ((MainTabPage)((App)Application.Current).TabPage).SetBadgeInfo();

                // 理事・委員会一覧画面表示
                await Navigation.PushAsync(new ClubDirectorList());

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"理事・委員会一覧表示エラー : {ex.Message}", "OK");
            }
        }

        //-------------------------------------------
        /// <summary>
        /// 例会プログラム一覧画面へ
        /// </summary>
        //-------------------------------------------
        private async void Label_MeetingProgram_Taped(object sender, EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            await Navigation.PushAsync(new ClubMeetingProgramList());
        }

        //-------------------------------------------
        /// <summary>
        /// 連絡事項（クラブ）一覧画面へ
        /// </summary>
        //-------------------------------------------
        private async void Label_Infomation_Taped(object sender, EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            // DB情報取得処理
            try
            {
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_BADGEUPD());

                // タブページのバッジ更新
                ((MainTabPage)((App)Application.Current).TabPage).SetBadgeInfo();

                // 理事・委員会一覧画面表示
                await Navigation.PushAsync(new ClubInfomationList());

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", $"連絡事項（クラブ）一覧表示エラー : {ex.Message}", "OK");
            }
        }

        //-------------------------------------------
        /// <summary>
        /// 会員情報一覧画面へ
        /// </summary>
        //-------------------------------------------
        private async void Label_Member_Taped(object sender, EventArgs e)
        {
            // 処理中ダイアログ表示
            await((App)Application.Current).DispLoadingDialog();

            await Navigation.PushAsync(new ClubMemberList());
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
                using (var db = new SQLite.SQLiteConnection(_sqlite.dbFile))
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
                    if (ClubSlogan.Text == null)
                    {
                        ClubSlogan.Text = ST_NOCLUBSLOGAN;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(クラブスローガン) : &{ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ログイン者の未読情報(理事・委員会)をSQLiteファイルから取得して画面に設定する。
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void SetBadgeInfo()
        {

            // データ取得
            try
            {
                // 理事・委員会
                LabelDirectorBadge.Text = string.Empty;
                foreach (Table.T_BADGE row in _sqlite.Get_T_BADGE("SELECT * " +
                                                                  "FROM T_BADGE " +
                                                                  "WHERE DataClass = '2' " +
                                                                  "AND MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' "))
                {
                    // 未読情報設定
                    LabelDirectorBadge.Text = LADef.ST_TOP_BADGE;
                }

                // 連絡事項
                LabelInfomationBadge.Text = string.Empty;
                foreach (Table.T_BADGE row in _sqlite.Get_T_BADGE("SELECT * " +
                                                                  "FROM T_BADGE " +
                                                                  "WHERE DataClass = '3' " +
                                                                  "AND MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "' "))
                {
                    // 未読情報設定
                    LabelInfomationBadge.Text = LADef.ST_TOP_BADGE;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"SQLite検索エラー(理事・委員会_未読情報) : {ex.Message}", "OK");
            }
        }

    }

}