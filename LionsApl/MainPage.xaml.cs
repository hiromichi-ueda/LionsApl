using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.IO;

namespace LionsApl
{
    public partial class MainPage : ContentPage
    {

        // SQLiteマネージャークラス
        private SQLiteManager _sqlite;

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        public MainPage()
        {
            InitializeComponent();

            // SQLite マネージャークラス生成
            _sqlite = SQLiteManager.GetInstance();

            // アプリケーションバージョン設定
            AppVersion.Text = "Version " + ((App)Application.Current).AppVersion;

            // 開発用ボタンを非表示にする
            //Develop.IsVisible = false;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnAppearing()
        {
            base.OnAppearing();

            double dbVer = 0.0;
            double appVer = 0.0;

            // ボタン制御
            account.IsEnabled = true;
            home.IsEnabled = false;
            update.IsEnabled = false;
            try
            {
                // SQLiteファイル存在チェック
                if (_sqlite.CheckFileDB3())
                {
                    // ファイルがある場合
                    // 設定ファイル情報（A_SETTING）取得
                    _sqlite.SetSetting();
                    // 設定ファイル情報存在チェック
                    if (_sqlite.Db_A_Setting != null)
                    {
                        // 設定ファイル情報がある場合
                        // アカウント情報（A_ACCOUNT）取得
                        _sqlite.SetAccount();
                        // アカウント情報存在チェック
                        if (_sqlite.Db_A_Account != null)
                        {
                            // アカウント情報がある場合
                            // ホームボタン有効
                            home.IsEnabled = true;
                        }

                        // DB設定アプリバージョン取得
                        dbVer = double.Parse(_sqlite.Db_A_Setting.VersionNo);
                        // ローカルアプリバージョン取得
                        appVer = double.Parse(((App)Application.Current).AppVersion);
                        // バージョンチェック
                        if (dbVer > appVer)
                        {
                            // DB設定アプリバージョンの方が新しい場合
                            // ダウンロードボタン有効
                            update.IsEnabled = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"画面ロードエラー(OnAppearing) : {ex.Message}", "OK");
            }
        }

        // ===============================
        // ボタン押下処理
        // ===============================
        #region 各ボタン処理

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ホームボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        async void Button_Home_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();
            
            // DB情報取得処理
            try
            {
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(MessageText, _sqlite.GetSendFileContent_HOME());

                // HOME画面表示
                Application.Current.MainPage = new Content.MainTabPage();

            }
            catch (Exception ex)
            {
                ResultText.Text += "HOME処理（エラー） : " + ex.Message;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        async void Button_Account_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            try
            {
                // SQLiteファイル削除
                _sqlite.DelFileDB3();

                // 空DB作成処理
                _sqlite.CreateTable_ALL();

                // アカウント情報取得
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(MessageText, _sqlite.GetSendFileContent());

                // アカウント設定画面表示
                Application.Current.MainPage = new Content.AccountSetting();

            }
            catch (Exception ex)
            {
                ResultText.Text = "アカウント設定処理（エラー） : " + ex.Message;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アップデートボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        async void Button_Update_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

        }

        #endregion



        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 各画面遷移関連


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Account Setting)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_AccountSetting_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushModalAsync(new AccountSetting());
            // Navigation.PushAsync(new Content.AccountSetting());
            Application.Current.MainPage = new Content.AccountSetting();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Main Tab Page)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_MainTabPage_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushModalAsync(new AccountSetting());
            // Navigation.PushAsync(new Content.AccountSetting());
            Application.Current.MainPage = new Content.MainTabPage();
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Home Top)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_HomeTop_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushAsync(new Content.HomeTop());
            Application.Current.MainPage = new Content.HomeTop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(Club Top)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_ClubTop_Clicked(object sender, System.EventArgs e)
        {
            // Navigation.PushModalAsync(new AccountSetting());
            // Navigation.PushAsync(new Content.ClubTop());
            Application.Current.MainPage = new Content.ClubTop();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面遷移(SQLite Check)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void Button_SQLiteCheck_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new Content.SQLiteChk();
        }
        


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// SQLite関連（ファイル操作）


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create Sqliteファイル（DB3）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_CreateTable_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            ResultText.Text = "";

            // データベース初期設定
            try
            {
                _sqlite.CreateTable_ALL();
                ResultText.Text = "Create Table Finish : \r\n" + _sqlite.dbFile;
            }
            catch (Exception ex)
            {
                ResultText.Text = "Create Table Error : \r\n" + ex.Message;

            }
            ResultText.Text = "Create Table OK";

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報を取得する（Send SQLite File）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Button_SendSQLite_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Send SQLite File ACCOUNT";
            MessageText.Text = "";
            ResultText.Text = "";

            MessageText.Text = "Click Send SQLite File ACCOUNT\r\n";

            await DisplayAlert("Disp", $"URL : {_sqlite.webServiceUrl}", "OK");

            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            try
            {
                //HttpResponseMessage response = sqliteTest.AsyncPostFileForWebAPI();
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(MessageText, _sqlite.GetSendFileContent());

                ResultText.Text += "Send SQLite file Finish : " + response.Result.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                ResultText.Text += "Send SQLite file Error : " + ex.Message;

            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// アカウント情報を元にDB情報を取得する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Button_SendSQLite_HOME_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Send SQLite File HOME";
            MessageText.Text = "";
            ResultText.Text = "";

            MessageText.Text = "Click Send SQLite File HOME\r\n";

            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            try
            {
                //HttpResponseMessage response = sqliteTest.AsyncPostFileForWebAPI();
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(MessageText, _sqlite.GetSendFileContent_HOME());

                ResultText.Text += "Send SQLite file Finish : " + response.Result.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                ResultText.Text += "Send SQLite file Error : " + ex.Message;

            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get File Names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_GetFileName_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Get File Names";
            MessageText.Text = "";
            ResultText.Text = "";

            //sqliteManager = SQLiteManager.GetInstance();
            string[] names = _sqlite.GetFileName();
            foreach (string name in names)
            {
                var lwTime = System.IO.File.GetLastWriteTime(name);
                ResultText.Text += name + " " + lwTime + "\r\n";
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get DB Table Names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_GetDBTableName_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Get DB Table Names";
            MessageText.Text = "";
            ResultText.Text = "";

            //sqliteManager = SQLiteManager.GetInstance();

            try
            {
                using (var db = new SQLite.SQLiteConnection(_sqlite.dbFile))
                {

                    const string cmdText = "SELECT name FROM sqlite_master WHERE type='table'";
                    var cmd = db.CreateCommand(cmdText);
                    //ResultText.Text = cmd.ExecuteScalar<string>();
                    var datas = cmd.ExecuteQueryScalars<string>().ToList();
                    foreach (string data in datas)
                    {
                        ResultText.Text += data + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Get DB Table Names Error : " + ex.Message;
            }


            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// SQLite関連（データ操作）
        ///////////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button4_Select_JOIN_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_JOIN_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select JOIN";
            MessageText.Text = "";
            ResultText.Text = "";

            _sqlite.SetAccount();

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(_sqlite.dbFile))
                {   // Select
                    foreach (Table.HOME_EVENT row in db.Query<Table.HOME_EVENT>(
                                        "SELECT " +
                                            "t1.DataNo, " +
                                            "t1.EventClass, " +
                                            "t1.EventDataNo, " +
                                            "t1.EventDate, " +
                                            "t1.ClubCode, " +
                                            "t1.MemberCode, " +
                                            "t1.Answer, " +
                                            "t1.CancelFlg, " +
                                            "t2.Title, " +
                                            "t3.MeetingName " +
                                        "FROM " +
                                            "T_EVENTRET t1 " +
                                        "LEFT OUTER JOIN " +
                                            "T_EVENT t2 " +
                                        "ON " +
                                            "t1.EventClass = '1' and " +
                                            "t1.EventDataNo = t2.DataNo " +
                                        "LEFT OUTER JOIN " +
                                            "T_MEETINGSCHEDULE t3 " +
                                        "ON " +
                                            "t1.EventClass = '2' and " +
                                            "t1.EventDataNo = t3.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "'"))
                    {
                        if (row.EventClass.Equals("1"))
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.Title}\r\n";
                        }
                        else
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.MeetingName}\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select JOIN Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Button4_Select_JOINT_Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_Select_JOINT_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Select JOINT";
            MessageText.Text = "";
            ResultText.Text = "";

            _sqlite.SetAccount();

            // データ取得
            try
            {
                using (var db = new SQLite.SQLiteConnection(_sqlite.dbFile))
                {   // Select
                    foreach (Table.HOME_EVENT row in db.Query<Table.HOME_EVENT>(
                                        "SELECT " +
                                            "t1.DataNo, " +
                                            "t1.EventClass, " +
                                            "t1.EventDataNo, " +
                                            "t1.EventDate, " +
                                            "t1.ClubCode, " +
                                            "t1.MemberCode, " +
                                            "t1.Answer, " +
                                            "t1.CancelFlg, " +
                                            "t2.Title, " +
                                            "t3.MeetingName " +
                                        "FROM " +
                                            "T_EVENTRET t1 " +
                                        "LEFT OUTER JOIN " +
                                            "T_EVENT t2 " +
                                        "ON " +
                                            "t1.EventClass = '1' and " +
                                            "t1.EventDataNo = t2.DataNo " +
                                        "LEFT OUTER JOIN " +
                                            "T_MEETINGSCHEDULE t3 " +
                                        "ON " +
                                            "t1.EventClass = '2' and " +
                                            "t1.EventDataNo = t3.DataNo " +
                                        "WHERE " +
                                            "t1.MemberCode = '" + _sqlite.Db_A_Account.MemberCode + "'"))
                    {
                        if (row.EventClass.Equals("1"))
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.Title}\r\n";
                        }
                        else
                        {
                            ResultText.Text += $"HOME_EVENT:\r\n " +
                                               $"{row.DataNo}, {row.EventClass}, {row.EventDate}, {row.EventDataNo}, {row.MeetingName}\r\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = "Select JOINT Error : " + ex.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Delete DB3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void Button_DeleteDB3_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// sqlite class
            //sqliteManager = SQLiteManager.GetInstance();

            /// label clear
            CommandText.Text = "Delete DB3";
            MessageText.Text = "";
            ResultText.Text = "";

            try
            {
                File.Delete(_sqlite.dbFile);
                ResultText.Text += "Delete finish : \r\n" + _sqlite.dbFile + "\r\n";
            }
            catch (IOException deleteError)
            {
                ResultText.Text += "Delete Error : " + deleteError.Message;
            }

            /// 終了時間表示
            DateTime edDt = DateTime.Now;
            EndText.Text = edDt.ToString();
        }


        private void CheckDB3File()
        {

        }


    }
}
