﻿using System;
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

            // ボタンコントロール初期値
            account.IsEnabled = false;
            home.IsEnabled = false;
            update.IsEnabled = false;

            // ボタンコントロール
            ControlButtonEnable();

        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 画面表示時の更新処理
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //try
            //{
            //    // SQLiteファイル存在チェック
            //    if (_sqlite.CheckFileDB3() == _sqlite.SQLITE_NOFILE)
            //    {
            //        // ファイルがない場合

            //        // SQLiteファイル＆ALLテーブル作成
            //        _sqlite.CreateTable_ALL();

            //    }
            //    // TOP情報取得
            //    GetTopInfoAsync();

            //    // ボタンコントロール
            //    this.account.IsEnabled = true;

            //}
            //catch (Exception ex)
            //{
            //    DisplayAlert("Alert", $"画面ロードエラー(OnAppearing) : {ex.Message}", "OK");
            //}
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// 画面制御

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ボタンコントロール
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void ControlButtonEnable()
        {

            try
            {
                // SQLiteファイル存在チェック
                if (_sqlite.CheckFileDB3() == _sqlite.SQLITE_NOFILE)
                {
                    // ファイルがない場合
                    DisplayAlert("Alert", $"エラーが発生しました。{Environment.NewLine}" +
                                           "アプリを終了して再起動してください。", "OK");

                    // SQLiteファイル＆ALLテーブル作成
                    //_sqlite.CreateTable_ALL();

                }
                else
                {
                    // ファイルがある場合
                    // 設定ファイル情報（A_SETTING）取得
                    _sqlite.GetSetting();
                    // 設定ファイル情報存在チェック
                    if (_sqlite.Db_A_Setting != null)
                    {
                        // アカウントボタンON
                        this.account.IsEnabled = true;

                        // 設定ファイル情報がある場合
                        // アカウント情報（A_ACCOUNT）取得
                        _sqlite.GetAccount();
                        // アカウント情報存在チェック
                        if (_sqlite.Db_A_Account != null)
                        {
                            // アカウント情報がある場合
                            // ホームボタン有効
                            this.home.IsEnabled = true;
                        }
                        // ボタンコントロール（アップデートボタン）
                        ControlUpdBtnEnable();
                    }
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", $"ボタン制御エラー : {ex.Message}", "OK");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ボタンコントロール（アップデートボタン）
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////////////////////
        void ControlUpdBtnEnable()
        {
            double dbVer = 0.0;
            double appVer = 0.0;

            try
            {
                // DB設定アプリバージョン取得
                dbVer = double.Parse(_sqlite.Db_A_Setting.VersionNo);
                // ローカルアプリバージョン取得
                appVer = double.Parse(((App)Application.Current).AppVersion);
                // バージョンチェック
                if (dbVer > appVer)
                {
                    // DB設定アプリバージョンの方が新しい場合
                    // ダウンロードボタン有効
                    this.update.IsEnabled = true;
                }
            }
            catch
            {
                throw;
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
                //_sqlite.DelFileDB3();

                // 空DB作成処理
                //_sqlite.CreateTable_ALL();

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
        //async void Button_Update_Clicked(object sender, System.EventArgs e)
        [Obsolete]
        void Button_Update_Clicked(object sender, System.EventArgs e)
        {
            // 処理中ダイアログ表示
            //            await ((App)Application.Current).DispLoadingDialog();
            Device.OpenUri(new Uri("http://ap.insat.co.jp/LionsApl/DownLoad.html"));

        }

        private void OnLogoTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Content.TopMenu();
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

            ResultText.Text = string.Empty;

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
            MessageText.Text = string.Empty;
            ResultText.Text = string.Empty;

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
        /// アカウント情報を元にHOME用DB情報を取得する
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
            MessageText.Text = string.Empty;
            ResultText.Text = string.Empty;

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
        /// TOP用DB情報を取得する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void Button_SendSQLite_TOP_Clicked(object sender, EventArgs e)
        {
            /// 開始時間表示
            DateTime stDt = DateTime.Now;
            StartText.Text = stDt.ToString();

            /// label clear
            CommandText.Text = "Send SQLite File HOME";
            MessageText.Text = string.Empty;
            ResultText.Text = string.Empty;

            MessageText.Text = "Click Send SQLite File HOME\r\n";

            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            try
            {
                //HttpResponseMessage response = sqliteTest.AsyncPostFileForWebAPI();
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(MessageText, _sqlite.GetSendFileContent_TOP());

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
            MessageText.Text = string.Empty;
            ResultText.Text = string.Empty;

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
            MessageText.Text = string.Empty;
            ResultText.Text = string.Empty;

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
            MessageText.Text = string.Empty;
            ResultText.Text = string.Empty;

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





        private async Task GetTopInfo()
        {
            await GetTopInfoAsync();
        }

        private async Task GetTopInfoAsync()
        {
            // 処理中ダイアログ表示
            await ((App)Application.Current).DispLoadingDialog();

            // DB情報取得処理
            try
            {
                // TOP情報取得
                Task<HttpResponseMessage> response = _sqlite.AsyncPostFileForWebAPI(_sqlite.GetSendFileContent_TOP());
            }
            catch
            {
                throw;
            }
        }


    }
}
